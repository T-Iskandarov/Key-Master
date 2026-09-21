using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text;
using QisqaTugma.Models;
using QisqaTugma.Helpers;

namespace QisqaTugma.Services;

public static class DictionaryService
{
    private static string GetSettingsPath(string lang)
    {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QisqaTugma");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return Path.Combine(folder, $"HotkeyDictionary_{lang}.json");
    }

    public static HotkeyDictionaryData Load()
    {
        string lang = LocalizationService.CurrentLanguage ?? "uz";
        string path = GetSettingsPath(lang);

        if (File.Exists(path))
        {
            try
            {
                var json = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<HotkeyDictionaryData>(json);
                if (data != null && data.Categories.Count > 0)
                    return data;
            }
            catch { }
        }

        var defaultData = GetDefaultData(lang);
        Save(defaultData);
        return defaultData;
    }

    public static void Save(HotkeyDictionaryData data)
    {
        string lang = LocalizationService.CurrentLanguage ?? "uz";
        string path = GetSettingsPath(lang);
        try
        {
            string folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(path, json);
        }
        catch { }
    }

    private static string GetCatName(string catType, string lang)
    {
        if (lang == "uz")
        {
            if (catType == "System") return "Tizim";
            if (catType == "Office") return "Office dasturlari";
            if (catType == "Browser") return "Brauzerlar";
        }
        else if (lang == "kaa")
        {
            if (catType == "System") return "Sistema";
            if (catType == "Office") return "Office dástúrleri";
            if (catType == "Browser") return "Brauzerler";
        }
        else if (lang == "ru")
        {
            if (catType == "System") return "Система";
            if (catType == "Office") return "Программы Office";
            if (catType == "Browser") return "Браузеры";
        }
        else if (lang == "en")
        {
            if (catType == "System") return "System";
            if (catType == "Office") return "Office programs";
            if (catType == "Browser") return "Browsers";
        }
        return catType;
    }

