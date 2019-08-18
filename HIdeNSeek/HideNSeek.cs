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
                imgPath = imgs[0];
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
            string plaintext = "<STR " + textBox1.Text + " END>";
            textBox1.Text = "";
            byte[] textInBin = Encoding.UTF8.GetBytes(plaintext);
            
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
            imgPath = "";
            imgMap = null;
            status.Text = "Waiting for input...";
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
                    Console.WriteLine((pixelColor.R & 0xFE) + bit);
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

            byte[] textInBin = new byte[(int)Math.Ceiling((imgMap.Width * imgMap.Height * 3) / 8.0)];

            ExtractMsg(textInBin, imgMap);
            
            string plaintext = Encoding.UTF8.GetString(textInBin);

            textBox1.Text = plaintext;

            // now the user selects no image
            imgPath = "";
            imgMap = null;
            status.Text = "Waiting for input...";
        }
        
        private void ExtractMsg(byte[] textInBin, Bitmap imgMap)
        {
            int i = 0, j = 7;
            int[] mask = { 1, 2, 4, 8, 16, 32, 64, 128 };
            int sum = 0;
            for (int x = 0; x < imgMap.Width; x++)
            {
                for (int y = 0; y < imgMap.Height; y++)
                {
                    if (i == textInBin.Length) return;
                    
                    Color pixelColor = imgMap.GetPixel(x, y);
                    
                    sum += (pixelColor.R & 1) * mask[j--];
                    Console.WriteLine(sum);
                    if (j < 0)
                    {
                        j = 7;
                        textInBin[i++] = (byte)sum;
                        if (sum == 62) return;
                        sum = 0;
                    }
                    sum += (pixelColor.G & 1) * mask[j--];
                    Console.WriteLine(sum);
                    if (j < 0)
                    {
                        j = 7;
                        textInBin[i++] = (byte)sum;
                        if (sum == 62) return;
                        sum = 0;
                    }
                    sum += (pixelColor.B & 1) * mask[j--];
                    Console.WriteLine(sum);
                    if (j < 0)
                    {
                        j = 7;
                        textInBin[i++] = (byte)sum;
                        if (sum == 62) return;
                        sum = 0;
                    }
                }
            }
            return;
        }

        private void readPixel(Color pixelColor)
        {
            Console.WriteLine(pixelColor.R);
            Console.WriteLine(pixelColor.G);
            Console.WriteLine(pixelColor.B);
        }
    }
}
