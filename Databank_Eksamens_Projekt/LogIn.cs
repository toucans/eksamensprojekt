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
        public LogIn()
        {
            InitializeComponent();
        }

        private void ButtonNewUser_Click(object sender, EventArgs e)
        {
            try
            {
                //DialogResult drl = new DialogResult();
                DialogResult result = MessageBox.Show("Do you want to create a new account?", "UserCreation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string UserNameInput = Interaction.InputBox("Please Select Your Username:","Username Selection");
                    string PasswordInput = Interaction.InputBox("Please Select Your Password:", "Password Selection");
                    if (UserNameInput==""&&UserNameInput.Length>5&& PasswordInput == "" && PasswordInput.Length > 5)
                    {
                        MessageBox.Show("No Username or Username too short");
                    }

                    DialogResult ConResult = MessageBox.Show("Confirmation of information:\n Username: "+UserNameInput+"\n Password: "+PasswordInput+"","Confirmation",MessageBoxButtons.YesNo);

                    if (ConResult==DialogResult.Yes)
                    {
                        //Create user
                        string path = @"C:\Temp\Users.txt";
                        if (!File.Exists(path))
                        {
                            FileStream NewFile = File.Create(path);
                            NewFile.Close();
                        }
                        else
                        {
                           // string usersRead = File.ReadAllLines(path).ToString();
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
                        MessageBox.Show("User Created");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error");
            }
        }

        private void ButtonLogIn_Click(object sender, EventArgs e)
        {
            string path = @"C:\Temp\Users.txt";
            var UsersList = new List<string>();
            try
            {
               UsersList = new List<string>(File.ReadAllLines(path));
            }
            catch (Exception)
            {

                MessageBox.Show("Wrong Username or password");
            }
            foreach (String item in UsersList)
            {

                string password = item;
                string[] seperator = { ", " };
                Int32 count = 2;

                string[] strlist = password.Split(seperator, count, StringSplitOptions.RemoveEmptyEntries);
                MessageBox.Show(strlist[1]);

                if (strlist[0]==textBoxUsername.Text)
                {
                    var result = SecureHasher.Verify(textBoxPassword.Text, strlist[1]);
                    if (result.Equals(false))
                    {
                    MessageBox.Show("Wrong Username or password");
                    }
                    else
                    {
                    MessageBox.Show("loged in");
                    }
                }
               // else if (UsersList.Count()==)
               // {

               // }
              
            }
            
            /*
            Form login = new Home();
            login.Show();
            */
            
            //-----Mount encrypted file-----
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(@"""\Program Files\VeraCrypt\VeraCrypt.exe"" /q /v ""C:\Users\Johan\Documents\yoo"" /p ""programmeringsfaget""");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            //-----------------------------

            Form login = new Home();
            login.Show();


        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static class SecureHasher
        {
            private const int SaltSize = 16;
            private const int HashSize = 20;


            public static string Hash(string password, int interactions)
            {
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, interactions);
                var hash = pbkdf2.GetBytes(HashSize);

                var hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                var base64Hash = Convert.ToBase64String(hashBytes);

                return string.Format("$MYHASH$V1${0}${1}", interactions, base64Hash);
            }

            public static string Hash(string password)
            {
                return Hash(password, 10000);
            }

            public static bool IsHashSupported(string hashString)
            {
                return hashString.Contains("HASH$V1$");
            }
            public static bool Verify(string password, string hashedPassword)
            {
                if (!IsHashSupported(hashedPassword))
                {
                    throw new NotSupportedException("Error: Hashtype not supported");
                }
                
                
                var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
                var iterations = int.Parse(splittedHashString[0]);
                var base64Hash = splittedHashString[1];
                var hashBytes = Convert.FromBase64String(base64Hash);
                var salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
                byte[] hash = pbkdf2.GetBytes(HashSize);

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
    }
}