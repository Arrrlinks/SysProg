using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace EasySave_Graphique.Models;

public class Settings_m
{
    private static readonly object _lock = new object();
    public void UpdateConfigFile(string key, string? value)
    {
        lock (_lock)
        {
            string configFilePath = "../../../config.json";
            List<Dictionary<string, object?>> config;
            string configJson;
            if (!File.Exists(configFilePath) || string.IsNullOrWhiteSpace(File.ReadAllText(configFilePath)))
            {
                config = new List<Dictionary<string, object?>>
                {
                new Dictionary<string, object?> { { "Name", "Lang" }, { "Lang", "fr" } },
                new Dictionary<string, object?> { { "Name", "Format" }, { "Format", "json" } },
                new Dictionary<string, object?> { { "Name", "SaveMode" }, { "SaveMode", "complete" } },
                new Dictionary<string, object?> { { "Name", "SizeLimit" }, { "SizeLimit", null } },
                new Dictionary<string, object?> { { "Name", "Extensions" }, { "Extensions", new List<string> { "txt", "json" } } }
            };
            }
            else
            {
                try
                {
                    configJson = File.ReadAllText(configFilePath);
                    config = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(configJson);
                }
                catch (JsonException)
                {
                    config = new List<Dictionary<string, object?>>
                    {
                    new Dictionary<string, object?> { { "Name", "Lang" }, { "Lang", "fr" } },
                    new Dictionary<string, object?> { { "Name", "Format" }, { "Format", "json" } },
                    new Dictionary<string, object?> { { "Name", "SaveMode" }, { "SaveMode", "complete" } },
                    new Dictionary<string, object?> { { "Name", "SizeLimit" }, { "SizeLimit", null } },
                    new Dictionary<string, object?> { { "Name", "Extensions" }, { "Extensions", new List<string> { "txt", "json" } } }
                };
                }
            }
            var item = config.Find(dict => dict.ContainsKey("Name") && dict["Name"].ToString() == key);

            if (item != null)
            {
                if (key == "Lang" || key == "Format" || key == "SaveMode")
                {
                    item[key] = value;
                }
                else if (key == "Extensions")
                {
                    item[key] = value.Split(',').ToList();
                }
                else if (key == "SizeLimit")
                {
                    item[key] = string.IsNullOrWhiteSpace(value) ? null : int.Parse(value);
                }
            }
            configJson = JsonSerializer.Serialize(config);
            File.WriteAllText(configFilePath, configJson);
        }
    }
}