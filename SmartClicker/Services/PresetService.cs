//using System.Text.Json;
//using SmartClicker.Models;

//namespace SmartClicker.Services;

//public class PresetService
//{
//    private static readonly string PresetsDirectory =
//        Path.Combine(FileSystem.AppDataDirectory, "Presets");

//    public PresetService()
//    {
//        if (!Directory.Exists(PresetsDirectory))
//            Directory.CreateDirectory(PresetsDirectory);
//    }

//    public async Task SavePresetAsync(PresetData preset, string? name = null)
//    {
//        string fileName = string.IsNullOrWhiteSpace(name) ? GetNextPresetFileName() : $"{name}.json";
//        string filePath = Path.Combine(PresetsDirectory, fileName);

//        string json = JsonSerializer.Serialize(preset, new JsonSerializerOptions
//        {
//            WriteIndented = true
//        });

//        await File.WriteAllTextAsync(filePath, json);
//    }

//    public async Task<PresetData?> LoadPresetAsync(string fileName)
//    {
//        string filePath = Path.Combine(PresetsDirectory, fileName);
//        if (!File.Exists(filePath))
//            return null;

//        string json = await File.ReadAllTextAsync(filePath);
//        return JsonSerializer.Deserialize<PresetData>(json);
//    }

//    public List<string> GetAllPresetFiles()
//    {
//        return Directory.GetFiles(PresetsDirectory, "*.json")
//                        .Select(Path.GetFileName)
//                        .Where(name => name != null)
//                        .ToList()!;
//    }

//    public void DeletePreset(string name)
//    {
//        string path = Path.Combine(PresetsDirectory, $"{name}.json");
//        if (File.Exists(path))
//            File.Delete(path);
//    }

//    private string GetNextPresetFileName()
//    {
//        var files = Directory.GetFiles(PresetsDirectory, "Preset*.json");
//        int maxNumber = files
//            .Select(file => Path.GetFileNameWithoutExtension(file))
//            .Select(name => {
//                if (int.TryParse(name?.Replace("Preset", ""), out int n)) return n;
//                return 0;
//            })
//            .DefaultIfEmpty(0)
//            .Max();

//        return $"Preset{maxNumber + 1}.json";
//    }

//    // Обновлённый список методов для доступа
//    public string GetPresetFilePath(string name)
//    {
//        return Path.Combine(PresetsDirectory, $"{name}.json");
//    }
//}

using System.Text.Json;
using SmartClicker.Models;

namespace SmartClicker.Services;

public static class PresetService
{
    private static readonly string PresetsFolder =
        Path.Combine(FileSystem.AppDataDirectory, "Presets");

    static PresetService()
    {
        // Создаём папку при первом использовании
        if (!Directory.Exists(PresetsFolder))
            Directory.CreateDirectory(PresetsFolder);
    }

    public static async Task SavePresetAsync(PresetData preset, string name)
    {
        string path = GetPresetPath(name);
        string json = JsonSerializer.Serialize(preset, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(path, json);
    }

    public static async Task<PresetData?> LoadPresetAsync(string name)
    {
        string path = GetPresetPath(name);
        if (!File.Exists(path))
            return null;

        string json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<PresetData>(json);
    }

    public static string[] GetAllPresets()
    {
        if (!Directory.Exists(PresetsFolder))
            return [];

        return Directory.GetFiles(PresetsFolder, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .ToArray();
    }

    //public static void DeletePreset(string name)
    //{
    //    string path = GetPresetPath(name);
    //    if (File.Exists(path))
    //        File.Delete(path);
    //}

    private static string GetPresetPath(string name)
    {
        return Path.Combine(PresetsFolder, $"{name}.json");
    }
}