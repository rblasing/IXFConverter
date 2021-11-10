using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;


/// <summary>
/// DB/2 databases are case-sensitive by default, whereas SQL Server DBs are not.
/// We need to enable sensitivity using
///
///     ALTER DATABASE dts COLLATE Latin1_General_CS_AS;
/// 
/// else we might get duplicate key errors.
/// </summary>
namespace IXFConverter
{
   class Program
   {
      static void Main(string[] args)
      {
         string[] files = GetFiles(args);

         if (files == null)
            return;

         DbConnection dbConn = null;
         DbTransaction t = null;

         try
         {
            dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["destDb"].ConnectionString);
            dbConn.Open();

            t = dbConn.BeginTransaction();

            foreach (string f in files)
            {
               if (!ImportIXFFile(dbConn, t, f))
               {
                  t.Rollback();
                  t.Dispose();
                  t = null;

                  return;
               }
            }

            t.Commit();
            t.Dispose();
            t = null;
         }
         catch (Exception e)
         {
            Log(e.Message);
            Log(e.StackTrace);
         }
         finally
         {
            if (t != null)
            {
               t.Rollback();
               t.Dispose();
               t = null;
            }

            if (dbConn != null)
            {
               dbConn.Close();
               dbConn.Dispose();
            }
         }
      }


      public static string[] GetFiles(string[] args)
      {
         // args[0] is path to IXF files, e.g. c:\data\source\ibm\eduquest\DB2 Exports\DTS
         if (args.Length != 1)
         {
            Console.WriteLine("Usage: IXFConverter <pathToIxfFiles>");

            return null;
         }

         if (!System.IO.Directory.Exists(args[0]))
         {
            Console.WriteLine("Cannot locate " + args[0]);

            return null;
         }

         string[] files = Directory.GetFiles(args[0], "*.ixf");

         if (files == null  ||  files.Length == 0)
         {
            Console.WriteLine("No IXF files in " + args[0]);

            return null;
         }

         return files;
      }


      private static void Log(string s)
      {
         Console.WriteLine(s);
      }


      public static bool ImportIXFFile(DbConnection dbConn, DbTransaction t,
         string filename)
      {
         FileStream sr = new FileStream(filename, FileMode.Open);
         BinaryReader br = new BinaryReader(sr);

         IXFHeader header = null;
         IXFTable table = null;
         List<IXFColumn> columns = new List<IXFColumn>();
         List<IXFRecord> rows = new List<IXFRecord>();
         List<IXFApplication> apps = new List<IXFApplication>();

         while (br.BaseStream.Position != br.BaseStream.Length)
         {
            string l = new string(br.ReadChars(6));
            int recLen = int.Parse(l);
            string rectype = new string(br.ReadChars(1));

            switch (rectype)
            {
               case "H":
                  header = new IXFHeader(recLen, br);
                  break;

               case "T":
                  table = new IXFTable(recLen, br);
                  break;

               case "C":
                  IXFColumn c = new IXFColumn(recLen, br);
                  columns.Add(c);
                  Console.WriteLine(c.ToString());
                  break;

               case "D":
                  IXFRecord r = new IXFRecord(recLen, br);
                  rows.Add(r);
                  break;

               case "A":
                  IXFApplication a = new IXFApplication(recLen, br);
                  apps.Add(a);
                  break;
            }
         }

         br.Close();
         sr.Close();

         // remove .ixf from end of table name
         int end = table.IXFTNAME.IndexOf(".");
         string tname = table.IXFTNAME;

         if (end >= 0)
            tname = tname.Substring(0, tname.IndexOf("."));

         // create the table
         if (!CreateTable(dbConn, t, tname, columns))
            return false;

         // insert data into the table
         return InsertRows(dbConn, t, tname, rows, columns);
      }


