using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tik4Net;

namespace Simple
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            //get instanct of TikSession
            TikSession ts = PubConst.tikSession;
            try
            {
                //Connect to router
                ts.Open(ConnectToTextBox.Text, loginTextBox.Text, passwordTextBox.Text);
            }
            catch (Exception ex)
            {
                // working on exception
                MessageBox.Show("Login failed!"+ex.Message);
                return;
            }
            // hide self & show mainform
            MainForm mf = new MainForm();
            this.Hide();
            mf.Show();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
