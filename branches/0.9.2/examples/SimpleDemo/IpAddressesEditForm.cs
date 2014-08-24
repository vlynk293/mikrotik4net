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
    public partial class IpAddressesEditForm : Form
    {
        private Boolean bNew = false; // flag of new or modify
        private Tik4Net.Objects.Ip.IpAddress ipAddressPrv = null;
        private Tik4Net.Objects.Ip.IpAddressList ipAddressListPrv = null;
        // this is for add new one
        public IpAddressesEditForm()
        {
            InitializeComponent();
            bNew = true;
            ipAddressPrv = new Tik4Net.Objects.Ip.IpAddress();
            ipAddressListPrv = new Tik4Net.Objects.Ip.IpAddressList();
            BindingData();
        }
        // this is for modify 
        public IpAddressesEditForm(Tik4Net.Objects.Ip.IpAddress ipAddress, Tik4Net.Objects.Ip.IpAddressList ipAddressList)
        {
            InitializeComponent();
            bNew = false;
            ipAddressPrv = ipAddress;
            if (ipAddressPrv == null)
                ipAddressPrv = new Tik4Net.Objects.Ip.IpAddress();
            ipAddressListPrv = ipAddressList;
            if (ipAddressListPrv == null)
                ipAddressListPrv = new Tik4Net.Objects.Ip.IpAddressList();
            BindingData();
        }

        private void BindingData()
        {
            // Load Interface date
            LoadInterfaces();
            // Binding UI to object
            addressMaskedTextBox.DataBindings.Add("Text", ipAddressPrv, "Address");
            networkMaskedTextBox.DataBindings.Add("Text", ipAddressPrv, "Network");
            interfaceComboBox.DataBindings.Add("Text", ipAddressPrv, "Interface");
        }

        /// <summary>
        /// Load interface and binding to ComoboBox control;
        /// </summary>
        private void LoadInterfaces()
        {
            // create interfacelist object 
            Tik4Net.Objects.InterfaceList interfaceList = new Tik4Net.Objects.InterfaceList();
            // load interface
            interfaceList.LoadAll();
            // binding to ComboBox
            interfaceComboBox.DataSource = interfaceList.ToList();
            interfaceComboBox.DisplayMember = "Name";
        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void okTtoolStripButton_Click(object sender, EventArgs e)
        {
            //Commit input to binding object
            addressMaskedTextBox.DataBindings[0].WriteValue();
            networkMaskedTextBox.DataBindings[0].WriteValue();
            interfaceComboBox.DataBindings[0].WriteValue();

            //Check if new ip address object
            if (bNew)
            {
                ipAddressListPrv.Add(ipAddressPrv);
                // Notify IpAddressForm to refresh
                if (NewSaved != null)
                    foreach (EditNewSaved ns in NewSaved.GetInvocationList())
                        ns();
            }
            //save to routeros
            ipAddressListPrv.Save();
            Close();
        }

        // Event interface for New Record Saved notify
        public event EditNewSaved NewSaved;

    }
}