      public static bool CreateTable(DbConnection dbConn, DbTransaction t,
         string tablename, List<IXFColumn> columns)
      {
         bool hasConstraint = false;
         string dml = $"CREATE TABLE {tablename} (";
         string constraint = $"CONSTRAINT PK_{tablename} PRIMARY KEY (";

         foreach (IXFColumn c in columns)
         {
            if (c.IXFCKEY)
            {
               hasConstraint = true;
               constraint += c.IXFCNAME + ",";
            }

            dml += c.IXFCNAME + " ";

            switch (c.IXFCTYPE)
            {
               case IXFDataType.CHAR:
                  dml += "CHAR(" + c.IXFCLENG.ToString() + ")";
                  break;

               case IXFDataType.CLOB:
                  dml += "VARCHAR(MAX)";
                  break;

               case IXFDataType.DATE:
                  dml += "DATETIME2";
                  break;

               case IXFDataType.FLOAT:
                  dml += "FLOAT(53)";
                  break;

               case IXFDataType.INTEGER:
                  dml += "INT";
                  break;

               case IXFDataType.LONGVARCHAR:
                  dml += "VARCHAR(MAX)";
                  break;

               case IXFDataType.TIMESTAMP:
                  dml += "DATETIME2";
                  break;

               case IXFDataType.VARCHAR:
                  dml += "VARCHAR(" + c.IXFCLENG.ToString() + ")";
                  break;

               default:
                  throw new ArgumentException();
            }

            dml += (c.IXFCNULL ? "," : " NOT NULL,");
         }

         if (hasConstraint)
         {
            if (constraint[constraint.Length - 1] == ',')
               constraint = constraint.Substring(0, constraint.Length - 1);

            constraint += ")";
            dml += constraint;
         }
         else
         {
            if (dml[dml.Length - 1] == ',')
               dml = dml.Substring(0, dml.Length - 1);
         }

         dml += ")";

         Log(dml);

         try
         {
            DbCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = dml;
            cmd.Transaction = t;
            cmd.ExecuteNonQuery();
         }
         catch (Exception e)
         {
            t.Rollback();
            Log(e.Message);

            return false;
         }

         return true;
      }


      public static bool InsertRows(DbConnection dbConn, DbTransaction tran,
         string tableName, List<IXFRecord> rows, List<IXFColumn> columns)
      {
         int rowNum = 0;
         int colNum = 0;
         object[] fields = null;

         foreach (var row in rows)
         {
            // some rows are stored using multiple IXFRecords
            if (row.IXFDRID == 1)
            {
               Log("Row " + rowNum);
               rowNum++;
               colNum = 0;
               fields = new object[columns.Count];
            }

            // record 1 might have columns 0-6, with record 2 containing
            // columns 7-end, so start with the next column index after
            // the previous record
            for (int colIdx = colNum; colIdx < columns.Count; colIdx++)
            {
               IXFColumn c = columns[colIdx];

               bool isNull = false;
               byte[] nullInd = new byte[2];
               Byte[] data = null;
               int dataLen = c.IXFCLENG;
               int startPos = c.IXFCPOSN;

               // null indicator is 2 bytes
               if (c.IXFCNULL)
               {
                  Array.Copy(row.IXFDCOLS, startPos, nullInd, 0, 2);
                  startPos += 2;

                  if (nullInd[0] == 0x00  &&  nullInd[1] == 0x00)
                     isNull = false;
                  else if (nullInd[0] == 0xFF  &&  nullInd[1] == 0xFF)
                     isNull = true;
                  else
                     throw new Exception("Unexpected nullable value");
               }

               if (!isNull)
               {
                  // get the number of bytes used by variable-length datatypes
                  if (c.isVarLen2)
                  {
                     byte[] varLen = new byte[2];
                     Array.Copy(row.IXFDCOLS, startPos, varLen, 0, 2);
                     startPos += 2;

                     if (!BitConverter.IsLittleEndian)
                        Array.Reverse(varLen);

                     dataLen = BitConverter.ToInt16(varLen, 0);
                  }

                  if (c.isVarLen4)
                  {
                     byte[] varLen = new byte[4];
                     Array.Copy(row.IXFDCOLS, startPos, varLen, 0, 4);
                     startPos += 4;

                     if (!BitConverter.IsLittleEndian)
                        Array.Reverse(varLen);

                     dataLen = BitConverter.ToInt32(varLen, 0);
                  }

                  data = new byte[dataLen];
                  Array.Copy(row.IXFDCOLS, startPos, data, 0, dataLen);

                  IXFDataType dt = new IXFDataType(c.IXFCTYPE, data);
                  Console.WriteLine(c.IXFCNAME + ": " + dt.val.ToString());
                  fields[colNum] = dt.val;
               }
               else
               {
                  Log(c.IXFCNAME + ": NULL");
               }

               // we're at the last column in this row, so write it to the DB
               if (colIdx + 1 == columns.Count)
                  if (!InsertRow(dbConn, tran, tableName, columns, fields))
                     return false;

               colNum++;

               // the next column starts the next IXFRecord, so break out of
               // this loop and move on
               if (colIdx + 1 < columns.Count  &&
                  columns[colIdx + 1].IXFCDRID != row.IXFDRID)
               {
                  break;
               }
            }
         }

         // all rows read
         return true;
      }


