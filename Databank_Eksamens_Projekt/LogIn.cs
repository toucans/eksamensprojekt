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
                DialogResult result = MessageBox.Show("Sure you wan't to create a new account?", "UserCreation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string UserNameInput = Interaction.InputBox("Please Scelect Your Username:","UsernameSeclection");
                    string PasswordInput = Interaction.InputBox("Please Scelect Your Password:", "PasswordSeclection");
                    if (UserNameInput==""&&UserNameInput.Length>5&& PasswordInput == "" && PasswordInput.Length > 5)
                    {
                        MessageBox.Show("No Username or Username to short");
                    }

                    DialogResult ConResult = MessageBox.Show("Confirmation of information:\n Username: "+UserNameInput+"\n Password: "+PasswordInput+"","Confirmation",MessageBoxButtons.YesNo);

                    if (ConResult==DialogResult.Yes)
                    {
                        //Create user

                        MessageBox.Show("User Created");
                        string path = @"C:\Temp\UsersTest.txt";
                        if (!File.Exists(path))
                        {
                            FileStream NewFile = File.Create(path);
                            NewFile.Close();
                        }
                        else
                        {
                            string[] UsersImport = File.ReadAllLines(path);
                            List<string> UsersList = new List<string>(/*UsersImport*/);
                            UsersList.Add(UserNameInput+","+PasswordInput);
                            StreamWriter FileWriter = new StreamWriter(path);
                            File.WriteAllText(path, string.Empty);
                            FileWriter.Write(UsersList.ToString());
                            FileWriter.Flush();
                            FileWriter.Close();
                        }
                        MessageBox.Show("Creating user");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(),"Error");
            }
        }
    }
}
