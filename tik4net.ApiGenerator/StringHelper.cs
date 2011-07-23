using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Tik4Net.ApiGenerator.Properties;

namespace Tik4Net.ApiGenerator
{
    public static class StringHelper
    {
        public static string Camelize(string str)
        {
            string result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
            result = result.Replace("-", "");
            result = result.Replace(".", "");
            return result;
        }

        public static string CamelizeFirstLower(string str)
        {
            string result = Camelize(str);
            if (!string.IsNullOrEmpty(str))
                result = char.ToLower(str[0], CultureInfo.InvariantCulture) + result.Substring(1);

            return result;
        }

        public static void TrimFirstItem(List<string> stringList)
        {
            if (stringList.Count > 0)
                stringList[0] = stringList[0].Trim();
        }

        public static FieldType DetermineFieldTypeFromValue(string str, bool allowNulable)
        {
            if (str.Equals("true", StringComparison.OrdinalIgnoreCase)
                || (str.Equals("false", StringComparison.OrdinalIgnoreCase))
                || (str.Equals("yes", StringComparison.OrdinalIgnoreCase))
                || (str.Equals("no", StringComparison.OrdinalIgnoreCase))
                )
                return allowNulable ? FieldType.BoolNulable : FieldType.Bool;
            else
            {
                long tmp;
                if (long.TryParse(str, out tmp))
                {
                    return allowNulable ? FieldType.Int64Nulable : FieldType.Int64;; 
                }
                else
                    return allowNulable ? FieldType.StringNulable : FieldType.String;
            }
        }

        public static string FieldTypeToClrBaseName(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Bool: return "bool";
                case FieldType.BoolNulable: return "bool";
                case FieldType.Int64: return "long";
                case FieldType.Int64Nulable: return "long";
                case FieldType.String: return "string";
                case FieldType.StringNulable: return "string";
                default:
                    throw new NotImplementedException();
            }
        }

        internal static string MakeClrTypeNulable(string clrTypeName)
        {
            Guard.ArgumentNotNullOrEmptyString(clrTypeName, "clrTypeName");

            if (string.Equals(clrTypeName, "string", StringComparison.OrdinalIgnoreCase))
                return clrTypeName;
            else if (string.Equals(clrTypeName, "bool", StringComparison.OrdinalIgnoreCase))
                return clrTypeName + "?";
            else if (string.Equals(clrTypeName, "long", StringComparison.OrdinalIgnoreCase))
                return clrTypeName + "?";
            else
                throw new NotImplementedException(string.Format("Property CLR type '{0}' not supported.", clrTypeName));
        }

        internal static string GetListClassFromListMode(TikListMode mode)
        {
            switch (mode)
            {
                case TikListMode.Ordered:
                    return "TikList"; //TODO from typeof(TikList)
                case TikListMode.NotOrdered:
                    return "TikUnorderedList";
                case TikListMode.SingleRow:
                    return "TikSingleRowList";
                default:
                    throw new NotImplementedException(string.Format("TikListMode '{0}' not supported.", mode));
            }
        }

        //public static string FieldTypeToDataLoadTemplate(FieldType fieldType)
        //{
        //    switch (fieldType)
        //    {
        //        case FieldType.Bool: return Resources.LoadCodeBoolTemplate;
        //        case FieldType.Int64: return Resources.LoadCodeInt64Template;
        //        case FieldType.String: return Resources.LoadCodeTemplate;
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        //public static string FieldTypeToGetMethod(FieldType fieldType)
        //{
        //    //methods from AttributeList
        //    switch (fieldType)
        //    {
        //        case FieldType.Bool: return "GetAsBoolean";
        //        case FieldType.BoolNulable: return "GetAsBooleanOrNull";
        //        case FieldType.Int64: return "GetAsInt64";
        //        case FieldType.Int64Nulable: return "GetAsInt64OrNull";
        //        case FieldType.String: return "GetAsString";
        //        case FieldType.StringNulable: return "GetAsStringOrNull";
        //        default:
        //            throw new NotImplementedException();
        //    }

        //}

        public static string PropertyTypeToGetMethod(string propertyType, bool mandatory)
        {
            //methods from AttributeList
            string result;
            if (propertyType == "string")
                result = "GetAsString";
            else if (propertyType == "bool")
                result = "GetAsBoolean";
            else if  (propertyType == "long")
                result = "GetAsInt64";
            else
                throw new NotImplementedException(string.Format("Property type '{0}' not supported.", propertyType));

            if (!mandatory)
                result += "OrNull"; //"GetAsInt64OrNull";

            return result;
        }


        internal static bool DetermineFieldNulable(string key)
        {
            //TODO - better heurestic
            if (string.Equals(key, ".id", StringComparison.OrdinalIgnoreCase)
                || string.Equals(key, "name", StringComparison.OrdinalIgnoreCase)
                || string.Equals(key, "disabled", StringComparison.OrdinalIgnoreCase))
                return false;
            else
                return true; 
        }

        internal static void GetEntityFqn(string entityPath, out string entityNamespace, out string entityName)
        {
            Guard.ArgumentNotNullOrEmptyString(entityPath, "entityPath");

            string[] pathItems = entityPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            //entityPath = entityPath;
            entityNamespace = string.Join(".", pathItems.Take(pathItems.Length - 1).Select(i => StringHelper.Camelize(i)).ToArray());
            //if (!string.IsNullOrEmpty(entityNamespace))
            //    entityNamespace = "." + entityNamespace;
            entityName = string.Join("", pathItems.Skip(pathItems.Length - 2).Select(i => StringHelper.Camelize(i)).ToArray());
        }


        internal static bool IsTypeNulable(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.BoolNulable:
                case FieldType.Int64Nulable:
                case FieldType.StringNulable:
                    return true;
                case FieldType.Bool:
                case FieldType.Int64:
                case FieldType.String:
                    return false;
                default:
                    throw new NotImplementedException(string.Format("Not supported FieldType '{0}'.", fieldType));
            }
        }

        internal static bool StrToBool(string boolStr)
        {
            if (string.Equals(boolStr, "true", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (string.Equals(boolStr, "false", StringComparison.OrdinalIgnoreCase))
                return false;
            else
                throw new NotImplementedException(string.Format("Bool value '{0}' not supported.", boolStr));

        }

        internal static string BoolToStr(bool val)
        {
            return val ? "true" : "false";
        }
    }
}
