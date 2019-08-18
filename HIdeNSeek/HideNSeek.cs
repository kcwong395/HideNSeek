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
            if(imgMap == null)
            {
                return;
            }

            string plaintext = "<START>" + textBox1.Text + "<END>";
            textBox1.Text = "";
            byte[] textInBin = Encoding.UTF8.GetBytes(plaintext);

            for (int i = 0; i < imgMap.Width; i++)
            {
                for (int j = 0; j < imgMap.Height; j++)
                {
                    Color pixelColor = imgMap.GetPixel(i, j);
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    imgMap.SetPixel(i, j, newColor);
                }
            }
            string newPath = Directory.GetParent(imgPath).ToString() + '/' + Path.GetFileNameWithoutExtension(imgPath) + "_copy.png";
            imgMap.Save(newPath, ImageFormat.Png);
            Console.WriteLine(newPath);
            status.Text = "File Saved: " + newPath;
            /*
            ImageConverter converter = new ImageConverter();
            byte[] imgInBin = (byte[])converter.ConvertTo(imgMap, typeof(byte[]));
            */
        }

        private void Seek_Click(object sender, EventArgs e)
        {
            if (imgMap == null)
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
