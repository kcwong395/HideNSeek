using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

            // only the first image will be processed
            string extension = Path.GetExtension(imgs[0]).ToLower();
            if (extension != ".png" && extension != ".jpg")
            {
                status.Text = "File Selected is not an image";
            }
            else
            {
                Reinitialize("", "File Selected: " + imgs[0], imgs[0], new Bitmap(imgs[0]));
            }
        }
        
        private void Open_Click(object sender, EventArgs e)
        {
            // open the file browser
            OpenFileDialog dlg = new OpenFileDialog
            {
                Title = "Select an image",
                InitialDirectory = "C:/"
            };
            
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(dlg.FileName).ToLower();
                if (extension != ".png" && extension != ".jpg")
                {
                    status.Text = "Error: File selected is not an image";
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
                status.Text = "Error: No file is selected";
                return;
            }

            // prepossing the input
            byte[] textInByte = TextProcessing.HideInput(textBox1.Text);

            // insert the hidden message
            ImgProcessing.InsertMsg(textInByte, imgMap);

            // save the picture with hidden message
            string newPath = Directory.GetParent(imgPath).ToString() + '/' + Path.GetFileNameWithoutExtension(imgPath) + "_copy.png";
            imgMap.Save(newPath, ImageFormat.Png);

            // reinitialize the output text and deselect the image
            Reinitialize("", "Waiting for input...", "", null);
        }

        private void Seek_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(imgPath))
            {
                return;
            }
            
            byte[] textInByte = ImgProcessing.ExtractByte(imgMap);
            
            string plaintext = TextProcessing.SeekOutput(textBox1.Text, textInByte);

            // now the user selects no image
            Reinitialize(plaintext, "Waiting for input...", "", null);
        }

        private void Reinitialize(string textBoxInput, string statusInput, string imgPathInput, Bitmap imgMapInput)
        {
            textBox1.Text = textBoxInput;
            status.Text = statusInput;
            imgPath = imgPathInput;
            imgMap = imgMapInput;
        }

        private void HideNSeek_Load(object sender, EventArgs e)
        {

        }

    }
}
