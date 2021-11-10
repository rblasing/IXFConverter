using System;


namespace IXFConverter
{
   public class IXFDataType
   {
      /* An 8-byte integer in the form specified by IXFTMFRM. It represents a
       * whole number between -9 223 372 036 854 775 808 and 9 223 372 036
       * 854 775 807. IXFCSBCP and IXFCDBCP are not significant, and should
       * be zero. IXFCLENG is not used, and should contain blanks.
       * 
       * SQL Server: INT */
      public const int BIGINT = 492;

      /* A variable-length character string. The maximum length of the string
       * is contained in the IXFCLENG field of the column descriptor record,
       * and cannot exceed 32 767 bytes. The string itself is preceded by a
       * current length indicator, which is a 4-byte integer specifying the
       * length of the string, in bytes. The string is in the code page
       * indicated by IXFCSBCP.
       * The following applies to BLOBs only: If IXFCSBCP is zero, the string
       * is bit data, and should not be translated by any transformation
       * program.
       * The following applies to CLOBs only: If IXFCDBCP is nonzero, the
       * string can also contain double-byte chars in the code page
       * indicated by IXFCDBCP.
       * 
       * SQL Server: VARCHAR(MAX) */
      public const int BLOB = 404;
      public const int CLOB = 408;

      /* A fixed-length field containing an SQLFILE structure with the
       * name_length and the name fields filled in. The length of the
       * structure is contained in the IXFCLENG field of the column
       * descriptor record, and cannot exceed 255 bytes. The file name is in
       * the code page indicated by IXFCSBCP. If IXFCDBCP is nonzero, the
       * file name can also contain double-byte chars in the code page
       * indicated by IXFCDBCP. If IXFCSBCP is zero, the file name is bit
       * data and should not be translated by any transformation program.
       * Since the length of the structure is stored in IXFCLENG, the actual
       * length of the original LOB is lost. IXF files with columns of type
       * BLOB_FILE, CLOB_FILE, or DBCLOB_FILE should not be used to recreate
       * the LOB field, since the LOB will be created with a length of
       * sql_lobfile_len. */
      public const int BLOB_FILE = 804;
      public const int CLOB_FILE = 808;
      public const int DBCLOB_FILE = 812;

      /* A fixed-length character string. The string length is contained in
       * the IXFCLENG field of the column descriptor record, and cannot
       * exceed 254 bytes. The string is in the code page indicated by
       * IXFCSBCP. If IXFCDBCP is nonzero, the string can also contain
       * double-byte chars in the code page indicated by IXFCDBCP.
       * If IXFCSBCP is zero, the string is bit data and should not be
       * translated by any transformation program.
       * 
       * SQL Server: VARCHAR(MAX) */
      public const int CHAR = 452;

      /* A point in time in accordance with the Gregorian calendar. Each date
       * is a 10-byte char string in International Standards
       * Organization (ISO) format: yyyy-mm-dd. The range of the year part
       * is 0001 to 9999. The range of the month part is 01 to 12.The range
       * of the day part is 01 to n, where n depends on the month, using the
       * usual rules for days of the month and leap year.Leading zeros
       * cannot be omitted from any part. IXFCLENG is not used, and should
       * contain blanks. Valid characters within DATE are invariant in all PC
       * ASCII code pages; therefore, IXFCSBCP and IXFCDBCP are not
       * significant, and should be zero.
       * 
       * SQL Server: DATETIME2 */
      public const int DATE = 384;

      /* A variable-length string of double-byte chars.The IXFCLENG
       * field in the column descriptor record specifies the maximum number
       * of double-byte chars in the string, and cannot exceed 16 383.
       * The string itself is preceded by a current length indicator, which
       * is a 4-byte integer specifying the length of the string in
       * double-byte chars (that is, the value of this integer is one
       * half the length of the string, in bytes). The string is in the DBCS
       * code page, as specified by IXFCDBCP in the C record.Since the string
       * consists of double-byte char data only, IXFCSBCP should be
       * zero.There are no surrounding shift-in or shift-out characters. */
      public const int DBCLOB = 412;

      /* A packed decimal number with precision P (as specified by the first
       * three bytes of IXFCLENG in the column descriptor record) and scale
       * S (as specified by the last two bytes of IXFCLENG). The length, in
       * bytes, of a packed decimal number is (P+2)/2. The precision must be
       * an odd number between 1 and 31, inclusive.The packed decimal number
       * is in the internal format specified by IXFTMFRM, where packed
       * decimal for the PC is defined to be the same as packed decimal for
       * the System/370. IXFCSBCP and IXFCDBCP are not significant, and
       * should be zero.
       * 
       * SQL Server: FLOAT[53] */
      public const int DECIMAL = 484;

