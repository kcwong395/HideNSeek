using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace HideNSeek
{
    class TextProcessing
    {
        private static string IV = "1234567812345678";

        public static byte[] HideInput(string rawInput)
        {
            // pre-processing the hidden message
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
            /*
            int idxSTR = rawInput.IndexOf("<IDX>");
            if (idxSTR >= 0)
            {
                int idxEND = rawInput.IndexOf("ENDIDX");
                if(idxEND >= 0)
                {
                    string tmp = rawInput.Substring(idxSTR + 5, idxEND - (idxSTR + 5));
                    int.TryParse(tmp, out index);
                    rawInput = rawInput.Substring(idxSTR + 5, idxEND + 8 - idxSTR);
                }
            }
            */

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
                    System.Buffer.BlockCopy(str, 0, textInByte, 0, str.Length);
                    System.Buffer.BlockCopy(plainByte, 0, textInByte, str.Length, plainByte.Length);
                    System.Buffer.BlockCopy(end, 0, textInByte, end.Length + plainByte.Length, end.Length);
                }
            }
            else
            {
                string plaintext = "<STR>" + rawInput + "<END>";
                textInByte = Encoding.UTF8.GetBytes(plaintext);
            }
            
            return textInByte;
        }

        public static string SeekOutput(string key, byte[] textInByte)
        {
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
    }
}
