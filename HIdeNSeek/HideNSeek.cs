using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private Bitmap img;

        public HideNSeek()
        {
            InitializeComponent();
        }

        public HideNSeek(string[] imgs)
        {
            InitializeComponent();
            img = new Bitmap(imgs[0]);
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
                    status.Text = "File Selected: " + dlg.FileName;
                    img = new Bitmap(dlg.FileName);
                }
            }
        }

        private void Hide_Click(object sender, EventArgs e)
        {
            string plaintext = textBox1.Text;
            textBox1.Text = "";
        }

        private void Seek_Click(object sender, EventArgs e)
        {

        }
    }
}
