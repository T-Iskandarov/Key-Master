using System.Windows.Input;

namespace QisqaTugma.Models;

public class KeyStroke
{
    public Key Key { get; set; }
    public ModifierKeys Modifiers { get; set; }
    public string DisplayText { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public bool IsModifierOnly { get; set; }
    public int VkCode { get; set; }
}
