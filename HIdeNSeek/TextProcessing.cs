using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HideNSeek
{
    class TextProcessing
    {
        // IV for AES encryption
        private static string IV = "1234567812345678";

        public static int InsertMsg(string rawInput, Bitmap imgMap)
        {
            // detect if a key input exist in the raw data
            string key = KeyExtract(rawInput);
            
            // detect if the user input an index tag
            int[] indexList = IndexExtract(rawInput);

            byte[] textInByte;

            // if the user inputs a key, encrypted the text input
            if (!string.IsNullOrEmpty(key))
            {
                using (Aes myAes = Aes.Create())
                {
                    // the key input might not be 256 bits which is required by the AES standard
                    // therefore, it is important to hash the key into a 256 bits long input
                    key = CryptoHandler.Sha256Hash(key);

                    // Encrypt the string to an array of bytes.
                    byte[] plainByte = CryptoHandler.EncryptStringToBytes_Aes(rawInput, Encoding.ASCII.GetBytes(key), Encoding.UTF8.GetBytes(IV));
                    byte[] str = Encoding.UTF8.GetBytes("<STR>");
                    byte[] end = Encoding.UTF8.GetBytes("<END>");
                    textInByte = new byte[str.Length + plainByte.Length + end.Length];
                    Buffer.BlockCopy(str, 0, textInByte, 0, str.Length);
                    Buffer.BlockCopy(plainByte, 0, textInByte, str.Length, plainByte.Length);
                    Buffer.BlockCopy(end, 0, textInByte, end.Length + plainByte.Length, end.Length);
                }
            }
            else
            {
                // prolong the plaintext with the header
                string plaintext = "<STR>" + rawInput + "<END>";
                textInByte = Encoding.UTF8.GetBytes(plaintext);
            }

            long availableWidth = (imgMap.Width - indexList[1]) / indexList[3];
            long availableHeight = (imgMap.Height - indexList[0]) / indexList[2];
            long availableSpace = (availableWidth + availableHeight) * (indexList[4] + indexList[5] + indexList[6]);
            if (availableSpace >= textInByte.Length * 8 && availableHeight > 0 && availableWidth > 0)
            {
                ImgProcessing.InsertByte(textInByte, imgMap, indexList);
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public static string ExtractMsg(string rawInput, Bitmap imgMap)
        {
            // pre-processing the hidden message
            string key = KeyExtract(rawInput);
            
            int[] indexList = IndexExtract(rawInput);
            
            byte[] textInByte = ImgProcessing.ExtractByte(imgMap, indexList);

            if (!string.IsNullOrEmpty(key))
            {
                using (Aes myAes = Aes.Create())
                {
                    key = CryptoHandler.Sha256Hash(key);
                    // Decrypt the bytes to a string.
                    return CryptoHandler.DecryptStringFromBytes_Aes(textInByte, Encoding.ASCII.GetBytes(key), Encoding.UTF8.GetBytes(IV));
                }
            }
            else
            {
                return Encoding.UTF8.GetString(textInByte);
            }
        }

        // return key value user inputed
        private static string KeyExtract(string rawInput)
        {
            return new Regex(@"(?<=<KEY>)\w+(?=<KEY>)", RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(rawInput).Groups[0].ToString();
        }

        // return index value user inputed
        private static int[] IndexExtract(string rawInput)
        {
            // x, y, increment x, increment y, R enable, G enable, B enable
            int[] indexList = { 0, 0, 1, 1, 1, 1, 1 };

            // reduce ...<IDX>[0-9,]*<IDX>... to [0-9,]*<IDX>
            string indexText = new Regex(@"(?<=<IDX>)[0-9,]*<IDX>", RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(rawInput).Groups[0].ToString();

            // reduce <IDX>[0-9,]*<IDX> to array of [0-9]
            MatchCollection matches = new Regex(@"\d+(?=,)|\d+(?=<IDX>)", RegexOptions.Compiled | RegexOptions.IgnoreCase).Matches(indexText);
            if (matches.Count > indexList.Length)
            {
                MessageBox.Show("Num of Index is not correct, index input will remain in default mode", "alert");
            }
            else
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    indexList[i] = int.Parse(matches[i].Groups[0].ToString());
                    Console.WriteLine(indexList[i]);
                }
            }
            return indexList;
        }

    }
}
