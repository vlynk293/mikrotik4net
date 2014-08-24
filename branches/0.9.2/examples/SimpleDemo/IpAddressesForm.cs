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
    public partial class IpAddressesForm : Form
    {
        private Tik4Net.Objects.Ip.IpAddressList IpAddresses;
        public IpAddressesForm()
        {
            InitializeComponent();
            // Create instance of IpAddressList
            IpAddresses = new Tik4Net.Objects.Ip.IpAddressList();
        }

        private void IpAddressesForm_Load(object sender, EventArgs e)
        {
            //Fill the grid when form load
            RefreshData();
        }

        private void RefreshData()
        {
            //load data from routeros device
            IpAddresses.LoadAll();
            //define an List,add ipAddress objects,use to binding to datagridview,
            //from now we can't binding datagridview to Tik4Net's Generic
            List<Tik4Net.Objects.Ip.IpAddress> ipAddressList = IpAddresses.ToList();
            //binding to datagridview 
            dataGridView1.DataSource = ipAddressList;
        }

        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void disableToolStripButton_Click(object sender, EventArgs e)
        {
            //locate the selected row
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // get the selected row
                DataGridViewRow myRow = dataGridView1.SelectedRows[0];
                // get row id
                string sId = myRow.Cells["id"].Value.ToString();
                //find the row from Tik4Net object list
                foreach (Tik4Net.Objects.Ip.IpAddress ipAddress in IpAddresses)
                {
                    //find the address object
                    if (ipAddress.Id.Equals(sId))
                    {
                        ipAddress.Disabled = true;
                        //write to routeros devices
                        IpAddresses.Save();
                        //refresh all data
                        RefreshData();
                        break;
                    }
                }
            }
        }

        private void enableToolStripButton_Click(object sender, EventArgs e)
        {
            //locate the selected row
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // get the selected row
                DataGridViewRow myRow = dataGridView1.SelectedRows[0];
                // get row id
                string sId = myRow.Cells["id"].Value.ToString();
                //find the row from Tik4Net object list
                foreach (Tik4Net.Objects.Ip.IpAddress ipAddress in IpAddresses)
                {
                    //find the address object
                    if (ipAddress.Id.Equals(sId))
                    {
                        ipAddress.Disabled = false;
                        //write to routeros devices
                        IpAddresses.Save();
                        //refresh all data
                        RefreshData();
                        break;
                    }
                }
            }
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // get the selected row
                DataGridViewRow myRow = dataGridView1.SelectedRows[0];
                // get row id
                string sId = myRow.Cells["id"].Value.ToString();
                //find the row from Tik4Net object list
                foreach (Tik4Net.Objects.Ip.IpAddress ipAddress in IpAddresses)
                {
                    //find the address object
                    if (ipAddress.Id.Equals(sId))
                    {
                        // Confirm if delete
                        if (MessageBox.Show("Remove the data?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            ipAddress.MarkDeleted();
                            //write to routeros devices
                            IpAddresses.Save();
                            //refresh all data
                            RefreshData();
                            break;
                        }
                    }
                }
            }
        }

        private void AddToolStripButton_Click(object sender, EventArgs e)
        {
            IpAddressesEditForm iaef = new IpAddressesEditForm();
            // After Add New record , this form need refresh
            iaef.NewSaved += this.EditNewSaved;
            iaef.MdiParent = this.MdiParent;
            iaef.Show();
        }

        private void modifyToolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // get the selected row
                DataGridViewRow myRow = dataGridView1.SelectedRows[0];
                // get row id
                string sId = myRow.Cells["id"].Value.ToString();
                //find the row from Tik4Net object list
                foreach (Tik4Net.Objects.Ip.IpAddress ipAddress in IpAddresses)
                {
                    //find the address object
                    if (ipAddress.Id.Equals(sId))
                    {
                        IpAddressesEditForm iaef = new IpAddressesEditForm(ipAddress, IpAddresses);
                        iaef.MdiParent = this.MdiParent;
                        iaef.Show();
                    }
                }
            }

        }


        //Event handle for refresh
        public void EditNewSaved()
        {
            RefreshData();
        }
    }
}
