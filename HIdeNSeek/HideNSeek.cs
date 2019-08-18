using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HideNSeek
{
    public partial class HideNSeek : Form
    {
        public HideNSeek()
        {
            InitializeComponent();
        }

        public HideNSeek(string[] imgs)
        {
            InitializeComponent();
        }

        private void HideNSeek_Load(object sender, EventArgs e)
        {

        }

        [STAThread]
        private void Open_Click(object sender, EventArgs e)
        {
            try
            {
                Thread t = new Thread(() => {
                    OpenFileDialog dlg = new OpenFileDialog
                    {
                        Title = "Select an image",
                        InitialDirectory = "C:/"
                    };

                    // The following would not return the dialog if the current
                    // thread is not STA
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        status.Text = "File Selected: " + dlg.FileName;
                    }

                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            catch (Exception) { }
        }

        private void Hide_Click(object sender, EventArgs e)
        {

        }

        private void Seek_Click(object sender, EventArgs e)
        {

        }
    }
}
