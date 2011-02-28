using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tik4Net.ApiGenerator.Properties;
using System.Xml;
using System.IO;
using Tik4Net.Connector.Api;

namespace Tik4Net.ApiGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Application.Idle += new EventHandler(Application_Idle);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        }

        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (consoleSession != null)
            {
                consoleSession.Dispose();
                consoleSession = null;
            }
        }

        void Application_Idle(object sender, EventArgs e)
        {
            //console buttons
            btnConsoleClose.Enabled = consoleSession != null;
            btnConsoleExecute.Enabled = consoleSession != null;
            btnConsoleOpen.Enabled = consoleSession == null;
            btnConsoleSendToParser.Enabled = !string.IsNullOrEmpty(consoleLastCommand) && (consoleLastResponse != null);
            tbConsoleInput.Enabled = consoleSession != null;

            //parser buttons
            btnParserSave.Enabled = !string.IsNullOrEmpty(tbParserOutput.Text) && !string.IsNullOrEmpty(tbParserFile.Text);
            btnParserGenerate.Enabled = !string.IsNullOrEmpty(tbParserOutput.Text) && !string.IsNullOrEmpty(tbParserFile.Text);
        }

        private void btnActivateConsole_Click(object sender, EventArgs e)
        {
            tcMain.SelectedTab = tcpConsole;
        }

        #region -- Console --
        private TikSession consoleSession;
        private string consoleLastCommand;
        private List<string> consoleLastResponse;

        private void btnConsoleOpen_Click(object sender, EventArgs e)
        {
            if (consoleSession != null)
                throw new Exception("Console session is already active.");
            else
            {
                consoleSession = new TikSession(TikConnectorType.Api);
                consoleSession.Open(tbHost.Text, int.Parse(tbPort.Text), tbUser.Text, tbPassword.Text);
            }
        }

        private void btnConsoleClose_Click(object sender, EventArgs e)
        {
            if (consoleSession != null)
            {
                consoleSession.Dispose();
                consoleSession = null;
            }
        }

        private void btnConsoleExecute_Click(object sender, EventArgs e)
        {
            if (tbConsoleInput.SelectedText != "")
                consoleLastCommand = tbConsoleInput.SelectedText.Replace(Environment.NewLine, "\n");
            else
                consoleLastCommand = string.Join("\n", tbConsoleInput.Lines.ToArray());

            tbConsoleOutput.AppendText(Environment.NewLine);
            tbConsoleOutput.AppendText(Environment.NewLine);
            tbConsoleOutput.AppendText(string.Join(Environment.NewLine, consoleLastCommand.Split('\n').Select(l => ">> " + l).ToArray()));
            tbConsoleOutput.AppendText(Environment.NewLine);

            consoleLastResponse = ((IApiConnector)consoleSession.Connector).Execute(consoleLastCommand);

            tbConsoleOutput.AppendText(string.Join(Environment.NewLine, consoleLastResponse.ToArray()));
            //tbConsoleInput.Text = "";
        }

        private void btnConsoleClear_Click(object sender, EventArgs e)
        {
            tbConsoleOutput.Text = "";
        }

        #endregion 

        #region -- PARSER --
        private void btnConsoleSendToParser_Click(object sender, EventArgs e)
        {
            tcMain.SelectedTab = tcpParser;
            TikPropertyParser parser = new TikPropertyParser(consoleSession, consoleLastCommand, consoleLastResponse);
            XmlDocument lastParserOutput = parser.GenerateOutput();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            lastParserOutput.Save(writer);
            tbParserOutput.Text = writer.ToString();
            string entityFqn = string.IsNullOrEmpty(parser.EntityNamespace) ? parser.EntityName : parser.EntityNamespace + "." + parser.EntityName;
            tbParserFile.Text = Path.Combine(Path.Combine(Environment.CurrentDirectory,  "defs"), entityFqn + ".xml");
        }

        private void btnParserSave_Click(object sender, EventArgs e)
        {
            XmlDocument lastParserOutput = new XmlDocument();
            lastParserOutput.LoadXml(tbParserOutput.Text);
             
            lastParserOutput.Save(tbParserFile.Text);
        }

        private void btnParserGenerate_Click(object sender, EventArgs e)
        {
            XmlDocument lastParserOutput = new XmlDocument();
            lastParserOutput.LoadXml(tbParserOutput.Text);
            tcMain.SelectedTab = tcpSourceCode;
            TikSourceGenerator generator = new TikSourceGenerator(lastParserOutput);
            string designerCode;
            string userCode;
            generator.GenerateSource(out designerCode, out userCode);

            eSourceCodeDesigner.Text = designerCode;
            eSourceCodeCustom.Text = userCode;
        }
        #endregion
    }
}