      /* Either a long (8-byte) or short (4-byte) floating point number,
       * depending on whether IXFCLENG is set to eight or to four.The data is
       * in the internal machine form, as specified by IXFTMFRM.IXFCSBCP and
       * IXFCDBCP are not significant, and should be zero.Four-byte floating
       * point is not supported by the database manager.
       * 
       * SQL Server: FLOAT[53] */
      public const int FLOAT = 480;

      /* A fixed-length string of double-byte chars. The IXFCLENG field
       * in the column descriptor record specifies the number of double-byte
       * characters in the string, and cannot exceed 127. The actual length
       * of the string is twice the value of the IXFCLENG field, in bytes.The
       * string is in the DBCS code page, as specified by IXFCDBCP in the C
       * record. Since the string consists of double-byte char data
       * only, IXFCSBCP should be zero. There are no surrounding shift-in
       * or shift-out characters. */
      public const int GRAPHIC = 468;

      /* A 4-byte integer in the form specified by IXFTMFRM. It represents a
       * whole number between -2 147 483 648 and +2 147 483 647. IXFCSBCP and
       * IXFCDBCP are not significant, and should be zero. IXFCLENG is not
       * used, and should contain blanks.
       * 
       * SQL Server: INT */
      public const int INTEGER = 496;

      /* A variable-length character string. The maximum length of the string
       * is contained in the IXFCLENG field of the column descriptor record,
       * and cannot exceed 32 767 bytes.The string itself is preceded by a
       * current length indicator, which is a 2-byte integer specifying the
       * length of the string, in bytes. The string is in the code page
       * indicated by IXFCSBCP. If IXFCDBCP is nonzero, the string can also
       * contain double-byte chars in the code page indicated by
       * IXFCDBCP. If IXFCSBCP is zero, the string is bit data and should not
       * be translated by any transformation program.
       * 
       * SQL Server: VARCHAR[MAX] */
      public const int LONGVARCHAR = 456;

      /* A variable-length string of double-byte chars. The IXFCLENG
       * field in the column descriptor record specifies the maximum number
       * of double-byte chars for the string, and cannot exceed 16 383.
       * The string itself is preceded by a current length indicator, which
       * is a 2-byte integer specifying the length of the string in
       * double-byte chars (that is, the value of this integer is one
       * half the length of the string, in bytes). The string is in the DBCS
       * code page, as specified by IXFCDBCP in the C record.Since the string
       * consists of double-byte char data only, IXFCSBCP should be
       * zero. There are no surrounding shift-in or shift-out characters. */
      public const int LONG_VARGRAPHIC = 472;

      /* A 2-byte integer in the form specified by IXFTMFRM.It represents a
       * whole number between -32 768 and +32 767. IXFCSBCP and IXFCDBCP are
       * not significant, and should be zero.IXFCLENG is not used, and should
       * contain blanks.
       * 
       * SQL Server: SMALLINT */
      public const int SMALLINT = 500;

      /* A point in time in accordance with the 24-hour clock. Each time is
       * an 8-byte char string in ISO format: hh.mm.ss. The range of the
       * hour part is 00 to 24, and the range of the other parts is 00 to 59.
       * If the hour is 24, the other parts are 00. The smallest time is
       * 00.00.00, and the largest is 24.00.00. Leading zeros cannot be
       * omitted from any part. IXFCLENG is not used, and should contain
       * blanks. Valid characters within TIME are invariant in all PC ASCII
       * code pages; therefore, IXFCSBCP and IXFCDBCP are not significant,
       * and should be zero. */
      public const int TIME = 388;

      /* The date and time with microsecond precision. Each time stamp is a
       * character string of the form yyyy-mm-dd-hh.mm.ss.nnnnnn (year month
       * day hour minutes seconds microseconds). IXFCLENG is not used, and
       * should contain blanks. Valid characters within TIMESTAMP are
       * invariant in all PC ASCII code pages; therefore, IXFCSBCP and
       * IXFCDBCP are not significant, and should be zero.
       * 
       * SQL Server: DATETIME2 */
      public const int TIMESTAMP = 392;

