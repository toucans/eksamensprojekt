using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            DialogResult drl = new DialogResult();
            drl = MessageBox.Show("Sure you want to create a new user?",MessageBoxButtons.YesNo.ToString());

            if (drl.ToString()=="Yes")
            {
                 
            }
            else
            {
                //do nothing
            }



            
        }
    }
}
