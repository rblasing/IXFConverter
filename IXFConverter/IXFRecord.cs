using System;
using System.IO;


namespace IXFConverter
{
   public class IXFRecord : IXFBase
   {
      public int IXFDRECL;           // 06-byte char   record length
      public string IXFDRECT = "D";  // 01-byte char   record type = 'D'
      public int IXFDRID;            // 03-byte char   'D' record identifier
      public string IXFDFIL1;        // 04-byte char   reserved
      public byte[] IXFDCOLS;        // varying variable    columnar data


      public IXFRecord(int len, BinaryReader br)
      {
         IXFDRECL = len;
         IXFDRID = int.Parse(ReadChars(br, 3));
         IXFDFIL1 = ReadChars(br, 4);
         IXFDCOLS = ReadBytes(br, len - 8);
      }
   }
}
