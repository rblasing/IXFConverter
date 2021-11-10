using System;
using System.IO;


namespace IXFConverter
{
   public class IXFColumn : IXFBase
   {
      public int IXFCRECL;           // 006-byte char   record length
      public string IXFCRECT = "C";  // 001-byte char   record type = 'C'
      public int IXFCNAML;           // 002-byte char   column name length
      public string IXFCNAME;        // 018-byte char   column name, ends with .ixf
      public bool IXFCNULL;          // 001-byte char   column allows nulls  Y/N
      public string IXFCSLCT;        // 001-byte char   column selected flag = 'Y'
      public bool IXFCKEY;           // 001-byte char   key column flag  Y/N
      public string IXFCCLAS;        // 001-byte char   data class = 'R'
      public int IXFCTYPE;           // 003-byte char   data type  IXFDataType
      public string IXFCSBCP;        // 005-byte char   single byte code page
      public string IXFCDBCP;        // 005-byte char   double byte code page
      public int IXFCLENG;           // 005-byte char   column data length, int or 3byte precision, 2byte scale
      public int IXFCDRID;           // 003-byte char   'D' record identifier
      public int IXFCPOSN;           // 006-byte char   column position
      public string IXFCDESC;        // 030-byte char   column description
      public string IXFCNDIM;        // 002-byte char   number of dimensions = 0
      public string IXFCDSIZ;        // varying char    size of each dimension

      // is the length for variable-length data types stored in a 2- or 4-byte field?
      public bool isVarLen2;
      public bool isVarLen4;


      public IXFColumn(int len, BinaryReader br)
      {
         IXFCRECL = len;
         IXFCNAML = int.Parse(ReadChars(br, 2));
         IXFCNAME = ReadChars(br, 18);
         IXFCNULL = ReadChars(br, 1) == "Y";
         IXFCSLCT = ReadChars(br, 1);
         IXFCKEY = ReadChars(br, 1) != "N";
         IXFCCLAS = ReadChars(br, 1);
         IXFCTYPE = int.Parse(ReadChars(br, 3));
         IXFCSBCP = ReadChars(br, 5);
         IXFCDBCP = ReadChars(br, 5);

         string l = ReadChars(br, 5);

         if (l != "")
            IXFCLENG = int.Parse(l);
         else
            IXFCLENG = IXFDataType.GetLength(IXFCTYPE);

         IXFCDRID = int.Parse(ReadChars(br, 3));

         // IXF uses 1-based index, so decrement to align with C#'s 0-based worldview
         IXFCPOSN = int.Parse(ReadChars(br, 6)) - 1;

         IXFCDESC = ReadChars(br, 30);
         IXFCNDIM = ReadChars(br, 2);

         isVarLen2 = IXFDataType.IsVarLen2(IXFCTYPE);
         isVarLen4 = IXFDataType.IsVarLen4(IXFCTYPE);

         // escape SQL Server reserved words
         if (IXFCNAME == "DESC")
            IXFCNAME = "[DESC]";
      }


      public override string ToString()
      {
         return string.Format(
            "Name: {0}\r\nNullable: {1}\r\nKey: {2}\r\nType: {3}\r\nLength: {4}",
            IXFCNAME, IXFCNULL, IXFCKEY, IXFDataType.GetName(IXFCTYPE), IXFCLENG);
      }
   }
}
