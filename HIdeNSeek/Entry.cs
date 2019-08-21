using System;
using System.Windows.Forms;

namespace HideNSeek
{
    class Entry
    {
        [STAThread]
        static void Main(string[] imgs)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // If args length is not 0, a file is opened with this application
            if (imgs.Length > 0)
            {
                Application.Run(new HideNSeek(imgs));
            }
            else
            {
                Application.Run(new HideNSeek());
            }
        }
    }
}
