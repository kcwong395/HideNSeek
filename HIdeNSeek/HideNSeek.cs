using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace HideNSeek
{
    public partial class HideNSeek : Form
    {
        private Bitmap imgMap;
        private string imgPath;
        private static string IV = "1234567812345678";

        // default constructor
        public HideNSeek()
        {
            InitializeComponent();
        }

        // application is opened with a file passing in
        public HideNSeek(string[] imgs)
        {
            InitializeComponent();

            string extension = Path.GetExtension(imgs[0]).ToLower();
            if (extension != ".png" && extension != ".jpg")
            {
                status.Text = "File Selected is not an image";
            }
            else
            {
                status.Text = "File Selected: " + imgs[0];
                imgMap = new Bitmap(imgs[0]);
                imgPath = imgs[0];
                textBox1.Text = "";
            }
        }

        private void HideNSeek_Load(object sender, EventArgs e)
        {

        }
        
        private void Open_Click(object sender, EventArgs e)
        {
            // open the file browser
            OpenFileDialog dlg = new OpenFileDialog
            {
                Title = "Select an image",
                InitialDirectory = "C:/"
            };

            // The following would not return the dialog if the current
            // thread is not STA
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(dlg.FileName).ToLower();
                if (extension != ".png" && extension != ".jpg")
                {
                    status.Text = "File Selected is not an image";
                }
                else
                {
                    Reinitialize("", "File Selected: " + dlg.FileName, dlg.FileName, new Bitmap(dlg.FileName));
                }
            }
        }

        private void Hide_Click(object sender, EventArgs e)
        {
            // check if any file is selected
            if(string.IsNullOrEmpty(imgPath))
            {
                return;
            }

            // pre-processing the hidden message
            string rawInput = textBox1.Text;
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
            byte[] textInBin;

            if (!(string.IsNullOrEmpty(key)))
            {
                using (Aes myAes = Aes.Create())
                {
                    // Encrypt the string to an array of bytes.
                    byte[] plainByte = Crypto.EncryptStringToBytes_Aes(rawInput, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(IV));
                    byte[] str = Encoding.UTF8.GetBytes("<STR>");
                    byte[] end = Encoding.UTF8.GetBytes("<END>");
                    textInBin = new byte[str.Length + plainByte.Length + end.Length];
                    System.Buffer.BlockCopy(str, 0, textInBin, 0, str.Length);
                    System.Buffer.BlockCopy(plainByte, 0, textInBin, str.Length, plainByte.Length);
                    System.Buffer.BlockCopy(end, 0, textInBin, end.Length + plainByte.Length, end.Length);
                }
            }
            else
            {
                string plaintext = "<STR>" + rawInput + "<END>";
                textInBin = Encoding.UTF8.GetBytes(plaintext);
            }

            foreach(byte b in textInBin)
            {
                Console.WriteLine(b);
            }
            Console.WriteLine("end");

            // ensure that the photo has enough space for the message
            if (textInBin.Length > (imgMap.Width * imgMap.Height * 3) / 8.0)
            {
                status.Text = "Not Enough Space";
                return;
            }

            // insert the hidden message
            InsertMsg(textInBin, imgMap);
            
            // save the picture with hidden message
            string newPath = Directory.GetParent(imgPath).ToString() + '/' + Path.GetFileNameWithoutExtension(imgPath) + "_copy.png";
            imgMap.Save(newPath, ImageFormat.Png);

            // now the user selects no image
            Reinitialize("", "Waiting for input...", "", null);
        }

        private void InsertMsg(byte[] textInBin, Bitmap imgMap)
        {
            int i = 0, j = 7;
            for (int x = 0; x < imgMap.Width; x++)
            {
                for (int y = 0; y < imgMap.Height; y++)
                {
                    if (i == textInBin.Length) return;

                    Color pixelColor = imgMap.GetPixel(x, y);
                    
                    int bit = (textInBin[i] >> j--) & 1;
                    Color newColor = Color.FromArgb((pixelColor.R & 0xFE) + bit, pixelColor.G, pixelColor.B);
                    if (j < 0)
                    {
                        i++;
                        j = 7;
                        if (i == textInBin.Length) return;
                    }

                    bit = (textInBin[i] >> j--) & 1;
                    newColor = Color.FromArgb(newColor.R, (pixelColor.G & 0xFE) + bit, pixelColor.B);
                    imgMap.SetPixel(x, y, newColor);
                    if (j < 0)
                    {
                        i++;
                        j = 7;
                        if (i == textInBin.Length) return;
                    }

                    bit = (textInBin[i] >> j--) & 1;
                    newColor = Color.FromArgb(newColor.R, newColor.G, (pixelColor.B & 0xFE) + bit);
                    imgMap.SetPixel(x, y, newColor);
                    if (j < 0)
                    {
                        i++;
                        j = 7;
                        if (i == textInBin.Length) return;
                    }
                }
            }
        }

        private void Seek_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(imgPath))
            {
                return;
            }

            string key = textBox1.Text;
            byte[] tmp = new byte[(int)Math.Ceiling((imgMap.Width * imgMap.Height * 3) / 8.0)];
            
            byte[] textInBin = new byte[ExtractMsg(tmp, imgMap)];
            Array.Copy(tmp, textInBin, textInBin.Length);


            foreach (byte b in textInBin)
            {
                Console.WriteLine(b);
            }
            Console.WriteLine("end");

            string plaintext = "";

            if (!(string.IsNullOrEmpty(key)))
            {
                using (Aes myAes = Aes.Create())
                {
                    // Decrypt the bytes to a string.
                    plaintext = Crypto.DecryptStringFromBytes_Aes(textInBin, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(IV));
                }
            }
            else
            {
                plaintext = Encoding.UTF8.GetString(textInBin);
            }
            
            // now the user selects no image
            Reinitialize(plaintext, "Waiting for input...", "", null);
        }
        
        private int ExtractMsg(byte[] textInBin, Bitmap imgMap)
        {
            byte[] str = { 60, 83, 84, 82, 62 };
            byte[] end = { 60, 69, 78, 68, 62 };
            byte[] tmp = new byte[5];
            bool STR = false, END = false;
            int i = 0, j = 7, k = 0;
            int[] mask = { 1, 2, 4, 8, 16, 32, 64, 128 };
            int sum = 0;
            for (int x = 0; x < imgMap.Width; x++)
            {
                for (int y = 0; y < imgMap.Height; y++)
                {
                    if (i == textInBin.Length) return i;
                    
                    Color pixelColor = imgMap.GetPixel(x, y);
                    
                    sum += (pixelColor.R & 1) * mask[j--];
                    if (j < 0)
                    {
                        j = 7;
                        if (STR && sum == 60) return i;
                        if (STR) textInBin[i++] = (byte)sum;
                        if (sum == 62) STR = true;
                        sum = 0;
                    }

                    sum += (pixelColor.G & 1) * mask[j--];
                    if (j < 0)
                    {
                        j = 7;
                        if (STR && sum == 60) return i;
                        if (STR) textInBin[i++] = (byte)sum;
                        if (sum == 62) STR = true;
                        sum = 0;
                    }

                    sum += (pixelColor.B & 1) * mask[j--];
                    if (j < 0)
                    {
                        j = 7;
                        if (STR && sum == 60) return i;
                        if (STR) textInBin[i++] = (byte)sum;
                        if (sum == 62) STR = true;
                        sum = 0;
                    }
                }
            }
            return i;
        }

        private void Reinitialize(string textBoxInput, string statusInput, string imgPathInput, Bitmap imgMapInput)
        {
            textBox1.Text = textBoxInput;
            status.Text = statusInput;
            imgPath = imgPathInput;
            imgMap = imgMapInput;
        }
    }
}