      public static bool InsertRow(DbConnection dbConn, DbTransaction t,
         string tablename, List<IXFColumn> columns, object[] fields)
      {
         DbCommand cmd = null;

         try
         {
            cmd = dbConn.CreateCommand();
            string sql = $"INSERT INTO {tablename} (";

            for (int idx = 0; idx < columns.Count - 1; idx++)
               sql += columns[idx].IXFCNAME + ",";

            sql += columns[columns.Count - 1].IXFCNAME + ") VALUES (";

            for (int idx = 0; idx < columns.Count - 1; idx++)
               sql += "@" + columns[idx].IXFCNAME.Replace("[", "").Replace("]", "") + ",";

            sql += "@" + columns[columns.Count - 1].IXFCNAME.Replace("[", "").Replace("]", "") + ")";

            for (int idx = 0; idx < columns.Count; idx++)
            {
               DbParameter p = BuildParameter(cmd,
                  columns[idx].IXFCNAME.Replace("[", "").Replace("]", ""),
                  fields[idx], columns[idx].IXFCTYPE, columns[idx].IXFCLENG);

               cmd.Parameters.Add(p);
            }

            cmd.Connection = dbConn;
            cmd.CommandText = sql;
            cmd.Transaction = t;
            cmd.ExecuteNonQuery();
         }
         catch (Exception e)
         {
            t.Rollback();
            Log(e.Message);

            return false;
         }
         finally
         {
            if (cmd != null)
            {
               cmd.Dispose();
               cmd = null;
            }
         }

         return true;
      }


      public static DbParameter BuildParameter(DbCommand cmd, string name,
         object value, int dataType, int len)
      {
         DbParameter p = cmd.CreateParameter();
         p.ParameterName = "@" + name;

         if (value == null)
            p.Value = DBNull.Value;
         else
            p.Value = value;

         switch (dataType)
         {
            case IXFDataType.CHAR:
               p.DbType = System.Data.DbType.String;
               p.Size = len;
               break;

            case IXFDataType.CLOB:
               p.DbType = System.Data.DbType.String;
               p.Size = len;
               break;

            case IXFDataType.DATE:
               p.DbType = System.Data.DbType.DateTime2;
               break;

            case IXFDataType.FLOAT:
               p.DbType = System.Data.DbType.Double;
               break;

            case IXFDataType.INTEGER:
               p.DbType = System.Data.DbType.Int32;
               break;

            case IXFDataType.LONGVARCHAR:
               p.DbType = System.Data.DbType.String;
               p.Size = len;
               break;

            case IXFDataType.TIMESTAMP:
               p.DbType = System.Data.DbType.DateTime2;
               break;

            case IXFDataType.VARCHAR:
               p.DbType = System.Data.DbType.String;
               p.Size = len;
               break;

            default:
               throw new ArgumentException("Unknown IXF column type: " + dataType);
         }

         return p;
      }
   }
}
