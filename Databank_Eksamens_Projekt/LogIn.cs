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
                    DialogResult ConResult = MessageBox.Show("Confirmation of information:\n Username: "+UserNameInput+"\n Password: "+PasswordInput+"","Confirmation",MessageBoxButtons.YesNo);
                    if (ConResult==DialogResult.Yes)
                    {
                        //Create user
                        MessageBox.Show("Creaationg user");
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
                else
                {
                    //do nothing
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Ups, Something went wrong","Error");
            }


        }
    }
}
