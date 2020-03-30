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
using System.Diagnostics;
using System.Threading;
using System.Net;

namespace Databank_Eksamens_Projekt
{
    public partial class Home : Form
    {
        String username1;
        public Home(string username)
        {
            InitializeComponent();
            username1 = username;
        }
        String serverAddress = @"\\192.168.0.47\pi";
        String mountDrive = "Z:/";

        private void Home_Load(object sender, EventArgs e)
        {

            webBrowser1.Url = new Uri(mountDrive);
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

        private void buttonDownloadZip_Click(object sender, EventArgs e)
        {
            SaveFileDialog zip = new SaveFileDialog();
            zip.Filter = "Zip Files (*.zip)|*.zip";
            if (zip.ShowDialog() == DialogResult.OK)
            {
                ZipFile.CreateFromDirectory(mountDrive, zip.FileName);
            }
        }
        private void buttonDownloadEncryptedFile_Click(object sender, EventArgs e)
        {
            DialogResult dismountYesNo = MessageBox.Show("Downloading your encrypted file will require it to dismount first. Do you want to continue?", "Dismount", MessageBoxButtons.YesNo);
            if (dismountYesNo.Equals(DialogResult.Yes))
            {
                DialogResult zippedYesNo = MessageBox.Show("Do you want your file zipped?", "Zipped", MessageBoxButtons.YesNo);
                //-----Dismount encrypted file-----
                CmdExecute(@"""\Program Files\VeraCrypt\VeraCrypt.exe"" /q /dismount /force");
                //-----------------------------
                
                if (zippedYesNo.Equals(DialogResult.Yes))
                {
                    SaveFileDialog zip = new SaveFileDialog();
                    zip.Filter = "Zip Files (*.zip)|*.zip";
                    if (zip.ShowDialog() == DialogResult.OK)
                    {

                        Directory.CreateDirectory(serverAddress + "\\temp");
                        File.Copy(serverAddress + "\\" + username1, serverAddress + "\\temp\\" + username1);
                        ZipFile.CreateFromDirectory(serverAddress + "\\temp", zip.FileName);
                        Thread.Sleep(500);
                        // File.Delete(serverAddress + "\\temp");
                    }
                }
                else
                {
                    SaveFileDialog file = new SaveFileDialog();
                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        File.Copy(serverAddress + "\\" + username1, file.FileName);
                    }
                }

            }


        }

        //-------Copy file with progress bar------
        public delegate void IntDelegate(int Int);
        public static event IntDelegate FileCopyProgress;
        public static void CopyFileWithProgress(string source, string destination)
        {
            var webClient = new WebClient();
            webClient.DownloadProgressChanged += DownloadProgress;
            webClient.DownloadFileAsync(new Uri(source), destination);
        }
        private static void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            if (FileCopyProgress != null)
                FileCopyProgress(e.ProgressPercentage);
        }
        //-----------------------------------------
        public void CmdExecute(String command)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }
    }
}
