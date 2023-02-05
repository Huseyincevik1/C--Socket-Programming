using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace denemesocket
{
    public class DosyaHash
    {
        public string fileName { get; internal set; }
        public string filePath { get; internal set; }

        public static string FileToHash(byte[] fileBytes)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                // Compute the hash of the fileStream.
                byte[] hashValue = mySHA256.ComputeHash(fileBytes);
                // Write the name and hash value of the file to the console.

                return ByteArrayToString(hashValue);

            }
        }

        private static string ByteArrayToString(byte[] array)
        {
            var hash = "";
            for (int i = 0; i < array.Length; i++)
            {
                hash += array[i].ToString("x2");
            }
            return hash;
        }

    }
}
