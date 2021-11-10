using System;
using System.IO;


namespace IXFConverter
{
   public class IXFBase
   {
      public static string ReadChars(BinaryReader br, int len, bool varLen = false)
      {
         char[] buf = new char[len];
         int idx = 0;

         while (idx < len)
         {
            char c = br.ReadChar();

            if (varLen  &&  c == '\0')
               break;
            else
               buf[idx] = c;

            idx++;
         }

         return new string(buf).Replace("\0", "").Trim();
      }


      public byte[] ReadBytes(BinaryReader br, int len, bool varLen = false)
      {
         byte[] buf = new byte[len];
         int idx = 0;

         while (idx < len)
         {
            byte c = br.ReadByte();

            if (varLen  &&  c == '\0')
               break;
            else
               buf[idx] = c;

            idx++;
         }

         return buf;
      }
   }
}
