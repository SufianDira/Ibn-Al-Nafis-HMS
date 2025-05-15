using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentRegSys
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (IdTexbox.Text == "test" && PasswordTextBox.Text == "test")
            {
                var frm = new Form1();
                frm.Location = this.Location;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Show();
                this.Hide();
            }
            
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
