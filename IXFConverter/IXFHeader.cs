using System;
using System.IO;


namespace IXFConverter
{
   public class IXFHeader : IXFBase
   {
      public int IXFHRECL;          // 06-byte char   record length
      public string IXFHRECT = "H"; // 01-byte char   record type = 'H'
      public string IXFHID;         // 03-byte char   IXF identifier = 'IXF'
      public string IXFHVERS;       // 04-byte char   IXF version = '0001'
      public string IXFHPROD;       // 12-byte char   product
      public string IXFHDATE;       // 08-byte char   date written   yyyymmdd
      public string IXFHTIME;       // 06-byte char   time written   hhmmss
      public int IXFHHCNT;          // 05-byte char   heading record count
      public string IXFHSBCP;       // 05-byte char   single byte code page
      public string IXFHDBCP;       // 05-byte char   double byte code page
      public string IXFHFIL1;       // 02-byte char   reserved


      public IXFHeader(int len, BinaryReader br)
      {
         IXFHRECL = len;
         IXFHID = ReadChars(br, 3);
         IXFHVERS = ReadChars(br, 4);
         IXFHPROD = ReadChars(br, 12);
         IXFHDATE = ReadChars(br, 8);
         IXFHTIME = ReadChars(br, 6);
         IXFHHCNT = int.Parse(ReadChars(br, 5));
         IXFHSBCP = ReadChars(br, 5);
         IXFHDBCP = ReadChars(br, 5);
         IXFHFIL1 = ReadChars(br, 2);
      }
   }
}
