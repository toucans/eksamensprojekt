using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;

namespace Databank_Eksamens_Projekt
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }
        String serverAddress = "file://192.168.0.47";
        String serverAddress2 = @"\\192.168.0.47\pi";

        private void Home_Load(object sender, EventArgs e)
        {

            webBrowser1.Url = new Uri(serverAddress);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoBack)
            {
                webBrowser1.GoBack();
            }
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            if (webBrowser1.CanGoForward)
            {
                webBrowser1.GoForward();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog zip = new SaveFileDialog();
            zip.Filter = "Zip Files (*.zip)|*.zip";
            if (zip.ShowDialog() == DialogResult.OK)
            {
                ZipFile.CreateFromDirectory(serverAddress2, zip.FileName);
            }


        }
    }
}
