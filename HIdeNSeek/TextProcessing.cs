using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace HideNSeek
{
    class TextProcessing
    {
        private static string IV = "1234567812345678";

        public static void InsertMsg(string rawInput, Bitmap imgMap)
        {
            // pre-processing the hidden message
            string key = KeyDetect(ref rawInput);
            
            int[] indexList = IndexDetect(ref rawInput);

            Console.WriteLine(rawInput);

            byte[] textInByte;

            if (!string.IsNullOrEmpty(key))
            {
                using (Aes myAes = Aes.Create())
                {
                    key = Crypto.Sha256Hash(key);
                    Console.WriteLine(Encoding.ASCII.GetBytes(key).Length);
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
                string plaintext = "<STR>" + rawInput + "<END>";
                textInByte = Encoding.UTF8.GetBytes(plaintext);
            }

            ImgProcessing.InsertByte(textInByte, imgMap, indexList);

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
            int[] indexList = { 0, 0, 1, 1 };
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
