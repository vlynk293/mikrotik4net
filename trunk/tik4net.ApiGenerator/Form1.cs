using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tik4Net.Session;
using Tik4Net.ApiGenerator.Properties;

namespace Tik4Net.ApiGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string source = GenerateEntitySouceCodeFromMk(ePath.Text.ToLower());
            eSourceCode.Lines = source.Split('\n');
            eSourceCode.Focus();
            eSourceCode.SelectAll();
            Clipboard.SetText(source);
        }

        public string GenerateEntitySouceCodeFromMk(string entityPath)
        {
            ITikEntityRow entityRow = ReadEntityFromRouter(entityPath);
            string source;
            try
            {
                if (entityRow == null)
                    source = string.Format("There must be at least one item in '{0}'", entityPath);
                else
                    source = GenerateSource(entityRow, entityPath);


            }
            catch (Exception ex)
            {
                source = ex.ToString();
            }

            return source;
        }

        private ITikEntityRow ReadEntityFromRouter(string entityPath)
        {
            using (ITikSession session = new Tik4Net.Session.Api.ApiSession())
            {
                session.Open("10.43.101.1"); //TODO
                session.LogOn("test", "testp1234");

                IEnumerable<ITikEntityRow> rows = session.QueryDataRows(entityPath);
                session.LogOff();

                if (rows.Count() == 0)
                    return null;
                else
                    return rows.First();
            }            
            //debug return new Tik4Net.Session.Api.ApiEntityRow(@"!re=.id=*100004C=name=Jenstejn - local=parent=TOP=packet-mark==limit-at=1000000=queue=wireless-default=priority=6=max-limit=8000000=burst-limit=0=burst-threshold=0=burst-time=00:00:00=bytes=0=packets=0=dropped=0=rate=0=packet-rate=0=queued-packets=0=queued-bytes=0=lends=0=borrows=0=pcq-queues=0=disabled=true=invalid=true");            
        }

        private string GenerateSource(ITikEntityRow row, string entityPath)
        {
            string[] pathItems = entityPath.Split(new char[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            //entityPath = entityPath;
            string entityDotedPath = string.Join(".", pathItems.Take(pathItems.Length - 1).Select(i => StringHelper.Camelize(i)).ToArray());
            if (!string.IsNullOrEmpty(entityDotedPath))
                entityDotedPath = "." + entityDotedPath;
            string entityName = string.Join("", pathItems.Skip(pathItems.Length - 2).Select(i => StringHelper.Camelize(i)).ToArray()); 
            string entityDataRow = row.DataRow;
            
            List<string> entityProperties = new List<string>(row.Keys.Count());
            List<string> entityLoadCode = new List<string>(row.Keys.Count());

            List<string> fieldsOnBaseClass = new List<string>(new string[] { ".id" });

            foreach (string key in row.Keys.OrderBy(s=>s.ToString()))
            {
                if (!fieldsOnBaseClass.Contains(key, StringComparer.OrdinalIgnoreCase))
                {
                    string propertyName = StringHelper.Camelize(key);
                    //string fieldName = StringHelper.CamelizeFirstLower(key);
                    string propertyDataName = key;
                    FieldType fieldType = StringHelper.DetermineFieldTypeFromValue(row.GetValue(key));
                    string fieldTypeName = StringHelper.FieldTypeToClrName(fieldType);
                    string getMethod = StringHelper.FieldTypeToGetMethod(fieldType);

                    //entityFields.Add(Resources.FieldTemplate
                    //    .Replace("%FieldName%", fieldName)
                    //    .Replace("%FieldType%", fieldTypeName));
                    entityProperties.Add(Resources.PropertyTemplate
                        .Replace("%PropertyDataName%", propertyDataName)
                        .Replace("%PropertyName%", propertyName)
                        .Replace("%FieldType%", fieldTypeName)
                        .Replace("%GetMethod%", getMethod));
                    string loadTemplate = StringHelper.FieldTypeToDataLoadTemplate(fieldType);

                    entityLoadCode.Add(loadTemplate
                        .Replace("%PropertyDataName%", propertyDataName));
                }
            }

            //StringHelper.TrimFirstItem(entityFields);
            StringHelper.TrimFirstItem(entityProperties);
            StringHelper.TrimFirstItem(entityLoadCode);

            string result = Resources.ClassTemplate
                .Replace("%EntityDotedPath%", entityDotedPath)
                .Replace("%EntityPath%", entityPath)
                .Replace("%EntityName%", entityName)
                .Replace("%EntityProperties%", string.Join(Environment.NewLine+Environment.NewLine, entityProperties.ToArray()))
                .Replace("%EntityDataRow%", entityDataRow)
                .Replace("%EntityLoadCode%", string.Join(Environment.NewLine, entityLoadCode.ToArray()));

            return result;
        }
    }
}


