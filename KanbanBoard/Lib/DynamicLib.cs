using Newtonsoft.Json;
using System.Text.Json;
using System.Xml;

namespace KanbanBoard.Lib
{
    public static class DynamicLib
    {
        public static dynamic? Convert(object? obj)
        {
            if (obj != null)
            {
                if (obj is JsonElement json)
                {
                    return JsonStringToDynamic(json.ToString());
                }
                if (obj is XmlDocument document)
                {
                    var _json = JsonConvert.SerializeXmlNode(document);
                    return JsonStringToDynamic(_json);
                }
            }
            return null;
        }

        private static dynamic? JsonStringToDynamic(string? json)
        {
            if (json != null)
            {
                return JsonConvert.DeserializeObject(json);
            }
            return null;
        }

        public static bool? IsPropertyExists(dynamic obj, string property)
        {
            var _property = obj[property];
            if (_property is null)
            {
                // property does not exist.
                return false;
            }
            else if (_property == null)
            {
                // property exists with a value of null.
                return null;
            }
            else
            {
                // property exists with a value.
                return true;
            }
        }
    }
}
