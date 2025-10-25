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
    
    private static string ResolveTranslationsPath(string language)
    {
        // If running under ASP.NET, use MapPath
        if (HttpContext.Current != null && HttpContext.Current.Server != null)
        {
            string webDir = HttpContext.Current.Server.MapPath("~/translations/" + language);
            if (Directory.Exists(webDir)) return webDir;
        }

        // Fallback for non-web contexts (e.g., unit tests). Traverse up from base directory
        var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        while (dir != null)
        {
            // Look for /translations/<language>
            string candidate = Path.Combine(dir.FullName, "translations", language);
            if (Directory.Exists(candidate)) return candidate;

            // Look for /www/translations/<language>
            string wwwCandidate = Path.Combine(dir.FullName, "www", "translations", language);
            if (Directory.Exists(wwwCandidate)) return wwwCandidate;

            dir = dir.Parent;
        }

        return null;
    }

    public static string CurrentLanguage
    {
        get { return _currentLanguage; }
    }

    public static bool SetLanguage(string language)
    {
        if (string.IsNullOrEmpty(language)) language = "ES_es";
        lock (_lock)
        {
            bool same = string.Equals(_currentLanguage, language, StringComparison.OrdinalIgnoreCase);

            // Always (re)load to pick up new/changed strings, even if same language
            _strings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _currentLanguage = language;

            string baseDir = ResolveTranslationsPath(language);
            if (string.IsNullOrEmpty(baseDir) || !Directory.Exists(baseDir)) return false;

            var serializer = new JavaScriptSerializer();
            foreach (var file in Directory.GetFiles(baseDir, "*.json", SearchOption.TopDirectoryOnly))
            {
                string prefix = Path.GetFileNameWithoutExtension(file);
                string json = File.ReadAllText(file);
                var dict = serializer.Deserialize<Dictionary<string, object>>(json);
                if (dict == null) continue;
                foreach (var kv in dict)
                {
                    if (kv.Value is string value)
                    {
                        string key = prefix + "." + kv.Key;
                        _strings[key] = value;
                    }
                }
            }

            return !same; // preserve previous API semantics for callers/tests
        }
    }

    public static string Get(string key)
    {
        if (_strings != null && _strings.TryGetValue(key, out string value))
            return value;
        return key;
    }
}
