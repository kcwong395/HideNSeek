using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace HideNSeek
{
    class TextProcessing
    {
        // IV for AES encryption
        private static string IV = "1234567812345678";

        public static int InsertMsg(string rawInput, Bitmap imgMap)
        {

            // detect if a key input exist in the raw data
            string key = KeyDetect(ref rawInput);
            
            // detect if the user input an index tag
            int[] indexList = IndexDetect(ref rawInput);

            byte[] textInByte;

            // if the user inputs a key, encrypted the text input
            if (!string.IsNullOrEmpty(key))
            {
                using (Aes myAes = Aes.Create())
                {
                    // the key input might not be 256 bits which is required by the AES standard
                    // therefore, it is important to hash the key into a 256 bits long input
                    key = Crypto.Sha256Hash(key);

                    // Encrypt the string to an array of bytes.
                    byte[] plainByte = Crypto.EncryptStringToBytes_Aes(rawInput, Encoding.ASCII.GetBytes(key), Encoding.UTF8.GetBytes(IV));
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
            string key = KeyDetect(ref rawInput);
            
            int[] indexList = IndexDetect(ref rawInput);
            
            byte[] textInByte = ImgProcessing.ExtractByte(imgMap, indexList);

            if (!string.IsNullOrEmpty(key))
            {
                using (Aes myAes = Aes.Create())
                {
                    key = Crypto.Sha256Hash(key);
                    // Decrypt the bytes to a string.
                    return Crypto.DecryptStringFromBytes_Aes(textInByte, Encoding.ASCII.GetBytes(key), Encoding.UTF8.GetBytes(IV));
                }
            }
            else
            {
                return Encoding.UTF8.GetString(textInByte);
            }
        }

        private static string KeyDetect(ref string rawInput)
        {
            string key = "";

            int keySTR = rawInput.IndexOf("<KEY>");
            if (keySTR >= 0)
            {
                int keyEND = rawInput.IndexOf("<ENDKEY>");
                if (keyEND >= 0)
                {
                    key = rawInput.Substring(keySTR + 5, keyEND - (keySTR + 5));
                    rawInput = rawInput.Remove(keySTR, keyEND + 8 - keySTR);
                }
            }

            return key;

        }

        private static int[] IndexDetect(ref string rawInput)
        {
            // x, y, increment x, increment y, R enable, G enable, B enable
            int[] indexList = { 0, 0, 1, 1, 1, 1, 1 };
            int index = 0;

            int idxSTR = rawInput.IndexOf("<IDX>");
            if (idxSTR >= 0)
            {
                int idxEND = rawInput.IndexOf("<ENDIDX>");
                if (idxEND >= 0)
                {
                    string indexInput = rawInput.Substring(idxSTR + 5, idxEND - (idxSTR + 5));
                    int commaIndex = indexInput.IndexOf(',');
                    while (commaIndex > 0)
                    {
                        string num = indexInput.Substring(0, commaIndex);
                        int.TryParse(num, out indexList[index++]);
                        indexInput = indexInput.Substring(commaIndex + 1, indexInput.Length - (num.Length + 1));
                        commaIndex = indexInput.IndexOf(',');
                    }
                    int.TryParse(indexInput.Substring(0, indexInput.Length), out indexList[index++]);
                    rawInput = rawInput.Remove(idxSTR, idxEND + 8 - idxSTR);
                }
            }

            return indexList;

        }

    }
}
