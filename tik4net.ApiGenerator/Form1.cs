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

        //public string GenerateEntitySouceCodeFromMk(string entityPath)
        //{
        //    ITikEntityRow entityRow = ReadEntityFromRouter(entityPath);
        //    string source;
        //    try
        //    {
        //        if (entityRow == null)
        //            source = string.Format("There must be at least one item in '{0}'", entityPath);
        //        else
        //            source = GenerateSource(entityRow, entityPath);


        //    }
        //    catch (Exception ex)
        //    {
        //        source = ex.ToString();
        //    }

        //    return source;
        //}

        //private ITikEntityRow ReadEntityFromRouter(string entityPath)
        //{
        //    using (TikSession session = new TikSession(TikConnectorType.Api))
        //    {
        //        session.Open("10.43.94.197", "test", "testp1234"); //TODO

        //        IEnumerable<ITikEntityRow> rows = session.Connector.QueryDataRows(entityPath);

        //        //TODO pass all rows and make union of attributes
        //        if (rows.Count() == 0)
        //            return null;
        //        else
        //            return rows.First();
        //    }            
        //    //debug return new Tik4Net.Session.Api.ApiEntityRow(@"!re=.id=*100004C=name=Jenstejn - local=parent=TOP=packet-mark==limit-at=1000000=queue=wireless-default=priority=6=max-limit=8000000=burst-limit=0=burst-threshold=0=burst-time=00:00:00=bytes=0=packets=0=dropped=0=rate=0=packet-rate=0=queued-packets=0=queued-bytes=0=lends=0=borrows=0=pcq-queues=0=disabled=true=invalid=true");            
        //}

        //private string GenerateSource(ITikEntityRow row, string entityPath)
        //{
        //    string[] pathItems = entityPath.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

        //    //entityPath = entityPath;
        //    string entityDotedPath = string.Join(".", pathItems.Take(pathItems.Length - 1).Select(i => StringHelper.Camelize(i)).ToArray());
        //    if (!string.IsNullOrEmpty(entityDotedPath))
        //        entityDotedPath = "." + entityDotedPath;
        //    string entityName = string.Join("", pathItems.Skip(pathItems.Length - 2).Select(i => StringHelper.Camelize(i)).ToArray()); 
        //    string entityDataRow = row.DataRow;
            
        //    List<string> entityProperties = new List<string>(row.Keys.Count());
        //    //List<string> entityLoadCode = new List<string>(row.Keys.Count());

        //    List<string> fieldsOnBaseClass = new List<string>(new string[] { ".id" });

        //    foreach (string key in row.Keys.OrderBy(s=>s.ToString()))
        //    {
        //        if (!fieldsOnBaseClass.Contains(key, StringComparer.OrdinalIgnoreCase))
        //        {
        //            string propertyName = StringHelper.Camelize(key);
        //            //string fieldName = StringHelper.CamelizeFirstLower(key);
        //            string propertyDataName = key;
        //            bool nullable = StringHelper.DetermineFieldNulable(key);
        //            FieldType fieldType = StringHelper.DetermineFieldTypeFromValue(row.GetStringValueOrNull(key, true), nullable);
        //            string fieldTypeName = StringHelper.FieldTypeToClrName(fieldType);
        //            string getMethod = StringHelper.FieldTypeToGetMethod(fieldType);

        //            //entityFields.Add(Resources.FieldTemplate
        //            //    .Replace("%FieldName%", fieldName)
        //            //    .Replace("%FieldType%", fieldTypeName));
        //            entityProperties.Add(Resources.AutoPropertyTemplate
        //                .Replace("%PropertyDataName%", propertyDataName)
        //                .Replace("%PropertyName%", propertyName)
        //                .Replace("%FieldType%", fieldTypeName)
        //                .Replace("%GetMethod%", getMethod)
        //                .Replace("%Mandatory%", nullable ? "false" : "true"));
                    
        //            //string loadTemplate = StringHelper.FieldTypeToDataLoadTemplate(fieldType);
        //            //entityLoadCode.Add(loadTemplate
        //            //    .Replace("%PropertyDataName%", propertyDataName));
        //        }
        //    }

        //    //StringHelper.TrimFirstItem(entityFields);
        //    StringHelper.TrimFirstItem(entityProperties);
        //    //StringHelper.TrimFirstItem(entityLoadCode);

        //    string result = Resources.AutoClassTemplate
        //        .Replace("%EntityDotedPath%", entityDotedPath)
        //        .Replace("%EntityPath%", entityPath)
        //        .Replace("%EntityName%", entityName)
        //        .Replace("%EntityProperties%", string.Join("\r\n\r\n", entityProperties.ToArray()))
        //        .Replace("%EntityDataRow%", entityDataRow)
        //        .Replace("%DateTime%", DateTime.Now.ToString());
        //        //.Replace("%EntityLoadCode%", string.Join(Environment.NewLine, entityLoadCode.ToArray()));

        //    return result;
        //}

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

            consoleLastResponse = consoleSession.Connector.ExecuteAndReadResponse(consoleLastCommand);

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


