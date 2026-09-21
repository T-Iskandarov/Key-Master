using System.Text.Json;
using System.IO;

namespace QisqaTugma.Models;

public class HotkeyItem
{
    public string KeyCombo { get; set; } = "";
    public string Description { get; set; } = "";
}

public class HotkeyCategory
{
    public string Name { get; set; } = "";
    public List<HotkeyItem> Hotkeys { get; set; } = new();
}

public class HotkeyDictionaryData
{
    public List<HotkeyCategory> Categories { get; set; } = new();
}
