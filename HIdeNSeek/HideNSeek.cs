using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HideNSeek
{
    public partial class HideNSeek : Form
    {
        private Bitmap imgMap;
        private string imgPath;

        public HideNSeek()
        {
            InitializeComponent();
        }

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
            }
        }

        private void HideNSeek_Load(object sender, EventArgs e)
        {

        }
        
        private void Open_Click(object sender, EventArgs e)
        {
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
                    imgPath = dlg.FileName;
                    status.Text = "File Selected: " + imgPath;
                    imgMap = new Bitmap(imgPath);
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
            string plaintext = "<START>" + textBox1.Text + "<END>";
            textBox1.Text = "";
            byte[] textInBin = Encoding.UTF8.GetBytes(plaintext);
            int i = 0, j = 7;

            // ensure that the photo has enough space for the message
            if(textInBin.Length > imgMap.Width * imgMap.Height * 3)
            {
                status.Text = "Not Enough Space";
                return;
            }

            // insert the hidden message
            for (int x = 0; x < imgMap.Width; x++)
            {
                for (int y = 0; y < imgMap.Height; y++)
                {
                    Color pixelColor = imgMap.GetPixel(x, y);

                    int bit = (textInBin[i] >> j--) & 1;
                    Color newColor = Color.FromArgb(pixelColor.R & 0xFE + bit, pixelColor.G, pixelColor.B);
                    imgMap.SetPixel(x, y, newColor);
                    if(j < 0)
                    {
                        i++;
                        j = 7;
                    }

                    bit = (textInBin[i] >> j--) & 1;
                    newColor = Color.FromArgb(pixelColor.R, pixelColor.G & 0xFE + bit, pixelColor.B);
                    imgMap.SetPixel(x, y, newColor);
                    if (j < 0)
                    {
                        i++;
                        j = 7;
                    }

                    bit = (textInBin[i] >> j--) & 1;
                    newColor = Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B & 0xFE + bit);
                    imgMap.SetPixel(x, y, newColor);
                    if (j < 0)
                    {
                        i++;
                        j = 7;
                    }
                }
            }
            
            // save the picture with hidden message
            string newPath = Directory.GetParent(imgPath).ToString() + '/' + Path.GetFileNameWithoutExtension(imgPath) + "_copy.png";
            imgMap.Save(newPath, ImageFormat.Png);
            status.Text = "File Saved: " + newPath;

            // now the user selects no image
            imgPath = "";

            /*
            ImageConverter converter = new ImageConverter();
            byte[] imgInBin = (byte[])converter.ConvertTo(imgMap, typeof(byte[]));
            */
        }

        private void InsertMsg(byte[] textInBin, Bitmap imgMap)
        {

        }

        private void Seek_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(imgPath))
            {
                return;
            }

            ImageConverter converter = new ImageConverter();
            byte[] imgInBin = (byte[])converter.ConvertTo(imgMap, typeof(byte[]));

            foreach(byte b in imgInBin)
            {
                Console.WriteLine(b);
            }

            string plaintext = Encoding.UTF8.GetString(imgInBin);
            textBox1.Text = plaintext;
        }

        static string ToBinaryString(Encoding encoding, string text)
        {
            return string.Join("", encoding.GetBytes(text).Select(n => Convert.ToString(n, 2).PadLeft(8, '0')));
        }
    }
}
