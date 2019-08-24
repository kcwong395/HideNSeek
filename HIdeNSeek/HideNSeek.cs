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
            // pass the selected file to check whether it is an image
            isImg(imgs[0]);
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
                isImg(dlg.FileName);
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

            // prepossing the input and insert the message
            TextProcessing.InsertMsg(textBox1.Text, imgMap);

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

            // process the input and extract the message
            string plaintext = TextProcessing.ExtractMsg(textBox1.Text, imgMap);

            // now the user selects no image
            Reinitialize(plaintext, "Waiting for input...", "", null);
        }

        private void isImg(string path)
        {
            string extension = Path.GetExtension(path).ToLower();
            if (extension != ".png" && extension != ".jpg")
            {
                status.Text = "Error: File selected is not an image";
            }
            else
            {
                Reinitialize("", "File Selected: " + path, path, new Bitmap(path));
            }
        }

        // this functio controls the output, content of bitmap and the image path
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
