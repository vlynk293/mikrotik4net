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

        public static FieldType DetermineFieldTypeFromValue(string str)
        {
            if (str.Equals("true", StringComparison.OrdinalIgnoreCase)
                || (str.Equals("false", StringComparison.OrdinalIgnoreCase)))
                return FieldType.Bool;
            else
            {
                long tmp;
                if (long.TryParse(str, out tmp))
                    return FieldType.Int64;
                else
                    return FieldType.String;
            }
        }

        public static string FieldTypeToClrName(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Bool: return "bool";
                case FieldType.Int64: return "long";
                case FieldType.String: return "string";
                default:
                    throw new NotImplementedException();
            }
        }

        public static string FieldTypeToDataLoadTemplate(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Bool: return Resources.LoadCodeBoolTemplate;
                case FieldType.Int64: return Resources.LoadCodeInt64Template;
                case FieldType.String: return Resources.LoadCodeTemplate;
                default:
                    throw new NotImplementedException();
            }
        }

        public static string FieldTypeToGetMethod(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.Bool: return "GetAsBoolean";
                case FieldType.Int64: return "GetAsInt64";
                case FieldType.String: return "GetAsString";
                default:
                    throw new NotImplementedException();
            }

        }
    }
}
