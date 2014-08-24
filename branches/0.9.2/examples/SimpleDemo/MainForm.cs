using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Simple
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            //Clsoe application
            Application.Exit();
        }

        private void addressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IpAddressesForm iaf = new IpAddressesForm();
            iaf.MdiParent = this;
            iaf.Show();
        }

        private void interfacesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
