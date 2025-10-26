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

    // Helper: try to resolve and validate a translations directory for a language
    private static bool TryResolveTranslationsPath(string language, out string path)
    {
        path = ResolveTranslationsPath(language);
        return !string.IsNullOrEmpty(path) && Directory.Exists(path);
    }

    // Helper: load all JSON translation files from a directory into a dictionary
    private static Dictionary<string, string> LoadTranslationsFromDirectory(string baseDir)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
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
                    result[key] = value;
                }
            }
        }

        return result;
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

            // Resolve translations folder first; do not mutate state if the language is not available
            if (!TryResolveTranslationsPath(language, out string baseDir))
            {
                return false;
            }

            // Load translations into temporary structure
            var newStrings = LoadTranslationsFromDirectory(baseDir);

            // Commit only after successful load
            _strings = newStrings ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _currentLanguage = language;

            // preserve previous API semantics: return false if setting same language
            return !same;
        }
    }

    public static string Get(string key)
    {
        if (_strings != null && _strings.TryGetValue(key, out string value))
            return value;
        return key;
    }
}
