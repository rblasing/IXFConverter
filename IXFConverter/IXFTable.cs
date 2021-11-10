using System;
using System.IO;


namespace IXFConverter
{
   public class IXFTable : IXFBase
   {
      public int IXFTRECL;           // 006-byte char   record length
      public string IXFTRECT = "T";  // 001-byte char   record type = 'T'
      public int IXFTNAML;           // 002-byte char   name length
      public string IXFTNAME;        // 018-byte char   name of data
      public string IXFTQUAL;        // 008-byte char   qualifier
      public string IXFTSRC;         // 012-byte char   data source
      public string IXFTDATA;        // 001-byte char   data convention = 'C'
      public string IXFTFORM;        // 001-byte char   data format = 'M'
      public string IXFTMFRM;        // 005-byte char   machine format = 'PC'
      public string IXFTLOC;         // 001-byte char   data location = 'I'
      public int IXFTCCNT;           // 005-byte char   'C' record count
      public string IXFTFIL1;        // 002-byte char   reserved
      public string IXFTDESC;        // 030-byte char   data description


      public IXFTable(int len, BinaryReader br)
      {
         IXFTRECL = len;
         IXFTNAML = int.Parse(ReadChars(br, 2));
         IXFTNAME = ReadChars(br, 18);
         IXFTQUAL = ReadChars(br, 8);
         IXFTSRC = ReadChars(br, 12);
         IXFTDATA = ReadChars(br, 1);
         IXFTFORM = ReadChars(br, 1);
         IXFTMFRM = ReadChars(br, 5);
         IXFTLOC = ReadChars(br, 1);
         IXFTCCNT = int.Parse(ReadChars(br, 5));
         IXFTFIL1 = ReadChars(br, 2);
         IXFTDESC = ReadChars(br, 30);
      }
   }
}
