using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

public static class TranslationService
{
    private static readonly object _lock = new object();
    private static string _currentLanguage = null;
    private static Dictionary<string, string> _strings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    public static string CurrentLanguage
    {
        get { return _currentLanguage; }
    }

    public static void SetLanguage(string language)
    {
        if (string.IsNullOrEmpty(language)) language = "ES_es";
        lock (_lock)
        {
            if (string.Equals(_currentLanguage, language, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _strings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _currentLanguage = language;

            string baseDir = HttpContext.Current.Server.MapPath("~/translations/" + language);
            if (!Directory.Exists(baseDir)) return;

            var serializer = new JavaScriptSerializer();
            foreach (var file in Directory.GetFiles(baseDir, "*.json", SearchOption.TopDirectoryOnly))
            {
                string prefix = Path.GetFileNameWithoutExtension(file);
                try
                {
                    string json = File.ReadAllText(file);
                    var dict = serializer.Deserialize<Dictionary<string, object>>(json);
                    if (dict == null) continue;
                    foreach (var kv in dict)
                    {
                        var value = kv.Value as string;
                        if (value != null)
                        {
                            string key = prefix + "." + kv.Key;
                            _strings[key] = value;
                        }
                    }
                }
                catch { }
            }
        }
    }

    public static string Get(string key)
    {
        string value;
        if (_strings != null && _strings.TryGetValue(key, out value))
            return value;
        return key;
    }
}
