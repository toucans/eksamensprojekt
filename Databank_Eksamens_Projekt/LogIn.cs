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
                            List<string> UsersList = new List<string>(File.ReadAllLines(path).ToList());
                            StreamWriter FileWriter = new StreamWriter(path);
                            FileWriter.Write("");
                           /* string UserAdd = UsersList.Add;
                            FileWriter.Write(UsersList.Add(UserAdd));*/
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