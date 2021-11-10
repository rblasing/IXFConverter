using System;
using System.IO;


namespace IXFConverter
{
   public class IXFApplication : IXFBase
   {
      public int IXFARECL;           // 06-byte char   record length
      public string IXFARECT = "A";  // 01-byte char   record type = 'A'
      public string IXFAPPID;        // 12-byte char   application identifier
      public string IXFADATA;        // varying   variable application-specific data


      public IXFApplication(int len, BinaryReader br)
      {
         IXFARECL = len;
         IXFAPPID = ReadChars(br, 12);
         IXFADATA = ReadChars(br, len - 13);
      }
   }
}