    public static HotkeyDictionaryData GetDefaultData(string lang)
    {
        var data = new HotkeyDictionaryData();

        var systemCat = new HotkeyCategory { Name = GetCatName("System", lang) };
        var officeCat = new HotkeyCategory { Name = GetCatName("Office", lang) };
        var browserCat = new HotkeyCategory { Name = GetCatName("Browser", lang) };

        if (lang == "uz")
        {
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Alt + Tab", Description = "Ochiq dasturlar va oynalar orasida tez almashish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Tab", Description = "Task View - ochiq oynalar va virtual stollarni ko'rish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + C", Description = "Nusxa olish (Copy)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + V", Description = "Joylash (Paste)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Z", Description = "Oxirgi harakatni bekor qilish (Undo)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + D", Description = "Barcha oynalarni yig'ib, ish stolini ko'rsatish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + E", Description = "Fayl boshqaruvchisini (File Explorer) ochish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + L", Description = "Kompyuterni qulflash" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + V", Description = "Vaqtinchalik xotira (Clipboard) tarixini ochish" });
            
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + B", Description = "Qalin (Bold) qilish" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + I", Description = "Yotiq (Italic) qilish" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + U", Description = "Tagiga chizish (Underline)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Matnni chap qirg'oqqa tekislash" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + E", Description = "Matnni markazga tekislash" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + S", Description = "Hujjatni saqlash" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + P", Description = "Chop etish (Print)" });

            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + T", Description = "Yangi oyna (Tab) ochish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + W", Description = "Joriy oynani (Tab) yopish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Sahifani yangilash (Refresh)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + T", Description = "Yopilgan oynani qayta ochish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Yuklanmalar (Downloads)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + H", Description = "Tarixni (History) ochish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + D", Description = "Xatcho'pga (Bookmark) qo'shish" });
        }
        else if (lang == "kaa")
        {
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Alt + Tab", Description = "Ashıq dástúrler hám aynalar arasında tez almasıw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Tab", Description = "Task View - ashıq aynalar hám virtual stollardı kóriw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + C", Description = "Nusqa alıw (Copy)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + V", Description = "Qoyıw (Paste)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Z", Description = "Aqırǵı háreketti biykarlaw (Undo)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + D", Description = "Barlıq aymalardı jıynap, is stolin kórsetiw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + E", Description = "Fayl basqarıwshısın (File Explorer) ashıw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + L", Description = "Kompyuterdi qulıplaw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + V", Description = "Waqtınshalıq eslep qalıw (Clipboard) tariyxın ashıw" });
            
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + B", Description = "Qalıń (Bold) qılıw" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + I", Description = "Jantayıq (Italic) qılıw" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + U", Description = "Astiń sizıw (Underline)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Tekstti shep qırǵaqqa tegislew" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + E", Description = "Tekstti orayǵa tegislew" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + S", Description = "Hújjettti saqlaw" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + P", Description = "Baspadan shıǵarıw (Print)" });

            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + T", Description = "Taza ayna (Tab) ashıw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + W", Description = "Ayma (Tab) jabıw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Bettti jańalaw (Refresh)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + T", Description = "Jabılg'an aynanı qayta ashıw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Júklenbeler (Downloads)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + H", Description = "Tariyxtı (History) ashıw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + D", Description = "Bანიm (Bookmark) qosıw" });
        }
        else if (lang == "ru")
        {
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Alt + Tab", Description = "Быстрое переключение между окнами" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Tab", Description = "Просмотр задач (Task View)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + C", Description = "Копировать (Copy)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + V", Description = "Вставить (Paste)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Z", Description = "Отменить действие (Undo)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + D", Description = "Свернуть все и показать рабочий стол" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + E", Description = "Открыть Проводник (File Explorer)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + L", Description = "Заблокировать компьютер" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + V", Description = "Журнал буфера обмена (Clipboard)" });
            
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + B", Description = "Жирный текст (Bold)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + I", Description = "Курсив (Italic)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + U", Description = "Подчеркнутый (Underline)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Выровнять по левому краю" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + E", Description = "Выровнять по центру" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + S", Description = "Сохранить документ" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + P", Description = "Печать (Print)" });

            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + T", Description = "Новая вкладка (Tab)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + W", Description = "Закрыть текущую вкладку" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Обновить страницу (Refresh)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + T", Description = "Открыть закрытую вкладку" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Загрузки (Downloads)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + H", Description = "История (History)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + D", Description = "Добавить в закладки" });
        }
        else // en
        {
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Alt + Tab", Description = "Switch between open apps" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Tab", Description = "Task View - see virtual desktops and open windows" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + C", Description = "Copy" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + V", Description = "Paste" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Z", Description = "Undo action" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + D", Description = "Show desktop" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + E", Description = "Open File Explorer" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + L", Description = "Lock computer" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + V", Description = "Open clipboard history" });
            
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + B", Description = "Bold" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + I", Description = "Italic" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + U", Description = "Underline" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Align left" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + E", Description = "Align center" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + S", Description = "Save document" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + P", Description = "Print" });

            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + T", Description = "Open new tab" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + W", Description = "Close current tab" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Refresh page" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + T", Description = "Reopen closed tab" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Open Downloads" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + H", Description = "Open History" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + D", Description = "Bookmark current page" });
        }

        data.Categories.Add(systemCat);
        data.Categories.Add(officeCat);
        data.Categories.Add(browserCat);

        return data;
    }

    public static string GetActiveCategoryName()
    {
        string lang = LocalizationService.CurrentLanguage ?? "uz";
        string sysName = GetCatName("System", lang);
        string browserName = GetCatName("Browser", lang);
        string officeName = GetCatName("Office", lang);

        IntPtr handle = NativeMethods.GetForegroundWindow();
        if (handle == IntPtr.Zero) return sysName;

        const int nChars = 256;
        var sb = new StringBuilder(nChars);
        if (NativeMethods.GetWindowText(handle, sb, nChars) > 0)
        {
            string title = sb.ToString().ToLower();

            if (title.Contains("chrome") || title.Contains("edge") || title.Contains("firefox") || 
                title.Contains("opera") || title.Contains("brave") || title.Contains("safari") || title.Contains("browser"))
                return browserName;

            if (title.Contains("word") || title.Contains("excel") || title.Contains("powerpoint") ||
                title.Contains("wps") || title.Contains("document") || title.Contains("docs") || title.Contains("sheet"))
                return officeName;
        }
        
        return sysName;
    }

    public static string GetDescriptionForKey(string keyCombo, HotkeyDictionaryData data)
    {
        string categoryName = GetActiveCategoryName();
        var category = data.Categories.FirstOrDefault(c => c.Name == categoryName);
        
        if (category != null)
        {
            var match = category.Hotkeys.FirstOrDefault(h => h.KeyCombo.Equals(keyCombo, StringComparison.OrdinalIgnoreCase));
            if (match != null) return match.Description;
        }

        string lang = LocalizationService.CurrentLanguage ?? "uz";
        string sysName = GetCatName("System", lang);
        
        if (categoryName != sysName)
        {
            var sysCat = data.Categories.FirstOrDefault(c => c.Name == sysName);
            if (sysCat != null)
            {
                var match = sysCat.Hotkeys.FirstOrDefault(h => h.KeyCombo.Equals(keyCombo, StringComparison.OrdinalIgnoreCase));
                if (match != null) return match.Description;
            }
        }

        return "";
    }
}
