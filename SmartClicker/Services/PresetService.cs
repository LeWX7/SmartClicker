using SmartClicker.Models;
using System.Text.Json;

public class PresetService
{
    private static string PresetDirectory =>
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Presets_for_SmartClicker");

    public PresetService()
    {
        if (!Directory.Exists(PresetDirectory))
            Directory.CreateDirectory(PresetDirectory);
    }

    public string[] GetAllPresetNames()
    {
        return Directory.GetFiles(PresetDirectory, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .ToArray();
    }

    public static async Task SavePresetAsync(PresetData preset, string fileName)
    {
        var json = JsonSerializer.Serialize(preset, new JsonSerializerOptions { WriteIndented = true });
        var filePath = Path.Combine(PresetDirectory, fileName);
        await File.WriteAllTextAsync(filePath, json);
    }

    public static async Task<PresetData> LoadPresetAsync(string name)
    {
        var filePath = Path.Combine(PresetDirectory, name + ".json");
        if (!File.Exists(filePath))
            return new PresetData();

        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<PresetData>(json) ?? new PresetData();
    }
}