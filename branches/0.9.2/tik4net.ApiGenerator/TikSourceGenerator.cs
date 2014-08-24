using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Tik4Net.ApiGenerator.Properties;

namespace Tik4Net.ApiGenerator
{
    internal class TikSourceGenerator
    {
        private XmlDocument defDocument;

        internal TikSourceGenerator(string defFile)
        {
            Guard.ArgumentNotNullOrEmptyString(defFile, "defFile");

            XmlDocument defDocument = new XmlDocument();
            defDocument.Load(defFile);

            Init(defDocument);
        }

        internal TikSourceGenerator(XmlDocument defDocument)
        {
            Guard.ArgumentNotNull(defDocument, "defDocument");

            Init(defDocument);
        }

        private void Init(XmlDocument defDocument)
        {
            this.defDocument = defDocument;
        }

        internal void GenerateSource(out string designerCode, out string customCodeTemplate)
        {
            XmlNode rootNode = defDocument.DocumentElement;

            string entityPath = rootNode.Attributes["entityPath"].Value;
            string entityName = rootNode.Attributes["entityName"].Value;
            string entityNamespace = rootNode.Attributes["entityNamespace"].Value;
            string entityExampleRow = rootNode.Attributes["entityExampleRow"].Value;
            string entityEditMode = rootNode.Attributes["editMode"].Value;
            string entityReaderFlags;
            if (rootNode.Attributes["readerFlags"] == null)
                entityReaderFlags = typeof(Tik4Net.Connector.ExecuteReaderBehaviors).FullName + "." + Tik4Net.Connector.ExecuteReaderBehaviors.None.ToString();
            else
                entityReaderFlags = string.Join("|", rootNode.Attributes["readerFlags"].Value.Split(',').Select(s=>typeof(Tik4Net.Connector.ExecuteReaderBehaviors).FullName + "." + s.Trim()).ToArray());
            TikListMode listMode = (TikListMode)Enum.Parse(typeof(TikListMode), rootNode.Attributes["listMode"].Value, true);
            string setterPossibleRem;
            string listRwMethods;
            bool isListEditable;
            if (entityEditMode == "ReadOnly")
            {
                setterPossibleRem = "// Entity R/O ";
                listRwMethods = ""; //remove
                isListEditable = false;
            }
            else
            {
                setterPossibleRem = "";
                listRwMethods = Resources.ListRWMethodsTemplate;
                isListEditable = true;
            }
            
            //properties
            bool containsIdProperty = rootNode.SelectSingleNode("property[@name=\".id\"]") != null;
            List<string> fieldsAlreadyImplementedInBaseClass = new List<string>(new string[] { /*".id" - is not in all objects -> moved to generated class*/});
            List<string> autoPropertiesSource = new List<string>();
            List<string> customPropertiesSource = new List<string>();
            foreach (XmlNode propertyNode in rootNode.SelectNodes("property"))
            {
                string propertyName = propertyNode.Attributes["name"].Value;

                if (!fieldsAlreadyImplementedInBaseClass.Contains(propertyName, StringComparer.OrdinalIgnoreCase))
                {
                    string propertyMode = propertyNode.Attributes["mode"].Value;
                    string propertyFieldName = propertyNode.Attributes["fieldName"].Value;
                    string propertyType = propertyNode.Attributes["type"].Value;
                    bool propertyMandatory = StringHelper.StrToBool(propertyNode.Attributes["mandatory"].Value);
                    string propertyEditMode = propertyNode.Attributes["editMode"].Value;
                    bool propertyReadOnly = propertyEditMode == "ReadOnly";
                    string propertyClrType = propertyMandatory & !propertyReadOnly ? propertyType : StringHelper.MakeClrTypeNulable(propertyType);
                    string propertyPossibleSetterRem = setterPossibleRem;                   

                    if (propertyReadOnly)
                        propertyPossibleSetterRem = "// Property R/O ";
                    
                    if (propertyMode == "auto")
                    {
                        string propertySource = Resources.AutoPropertyTemplate
                            .Replace("%PropertyDataName%", propertyName)
                            .Replace("%PropertyName%", propertyFieldName)
                            .Replace("%FieldType%", propertyClrType)
                            .Replace("%GetMethod%", StringHelper.PropertyTypeToGetMethod(propertyType, propertyMandatory & !propertyReadOnly)) //R/O property is null in New object
                            .Replace("%Mandatory%", StringHelper.BoolToStr(propertyMandatory))
                            .Replace("%SetterPossibleRem%", propertyPossibleSetterRem)
                            .Replace("%PropertyEditMode%", propertyEditMode);
                        autoPropertiesSource.Add(propertySource);

                        if (propertyMandatory && !propertyReadOnly)
                        {
                            //Add _OrNull property variant.
                            string propertyOrNullSource = Resources.AutoProperty_OrNull_Template
                                .Replace("%PropertyDataName%", propertyName)
                                .Replace("%PropertyName%", propertyFieldName)
                                .Replace("%FieldType%", StringHelper.MakeClrTypeNulable(propertyType)) //always nullable
                                .Replace("%GetMethod%", StringHelper.PropertyTypeToGetMethod(propertyType, false));
                            autoPropertiesSource.Add(propertyOrNullSource);
                        }
                    }
                    else if (propertyMode == "custom")
                    {
                        string propertySource = Resources.CustomPropertyTemplate
                            .Replace("%PropertyDataName%", propertyName)
                            .Replace("%PropertyName%", propertyFieldName)
                            .Replace("%FieldType%", propertyClrType)
                            .Replace("%Mandatory%", StringHelper.BoolToStr(propertyMandatory))
                            .Replace("%PropertyEditMode%", propertyEditMode);
                        customPropertiesSource.Add(propertySource);
                    }
                    else 
                        throw new NotImplementedException(string.Format("Unsuported propertyMode {0}.", propertyMode));                    
                }
            }
            StringHelper.TrimFirstItem(autoPropertiesSource);
            StringHelper.TrimFirstItem(customPropertiesSource);

            designerCode = Resources.AutoClassTemplate
                .Replace("%EntityDotedPath%", string.IsNullOrEmpty(entityNamespace) ? "" : "." + entityNamespace)
                .Replace("%EntityPath%", entityPath)
                .Replace("%EntityName%", entityName)
                .Replace("%EntityProperties%", string.Join("\r\n\r\n", autoPropertiesSource.ToArray()))
                .Replace("%EntityDataRow%", entityExampleRow)
                .Replace("%DateTime%", DateTime.Now.ToString())
                .Replace("%EditMode%", entityEditMode)
                .Replace("%ListType%", StringHelper.GetListClassFromListMode(listMode))
                .Replace("%PossibleITikEntityWithId%", containsIdProperty ? ", ITikEntityWithId" : "")
                .Replace("%PossibleRWMethods%", listRwMethods /*or empty string*/)
                .Replace("%PossibleIEditableTikList%", isListEditable ? ", IEditableTikList" : "")
                .Replace("%EntityReadFlags%", entityReaderFlags); 
            customCodeTemplate = Resources.CustomClassTemplate
                .Replace("%EntityDotedPath%", string.IsNullOrEmpty(entityNamespace) ? "" : "." + entityNamespace)
                .Replace("%EntityPath%", entityPath)
                .Replace("%EntityName%", entityName)
                .Replace("%EntityProperties%", string.Join("\r\n\r\n", customPropertiesSource.ToArray()))
                .Replace("%EntityDataRow%", entityExampleRow)
                .Replace("%ListType%", StringHelper.GetListClassFromListMode(listMode));
        }
    }
}