      /* A variable-length character string. The maximum length of the
       * string, in bytes, is contained in the IXFCLENG field of the column
       * descriptor record, and cannot exceed 254 bytes. The string itself is
       * preceded by a current length indicator, which is a two-byte integer
       * specifying the length of the string, in bytes.The string is in the
       * code page indicated by IXFCSBCP. If IXFCDBCP is nonzero, the string
       * can also contain double-byte chars in the code page indicated
       * by IXFCDBCP. If IXFCSBCP is zero, the string is bit data and should
       * not be translated by any transformation program.
       * 
       * SQL Server: VARCHAR[MAX] */
      public const int VARCHAR = 448;

      /* A variable-length string of double-byte chars. The IXFCLENG
       * field in the column descriptor record specifies the maximum number
       * of double-byte chars in the string, and cannot exceed 127. The
       * string itself is preceded by a current length indicator, which is a
       * 2-byte integer specifying the length of the string in double-byte
       * characters (that is, the value of this integer is one half the
       * length of the string, in bytes). The string is in the DBCS code
       * page, as specified by IXFCDBCP in the C record.Since the string
       * consists of double-byte char data only, IXFCSBCP should be
       * zero.There are no surrounding shift-in or shift-out characters. */
      public const int VARGRAPHIC = 464;


      public object val;


      // variable length is stored in 2 bytes
      public static bool IsVarLen2(int type)
      {
         return (type == LONGVARCHAR  ||  type == LONG_VARGRAPHIC  ||
            type == VARCHAR  ||  type == VARGRAPHIC);
      }


      // variable length is stored in 4 bytes
      public static bool IsVarLen4(int type)
      {
         return (type == BLOB  ||  type == CLOB  ||  type == DBCLOB);
      }


      public IXFDataType(int type, byte[] data)
      {
         switch (type)
         {
            case CHAR:
               val = System.Text.Encoding.ASCII.GetString(data).TrimEnd();
               break;

            case CLOB:
               val = System.Text.Encoding.ASCII.GetString(data).TrimEnd();
               break;

            case DATE:
               String sd = System.Text.Encoding.ASCII.GetString(data).TrimEnd();

               val = new DateTime(int.Parse(sd.Substring(0, 4)),
                  int.Parse(sd.Substring(5, 2)),
                  int.Parse(sd.Substring(8)));

               break;

            case FLOAT:
               val = BitConverter.ToDouble(data, 0);
               break;

            case INTEGER:
               val = BitConverter.ToInt32(data, 0);
               break;

            case LONGVARCHAR:
               val = System.Text.Encoding.ASCII.GetString(data).TrimEnd();
               break;

            case TIMESTAMP:
               // 01234567890123456789012345
               // yyyy-mm-dd-hh.mm.ss.nnnnnn
               String st = System.Text.Encoding.ASCII.GetString(data);

               val = new DateTime(int.Parse(st.Substring(0, 4)),
                  int.Parse(st.Substring(5, 2)),
                  int.Parse(st.Substring(8, 2)),
                  int.Parse(st.Substring(11, 2)),
                  int.Parse(st.Substring(14, 2)),
                  int.Parse(st.Substring(17, 2)),
                  int.Parse(st.Substring(20, 3)));

               break;

            case VARCHAR:
               val = System.Text.Encoding.ASCII.GetString(data).TrimEnd();
               break;

            default:
               throw new NotSupportedException("Not implemented: " +
                  type.ToString());
         }
      }


      internal static int GetLength(int type)
      {
         switch (type)
         {
            case BIGINT: return 8;
            case DATE: return 10;
            case INTEGER: return 4;
            case SMALLINT: return 2;
            case TIMESTAMP: return 26;
            default:
               throw new ArgumentException("Unsupported type: " +
                  type.ToString());
         }
      }


      public static string GetName(int type)
      {
         switch (type)
         {
            case 492: return "BIGINT";
            case 404: return "BLOB";
            case 408: return "CLOB";
            case 804: return "BLOB_FILE";
            case 808: return "CLOB_FILE";
            case 812: return "DBCLOB_FILE";
            case 452: return "CHAR";
            case 384: return "DATE";
            case 412: return "DBCLOB";
            case 484: return "DECIMAL";
            case 480: return "FLOAT";
            case 468: return "GRAPHIC";
            case 496: return "INTEGER";
            case 456: return "LONGVARCHAR";
            case 472: return "LONG_VARGRAPHIC";
            case 500: return "SMALLINT";
            case 388: return "TIME";
            case 392: return "TIMESTAMP";
            case 448: return "VARCHAR";
            case 464: return "VARGRAPHIC";
            default:
               throw new ArgumentException();
         }
      }
   }
}
