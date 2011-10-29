using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Tik4Net.ApiGenerator
{
    internal class TikPropertyParser
    {
        private readonly string entityPath;
        private readonly Dictionary<string, FieldType> props;
        private readonly string entityExampleRow;
        private readonly string entityName;
        private readonly string entityNamespace;

        internal string EntityName
        {
            get { return entityName; }
        }

        internal string EntityNamespace 
        {
            get { return entityNamespace; }
        }

        internal TikPropertyParser(TikSession session, string request, List<string> responseDataRows)
        {
            Guard.ArgumentNotNull(responseDataRows, "responseDataRows");
            Guard.ArgumentNotNullOrEmptyString(request, "request");

            //entityPath
            string[] items = request.Split('\n');
            if (items[0].Contains(@"/print")) //remove /print sufix if present
                this.entityPath = items[0].Substring(0, items[0].IndexOf(@"/print"));
            else
                this.entityPath = items[0];


            StringHelper.GetEntityFqn(entityPath, out entityNamespace, out entityName);

            //response -> properties
            props = new Dictionary<string, FieldType>();
            entityExampleRow = "";
            foreach (string responseDataRow in responseDataRows)
            {
                if (responseDataRow.StartsWith("!re"))
                {
                    if (string.IsNullOrEmpty(entityExampleRow))
                        entityExampleRow = responseDataRow;

                    ITikEntityRow row = session.Connector.CreateEntityRow(responseDataRow);
                    foreach (string key in row.Keys)
                    {
                        bool nullable = StringHelper.DetermineFieldNulable(key);
                        FieldType fieldType = StringHelper.DetermineFieldTypeFromValue(row.GetStringValueOrNull(key, true), nullable);

                        if (!props.ContainsKey(key))
                            props.Add(key, fieldType);
                        else
                        {
                            FieldType actualType = props[key];
                            if (actualType != fieldType) //set type=string if there more than one variant
                                props[key] = nullable ? FieldType.StringNulable : FieldType.String;
                        }
                    }
                }
            }
        }

        internal XmlDocument GenerateOutput()
        {
            XmlDocument result = new XmlDocument();
            //root
            XmlNode root = result.CreateElement("tikEntity");
            root.Attributes.Append(result.CreateAttribute("entityPath"));
            root.Attributes["entityPath"].Value = entityPath;
            root.Attributes.Append(result.CreateAttribute("entityName"));
            root.Attributes["entityName"].Value = entityName;
            root.Attributes.Append(result.CreateAttribute("entityNamespace"));
            root.Attributes["entityNamespace"].Value = entityNamespace;
            root.Attributes.Append(result.CreateAttribute("editMode"));
            root.Attributes["editMode"].Value = TikEntityEditMode.Editable.ToString();
            root.Attributes.Append(result.CreateAttribute("listMode"));
            root.Attributes["listMode"].Value = TikListMode.Ordered.ToString();
            root.Attributes.Append(result.CreateAttribute("entityExampleRow"));
            root.Attributes["entityExampleRow"].Value = entityExampleRow.Replace("\n", "\r\n");
            result.AppendChild(root);

            foreach (KeyValuePair<string, FieldType> property in props.OrderBy(p=>p.Key))
            {
                XmlNode propertyNode = result.CreateElement("property");
                propertyNode.Attributes.Append(result.CreateAttribute("name"));
                propertyNode.Attributes["name"].Value = property.Key;
                propertyNode.Attributes.Append(result.CreateAttribute("fieldName"));
                propertyNode.Attributes["fieldName"].Value = StringHelper.Camelize(property.Key);
                propertyNode.Attributes.Append(result.CreateAttribute("type"));
                propertyNode.Attributes["type"].Value = StringHelper.FieldTypeToClrBaseName(property.Value);
                propertyNode.Attributes.Append(result.CreateAttribute("mandatory"));
                propertyNode.Attributes["mandatory"].Value = StringHelper.IsTypeNulable(property.Value) ? "false" : "true";
                propertyNode.Attributes.Append(result.CreateAttribute("mode"));
                propertyNode.Attributes["mode"].Value = "auto";
                propertyNode.Attributes.Append(result.CreateAttribute("editMode"));
                propertyNode.Attributes["editMode"].Value = property.Key == ".id" ? TikPropertyEditMode.ReadOnly.ToString() : TikPropertyEditMode.Editable.ToString();

                root.AppendChild(propertyNode);
            }

            return result;
        }
    }
}
