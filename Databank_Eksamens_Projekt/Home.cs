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
        String serverAddress = @"\\212.237.140.40\pi";
        String mountDrive = "X:/";

        private void Home_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri(mountDrive);

            copyWorker.WorkerSupportsCancellation = true;
            copyWorker.WorkerReportsProgress = true;
            copyWorker.ProgressChanged += Worker_ProgressChanged;
            copyWorker.DoWork += Worker_DoWork;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CopyFile(copySource, copyDest);
            if (zipBool == true)
            {
                MessageBox.Show("Zipping file. This may take a while.");
                ZipFile.CreateFromDirectory(serverAddress + "\\temp", zipFileName);
                MessageBox.Show("Zip download done.");
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarCopy.Value = e.ProgressPercentage;
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
                        zipBool = true;
                        Directory.CreateDirectory(serverAddress + "\\temp");
                        copySource = serverAddress + "\\" + username1;
                        copyDest = serverAddress + "\\temp\\" + username1;
                        //File.Copy(serverAddress + "\\" + username1, serverAddress + "\\temp\\" + username1);
                        zipFileName = zip.FileName;
                        MessageBox.Show("Copying file before zipping. This may take a while.");
                        copyWorker.RunWorkerAsync();

                        
                        // File.Delete(serverAddress + "\\temp");
                    }
                }
                else
                {
                    SaveFileDialog file = new SaveFileDialog();
                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        zipBool = false;
                        copySource = serverAddress + "\\" + username1;
                        copyDest = file.FileName;
                        copyWorker.RunWorkerAsync();
                        
                    }
                }

            }


        }
        
        //-------Copy file with progress bar------

        BackgroundWorker copyWorker = new BackgroundWorker();
        String copySource;
        String copyDest;
        void CopyFile(string source, string des)
        {
            FileStream fsOut = new FileStream(des, FileMode.Create);
            FileStream fsIn = new FileStream(source, FileMode.Open);
            byte[] bt = new byte[1048756];
            int readByte;

            while((readByte = fsIn.Read(bt, 0, bt.Length)) > 0)
            {
                fsOut.Write(bt, 0, readByte);
                copyWorker.ReportProgress((int)(fsIn.Position * 100 / fsIn.Length));
            }
            fsIn.Close();
            fsOut.Close();
        }
        bool zipBool = false;
        String zipFileName;
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
        private void progressBarCopy_Click(object sender, EventArgs e)
        {
        }
    }
}
