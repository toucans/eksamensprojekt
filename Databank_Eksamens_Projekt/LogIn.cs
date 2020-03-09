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
                        MessageBox.Show("No Username or Username to short");
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
                            FileWriter.WriteLine(UserNameInput +", "+ PasswordInput);
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
            Form login = new Home();
            login.Show();

        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}