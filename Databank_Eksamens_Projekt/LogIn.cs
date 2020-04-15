using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Databank_Eksamens_Projekt
{
    public partial class LogIn : Form
    {
        //-----Declare server info-----
        String serverAddress = @"\\212.237.140.40\pi";
        String username;
        public LogIn()
        {
            InitializeComponent();
        }
        
        private void ButtonNewUser_Click(object sender, EventArgs e)
        {
            //-----Try to create new user-----
            try
            {
                //-----Ask user if they want to create new user-----
                DialogResult result = MessageBox.Show("Do you want to create a new account?", "UserCreation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //-----Get new account password and username-----
                    string UserNameInput = Interaction.InputBox("Please Select Your Username:","Username Selection");
                    string PasswordInput = Interaction.InputBox("Please Select Your Password:", "Password Selection");

                    //-----Chek if username and password is at least 5 long else send error-----
                    if (UserNameInput==""&&UserNameInput.Length>5&& PasswordInput == "" && PasswordInput.Length > 5)
                    {
                        MessageBox.Show("No Username or Username too short");
                    }

                    //-----Chek if data is correct-----
                    DialogResult ConResult = MessageBox.Show("Confirmation of information:\n Username: "+UserNameInput+"\n Password: "+PasswordInput+"","Confirmation",MessageBoxButtons.YesNo);

                    //-----Create user-----
                    if (ConResult==DialogResult.Yes)
                    {
                        //-----Declare path and file name and chek if it allready exists else create file-----
                        string path = @"C:\Temp\Users.txt";
                        if (!File.Exists(path))
                        {
                            FileStream NewFile = File.Create(path);
                            NewFile.Close();
                        }
                        else
                        {
                            //-----If file exist hash pasword and save user to file-----
                            var UsersList = new List<string>(File.ReadAllLines(path));
                            TextWriter FileWriter = new StreamWriter(path);
                            foreach (String item in UsersList)
                            {
                                FileWriter.WriteLine(item);
                            }
                            var hash = SecureHasher.Hash(PasswordInput);
                            FileWriter.WriteLine(UserNameInput +", "+ hash );
                            FileWriter.Flush();
                            FileWriter.Close();
                        }
                        MessageBox.Show("User will be created. This may take a while.");
                        //-----Create encrypted file and send to server-----
                        CmdExecute(string.Format(@"""\Program Files\VeraCrypt\VeraCrypt Format.exe"" /silent /create ""{0}\{1}"" /hash sha512 /encryption aes /size 200M /filesystem fat /dynamic /password ""{2}""", serverAddress, UserNameInput, PasswordInput));
                        //-----------------------------
                        MessageBox.Show("User Created");
                    }
                }
            }
            //-----If something goes worng send error report-----
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error");
            }
        }

        private void ButtonLogIn_Click(object sender, EventArgs e)
        {
            //-----select path and create list-----
            string path = @"C:\Temp\Users.txt";
            var UsersList = new List<string>();
            //-----Try to read users from file-----
            try
            {
               UsersList = new List<string>(File.ReadAllLines(path));
            }
            catch (Exception)
            {

                MessageBox.Show("Oops. Something went wrong");
            }
            //-----Set number og users read to 0-----
            int Usrcount =0;

            //-----Go throw all users and see if they match the login info-----
            foreach (String item in UsersList)
            {
                //-----Construct seperator-----
                string password = item;
                string[] seperator = { ", " };
                Int32 count = 2;
                string[] strlist = password.Split(seperator, count, StringSplitOptions.RemoveEmptyEntries);

                //-----Add one to usrcount becuase one user is now being read-----
                Usrcount = Usrcount+1;

                //-----Chek if username is correct else send error-----
                if (strlist[0]==textBoxUsername.Text)
                {
                    //-----Hash the password from login and varify password if worong send error-----
                    var result = SecureHasher.Verify(textBoxPassword.Text, strlist[1]);
                    if (result.Equals(false))
                    {
                    MessageBox.Show("Wrong Username or password");
                    }
                    else
                    {
                        //-----Ask if you want to skip Facedetection-----
                        DialogResult MBResult = MessageBox.Show("Want to skip Facedetection?","TestMode",MessageBoxButtons.YesNo);
                        if (MBResult.Equals(DialogResult.Yes))
                        {
                            //-----open home-----
                            username = textBoxUsername.Text;
                            Mount();
                            Form Home = new Home(username);
                            Home.Show();
                            Form Login = new LogIn();
                            Login.Close();
                        }
                        else
                        {
                            //-----open FaceDetection-----
                            Form Face = new FaceDetection(username);
                            Face.Show();
                            Form Login = new LogIn();
                            Login.Close();
                        }
                    
                    }
                }

                //-----If no more users to go throw send error-----
                else if (UsersList.Count()==Usrcount)
                {
                    MessageBox.Show("Wrong Username or password");
                }
            }
        }

        public void Mount()
        {
            //-----Mount encrypted file-----
            CmdExecute(string.Format(@"""\Program Files\VeraCrypt\VeraCrypt.exe"" /q /v ""{0}\{1}"" /letter z /p ""{2}""", serverAddress, textBoxUsername.Text, textBoxPassword.Text));
            //-----------------------------
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            //-----Close form-----
            this.Close();
        }

        public static class SecureHasher
        {
            //-----Declare size-----
            private const int SaltSize = 16;
            private const int HashSize = 20;


            public static string Hash(string password, int interactions)
            {
                //-----Create salt with RNG from System.Security.Cryptography;-----
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

                //-----Create pbkdf2 and get hash value-----
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, interactions);
                var hash = pbkdf2.GetBytes(HashSize);

                //-----Combine salt and password bytes and convert-----
                var hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
                var base64Hash = Convert.ToBase64String(hashBytes);

                //-----Format hash with info-----
                return string.Format("$MYHASH$V1${0}${1}", interactions, base64Hash);
            }

            //-----Hash the password-----
            public static string Hash(string password)
            {
                return Hash(password, 10000);
            }

            //-----Chek if hash is supported-----
            public static bool IsHashSupported(string hashString)
            {
                return hashString.Contains("HASH$V1$");
            }

            //-----Veryfi incoming password with hashed password-----
            public static bool Verify(string password, string hashedPassword)
            {
                //-----Check if string contains "HASH$V1$"-----
                if (!IsHashSupported(hashedPassword))
                {
                    throw new NotSupportedException("Error: Hashtype not supported");
                }

                //-----Get iteraton and base64 string-----
                var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
                var iterations = int.Parse(splittedHashString[0]);
                var base64Hash = splittedHashString[1];

                //-----get hashed bytes and salt-----
                var hashBytes = Convert.FromBase64String(base64Hash);
                var salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                //-----´Create hash with salt-----
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
                byte[] hash = pbkdf2.GetBytes(HashSize);

                //-----Check if characters match and send result-----
                for (var i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
                
                
            }


        }
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

        private void LogIn_Load(object sender, EventArgs e)
        {
            CmdExecute(@"net use \\212.237.140.40\pi programmeringsfaget /user:pi");
        }
    }
}