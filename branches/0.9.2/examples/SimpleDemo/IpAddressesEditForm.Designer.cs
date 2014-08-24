namespace Simple
{
    partial class IpAddressesEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IpAddressesEditForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.okTtoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.cancelToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.addressMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.networkMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.interfaceComboBox = new System.Windows.Forms.ComboBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.okTtoolStripButton,
            this.cancelToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(377, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // okTtoolStripButton
            // 
            this.okTtoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("okTtoolStripButton.Image")));
            this.okTtoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.okTtoolStripButton.Name = "okTtoolStripButton";
            this.okTtoolStripButton.Size = new System.Drawing.Size(45, 22);
            this.okTtoolStripButton.Text = "Ok";
            this.okTtoolStripButton.Click += new System.EventHandler(this.okTtoolStripButton_Click);
            // 
            // cancelToolStripButton
            // 
            this.cancelToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelToolStripButton.Image")));
            this.cancelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cancelToolStripButton.Name = "cancelToolStripButton";
            this.cancelToolStripButton.Size = new System.Drawing.Size(66, 22);
            this.cancelToolStripButton.Text = "Cancel";
            this.cancelToolStripButton.Click += new System.EventHandler(this.cancelToolStripButton_Click);
            // 
            // addressMaskedTextBox
            // 
            this.addressMaskedTextBox.Location = new System.Drawing.Point(110, 61);
            this.addressMaskedTextBox.Name = "addressMaskedTextBox";
            this.addressMaskedTextBox.Size = new System.Drawing.Size(234, 21);
            this.addressMaskedTextBox.TabIndex = 2;
            // 
            // networkMaskedTextBox
            // 
            this.networkMaskedTextBox.Location = new System.Drawing.Point(110, 109);
            this.networkMaskedTextBox.Name = "networkMaskedTextBox";
            this.networkMaskedTextBox.Size = new System.Drawing.Size(234, 21);
            this.networkMaskedTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Network:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Interface:";
            // 
            // interfaceComboBox
            // 
            this.interfaceComboBox.FormattingEnabled = true;
            this.interfaceComboBox.Location = new System.Drawing.Point(110, 163);
            this.interfaceComboBox.Name = "interfaceComboBox";
            this.interfaceComboBox.Size = new System.Drawing.Size(234, 20);
            this.interfaceComboBox.TabIndex = 8;
            // 
            // IpAddressesEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 226);
            this.Controls.Add(this.interfaceComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.networkMaskedTextBox);
            this.Controls.Add(this.addressMaskedTextBox);
            this.Controls.Add(this.toolStrip1);
            this.Name = "IpAddressesEditForm";
            this.Text = "IpAddressesEditForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton okTtoolStripButton;
        private System.Windows.Forms.ToolStripButton cancelToolStripButton;
        private System.Windows.Forms.MaskedTextBox addressMaskedTextBox;
        private System.Windows.Forms.MaskedTextBox networkMaskedTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox interfaceComboBox;

    }
}