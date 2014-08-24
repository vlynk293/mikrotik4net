namespace Tik4Net.ApiGenerator
{
    partial class Form1
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tcpCredentials = new System.Windows.Forms.TabPage();
            this.btnActivateConsole = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.tcpConsole = new System.Windows.Forms.TabPage();
            this.btnConsoleSendToParser = new System.Windows.Forms.Button();
            this.btnConsoleClear = new System.Windows.Forms.Button();
            this.btnConsoleClose = new System.Windows.Forms.Button();
            this.btnConsoleOpen = new System.Windows.Forms.Button();
            this.btnConsoleExecute = new System.Windows.Forms.Button();
            this.tbConsoleOutput = new System.Windows.Forms.TextBox();
            this.tbConsoleInput = new System.Windows.Forms.TextBox();
            this.tcpParser = new System.Windows.Forms.TabPage();
            this.btnParserGenerate = new System.Windows.Forms.Button();
            this.tbParserFile = new System.Windows.Forms.TextBox();
            this.btnParserSave = new System.Windows.Forms.Button();
            this.tbParserOutput = new System.Windows.Forms.TextBox();
            this.tcpSourceCode = new System.Windows.Forms.TabPage();
            this.eSourceCodeDesigner = new System.Windows.Forms.TextBox();
            this.eSourceCodeCustom = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            this.tcMain.SuspendLayout();
            this.tcpCredentials.SuspendLayout();
            this.tcpConsole.SuspendLayout();
            this.tcpParser.SuspendLayout();
            this.tcpSourceCode.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 9);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(32, 13);
            label2.TabIndex = 0;
            label2.Text = "Host:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 35);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(29, 13);
            label3.TabIndex = 1;
            label3.Text = "Port:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(6, 61);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(32, 13);
            label4.TabIndex = 2;
            label4.Text = "User:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(6, 87);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(33, 13);
            label5.TabIndex = 3;
            label5.Text = "Pass:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(8, 9);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(56, 13);
            label6.TabIndex = 10;
            label6.Text = "Save path";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tcpCredentials);
            this.tcMain.Controls.Add(this.tcpConsole);
            this.tcMain.Controls.Add(this.tcpParser);
            this.tcMain.Controls.Add(this.tcpSourceCode);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(807, 471);
            this.tcMain.TabIndex = 4;
            // 
            // tcpCredentials
            // 
            this.tcpCredentials.Controls.Add(this.btnActivateConsole);
            this.tcpCredentials.Controls.Add(this.tbPassword);
            this.tcpCredentials.Controls.Add(this.tbUser);
            this.tcpCredentials.Controls.Add(this.tbPort);
            this.tcpCredentials.Controls.Add(this.tbHost);
            this.tcpCredentials.Controls.Add(label5);
            this.tcpCredentials.Controls.Add(label4);
            this.tcpCredentials.Controls.Add(label3);
            this.tcpCredentials.Controls.Add(label2);
            this.tcpCredentials.Location = new System.Drawing.Point(4, 22);
            this.tcpCredentials.Name = "tcpCredentials";
            this.tcpCredentials.Padding = new System.Windows.Forms.Padding(3);
            this.tcpCredentials.Size = new System.Drawing.Size(799, 445);
            this.tcpCredentials.TabIndex = 2;
            this.tcpCredentials.Text = "Credentials";
            this.tcpCredentials.UseVisualStyleBackColor = true;
            // 
            // btnActivateConsole
            // 
            this.btnActivateConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActivateConsole.Location = new System.Drawing.Point(9, 110);
            this.btnActivateConsole.Name = "btnActivateConsole";
            this.btnActivateConsole.Size = new System.Drawing.Size(309, 23);
            this.btnActivateConsole.TabIndex = 12;
            this.btnActivateConsole.Text = "Continue >";
            this.btnActivateConsole.UseVisualStyleBackColor = true;
            this.btnActivateConsole.Click += new System.EventHandler(this.btnActivateConsole_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(109, 84);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(209, 20);
            this.tbPassword.TabIndex = 7;
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(109, 58);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(209, 20);
            this.tbUser.TabIndex = 6;
            this.tbUser.Text = "admin";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(109, 32);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(209, 20);
            this.tbPort.TabIndex = 5;
            this.tbPort.Text = "8728";
            // 
            // tbHost
            // 
            this.tbHost.Location = new System.Drawing.Point(109, 6);
            this.tbHost.Name = "tbHost";
            this.tbHost.Size = new System.Drawing.Size(209, 20);
            this.tbHost.TabIndex = 4;
            this.tbHost.Text = "10.43.100.169";
            // 
            // tcpConsole
            // 
            this.tcpConsole.Controls.Add(this.btnConsoleSendToParser);
            this.tcpConsole.Controls.Add(this.btnConsoleClear);
            this.tcpConsole.Controls.Add(this.btnConsoleClose);
            this.tcpConsole.Controls.Add(this.btnConsoleOpen);
            this.tcpConsole.Controls.Add(this.btnConsoleExecute);
            this.tcpConsole.Controls.Add(this.tbConsoleOutput);
            this.tcpConsole.Controls.Add(this.tbConsoleInput);
            this.tcpConsole.Location = new System.Drawing.Point(4, 22);
            this.tcpConsole.Name = "tcpConsole";
            this.tcpConsole.Padding = new System.Windows.Forms.Padding(3);
            this.tcpConsole.Size = new System.Drawing.Size(799, 445);
            this.tcpConsole.TabIndex = 1;
            this.tcpConsole.Text = "Console";
            this.tcpConsole.UseVisualStyleBackColor = true;
            // 
            // btnConsoleSendToParser
            // 
            this.btnConsoleSendToParser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConsoleSendToParser.Location = new System.Drawing.Point(716, 170);
            this.btnConsoleSendToParser.Name = "btnConsoleSendToParser";
            this.btnConsoleSendToParser.Size = new System.Drawing.Size(75, 23);
            this.btnConsoleSendToParser.TabIndex = 6;
            this.btnConsoleSendToParser.Text = "Parser >>";
            this.btnConsoleSendToParser.UseVisualStyleBackColor = true;
            this.btnConsoleSendToParser.Click += new System.EventHandler(this.btnConsoleSendToParser_Click);
            // 
            // btnConsoleClear
            // 
            this.btnConsoleClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConsoleClear.Location = new System.Drawing.Point(635, 170);
            this.btnConsoleClear.Name = "btnConsoleClear";
            this.btnConsoleClear.Size = new System.Drawing.Size(75, 23);
            this.btnConsoleClear.TabIndex = 5;
            this.btnConsoleClear.Text = "Clear";
            this.btnConsoleClear.UseVisualStyleBackColor = true;
            this.btnConsoleClear.Click += new System.EventHandler(this.btnConsoleClear_Click);
            // 
            // btnConsoleClose
            // 
            this.btnConsoleClose.Enabled = false;
            this.btnConsoleClose.Location = new System.Drawing.Point(89, 170);
            this.btnConsoleClose.Name = "btnConsoleClose";
            this.btnConsoleClose.Size = new System.Drawing.Size(75, 23);
            this.btnConsoleClose.TabIndex = 4;
            this.btnConsoleClose.Text = "Logout";
            this.btnConsoleClose.UseVisualStyleBackColor = true;
            this.btnConsoleClose.Click += new System.EventHandler(this.btnConsoleClose_Click);
            // 
            // btnConsoleOpen
            // 
            this.btnConsoleOpen.Location = new System.Drawing.Point(8, 170);
            this.btnConsoleOpen.Name = "btnConsoleOpen";
            this.btnConsoleOpen.Size = new System.Drawing.Size(75, 23);
            this.btnConsoleOpen.TabIndex = 3;
            this.btnConsoleOpen.Text = "Login";
            this.btnConsoleOpen.UseVisualStyleBackColor = true;
            this.btnConsoleOpen.Click += new System.EventHandler(this.btnConsoleOpen_Click);
            // 
            // btnConsoleExecute
            // 
            this.btnConsoleExecute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConsoleExecute.Enabled = false;
            this.btnConsoleExecute.Location = new System.Drawing.Point(170, 170);
            this.btnConsoleExecute.Name = "btnConsoleExecute";
            this.btnConsoleExecute.Size = new System.Drawing.Size(459, 23);
            this.btnConsoleExecute.TabIndex = 2;
            this.btnConsoleExecute.Text = "Execute!";
            this.btnConsoleExecute.UseVisualStyleBackColor = true;
            this.btnConsoleExecute.Click += new System.EventHandler(this.btnConsoleExecute_Click);
            // 
            // tbConsoleOutput
            // 
            this.tbConsoleOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConsoleOutput.Location = new System.Drawing.Point(8, 199);
            this.tbConsoleOutput.Multiline = true;
            this.tbConsoleOutput.Name = "tbConsoleOutput";
            this.tbConsoleOutput.Size = new System.Drawing.Size(783, 238);
            this.tbConsoleOutput.TabIndex = 1;
            // 
            // tbConsoleInput
            // 
            this.tbConsoleInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConsoleInput.Location = new System.Drawing.Point(8, 6);
            this.tbConsoleInput.Multiline = true;
            this.tbConsoleInput.Name = "tbConsoleInput";
            this.tbConsoleInput.Size = new System.Drawing.Size(783, 158);
            this.tbConsoleInput.TabIndex = 0;
            this.tbConsoleInput.Text = "/system/resource/print";
            // 
            // tcpParser
            // 
            this.tcpParser.Controls.Add(this.btnParserGenerate);
            this.tcpParser.Controls.Add(label6);
            this.tcpParser.Controls.Add(this.tbParserFile);
            this.tcpParser.Controls.Add(this.btnParserSave);
            this.tcpParser.Controls.Add(this.tbParserOutput);
            this.tcpParser.Location = new System.Drawing.Point(4, 22);
            this.tcpParser.Name = "tcpParser";
            this.tcpParser.Padding = new System.Windows.Forms.Padding(3);
            this.tcpParser.Size = new System.Drawing.Size(799, 445);
            this.tcpParser.TabIndex = 3;
            this.tcpParser.Text = "Parser";
            this.tcpParser.UseVisualStyleBackColor = true;
            // 
            // btnParserGenerate
            // 
            this.btnParserGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParserGenerate.Location = new System.Drawing.Point(716, 4);
            this.btnParserGenerate.Name = "btnParserGenerate";
            this.btnParserGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnParserGenerate.TabIndex = 11;
            this.btnParserGenerate.Text = "Generator >";
            this.btnParserGenerate.UseVisualStyleBackColor = true;
            this.btnParserGenerate.Click += new System.EventHandler(this.btnParserGenerate_Click);
            // 
            // tbParserFile
            // 
            this.tbParserFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbParserFile.Location = new System.Drawing.Point(70, 6);
            this.tbParserFile.Name = "tbParserFile";
            this.tbParserFile.Size = new System.Drawing.Size(559, 20);
            this.tbParserFile.TabIndex = 9;
            this.tbParserFile.Text = ".\\defs";
            // 
            // btnParserSave
            // 
            this.btnParserSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParserSave.Location = new System.Drawing.Point(635, 4);
            this.btnParserSave.Name = "btnParserSave";
            this.btnParserSave.Size = new System.Drawing.Size(75, 23);
            this.btnParserSave.TabIndex = 8;
            this.btnParserSave.Text = "Save";
            this.btnParserSave.UseVisualStyleBackColor = true;
            this.btnParserSave.Click += new System.EventHandler(this.btnParserSave_Click);
            // 
            // tbParserOutput
            // 
            this.tbParserOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbParserOutput.Location = new System.Drawing.Point(6, 31);
            this.tbParserOutput.Multiline = true;
            this.tbParserOutput.Name = "tbParserOutput";
            this.tbParserOutput.Size = new System.Drawing.Size(787, 406);
            this.tbParserOutput.TabIndex = 7;
            // 
            // tcpSourceCode
            // 
            this.tcpSourceCode.Controls.Add(this.eSourceCodeDesigner);
            this.tcpSourceCode.Controls.Add(this.eSourceCodeCustom);
            this.tcpSourceCode.Location = new System.Drawing.Point(4, 22);
            this.tcpSourceCode.Name = "tcpSourceCode";
            this.tcpSourceCode.Padding = new System.Windows.Forms.Padding(3);
            this.tcpSourceCode.Size = new System.Drawing.Size(799, 445);
            this.tcpSourceCode.TabIndex = 0;
            this.tcpSourceCode.Text = "Generate source code";
            this.tcpSourceCode.UseVisualStyleBackColor = true;
            // 
            // eSourceCodeDesigner
            // 
            this.eSourceCodeDesigner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.eSourceCodeDesigner.Location = new System.Drawing.Point(6, 191);
            this.eSourceCodeDesigner.Multiline = true;
            this.eSourceCodeDesigner.Name = "eSourceCodeDesigner";
            this.eSourceCodeDesigner.Size = new System.Drawing.Size(787, 246);
            this.eSourceCodeDesigner.TabIndex = 7;
            // 
            // eSourceCodeCustom
            // 
            this.eSourceCodeCustom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.eSourceCodeCustom.Location = new System.Drawing.Point(6, 6);
            this.eSourceCodeCustom.Multiline = true;
            this.eSourceCodeCustom.Name = "eSourceCodeCustom";
            this.eSourceCodeCustom.Size = new System.Drawing.Size(787, 179);
            this.eSourceCodeCustom.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 471);
            this.Controls.Add(this.tcMain);
            this.Name = "Form1";
            this.Text = "Mikrotik.Api entity code generator";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.tcMain.ResumeLayout(false);
            this.tcpCredentials.ResumeLayout(false);
            this.tcpCredentials.PerformLayout();
            this.tcpConsole.ResumeLayout(false);
            this.tcpConsole.PerformLayout();
            this.tcpParser.ResumeLayout(false);
            this.tcpParser.PerformLayout();
            this.tcpSourceCode.ResumeLayout(false);
            this.tcpSourceCode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tcpSourceCode;
        private System.Windows.Forms.TextBox eSourceCodeCustom;
        private System.Windows.Forms.TabPage tcpConsole;
        private System.Windows.Forms.TabPage tcpCredentials;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.TextBox tbConsoleInput;
        private System.Windows.Forms.TextBox tbConsoleOutput;
        private System.Windows.Forms.Button btnConsoleExecute;
        private System.Windows.Forms.Button btnConsoleOpen;
        private System.Windows.Forms.Button btnConsoleClose;
        private System.Windows.Forms.Button btnConsoleClear;
        private System.Windows.Forms.Button btnConsoleSendToParser;
        private System.Windows.Forms.TabPage tcpParser;
        private System.Windows.Forms.TextBox tbParserOutput;
        private System.Windows.Forms.TextBox tbParserFile;
        private System.Windows.Forms.Button btnParserSave;
        private System.Windows.Forms.Button btnParserGenerate;
        private System.Windows.Forms.TextBox eSourceCodeDesigner;
        private System.Windows.Forms.Button btnActivateConsole;

    }
}

