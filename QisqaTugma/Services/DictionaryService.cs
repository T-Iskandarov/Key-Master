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
    private static readonly int DataVersion = 2; // Increment when defaults change

    private static string GetSettingsPath(string lang)
    {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QisqaTugma");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return Path.Combine(folder, $"HotkeyDictionary_{lang}.json");
    }

    private static string GetVersionPath()
    {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QisqaTugma");
        return Path.Combine(folder, "dict_version.txt");
    }

    public static HotkeyDictionaryData Load()
    {
        string lang = LocalizationService.CurrentLanguage ?? "uz";
        string path = GetSettingsPath(lang);
        string versionPath = GetVersionPath();

        // Check if we need to regenerate defaults (version changed)
        int savedVersion = 0;
        if (File.Exists(versionPath))
        {
            int.TryParse(File.ReadAllText(versionPath).Trim(), out savedVersion);
        }

        if (savedVersion < DataVersion)
        {
            // Delete old cached files for all languages
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "QisqaTugma");
            foreach (var f in Directory.GetFiles(folder, "HotkeyDictionary_*.json"))
            {
                try { File.Delete(f); } catch { }
            }
            File.WriteAllText(versionPath, DataVersion.ToString());
        }

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
            // === TIZIM ===
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Shift + S", Description = "Ekranni belgilab screenshot qilish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "PrtSc", Description = "Ekranni screenshot qilish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Alt + Tab", Description = "Ochiq dasturlar va oynalar orasida tez almashish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Tab", Description = "Task View - ochiq oynalar va virtual stollarni ko'rish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + N", Description = "Yangi papka (Folder) yaratish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + C", Description = "Nusxa olish (Copy)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + V", Description = "Joylash (Paste)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + X", Description = "Qirqib olish (Cut)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Z", Description = "Oxirgi harakatni bekor qilish (Undo)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Y", Description = "Bekor qilingan harakatni qaytarish (Redo)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + D", Description = "Barcha oynalarni yig'ib, ish stolini ko'rsatish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + E", Description = "Fayl boshqaruvchisini (File Explorer) ochish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + L", Description = "Kompyuterni qulflash" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + R", Description = "\"Run\" (Bajarish) oynasini ochish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + Esc", Description = "Vazifalar menejerini (Task Manager) ochish" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + V", Description = "Vaqtinchalik xotira (Clipboard) tarixini ochish" });

            // === OFFICE ===
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + B", Description = "Matnni qalinlashtirish (Bold)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + I", Description = "Matnni qiyalashtirish (Italic)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + U", Description = "Matn ostiga chizish (Underline)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Matnni chap qirg'oqqa tekislash (Align Left)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + E", Description = "Matnni markazga tekislash (Center)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Matnni o'ng qirg'oqqa tekislash (Align Right)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Matnni har ikki chetga tekislash (Justify)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + X", Description = "Tanlangan qismni qirqib olish (Cut)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + S", Description = "Hujjatni saqlash (Save)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + N", Description = "Yangi hujjat yaratish (New)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + P", Description = "Chop etish (Print)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + F", Description = "So'z yoki matn qidirish (Find)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + A", Description = "Barcha matnni belgilash (Select All)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + K", Description = "Matnga havola (Link) qo'shish" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Enter", Description = "Yangi sahifaga o'tish (Page Break)" });

            // === BRAUZERLAR ===
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + T", Description = "Yangi oyna (Tab) ochish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + W", Description = "Joriy oynani (Tab) yopish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Q", Description = "Brauzerni butunlay yopish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + T", Description = "Yopilib ketgan oxirgi oynani qayta ochish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Manzil (URL) qatorini belgilash" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Sahifani yangilash (Refresh)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + H", Description = "Kirishlar tarixini (History) ko'rish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Yuklab olingan fayllar ro'yxatini (Downloads) ochish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + D", Description = "Joriy sahifani xatcho'plarga (Bookmarks) saqlash" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Tab", Description = "Ochiq oynalar (Tab) orasida o'ngga o'tish" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + Delete", Description = "Tarix, kesh va kukilarni tozalash" });
        }
        else if (lang == "kaa")
        {
            // === SISTEMA ===
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Shift + S", Description = "Ekrandı belgílep screenshot alıw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "PrtSc", Description = "Ekrannıń screenshot'ın alıw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Alt + Tab", Description = "Ashıq dástúrler hám aynalar arasında tez almasıw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Tab", Description = "Task View - ashıq aynalar hám virtual stollardı kóriw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + N", Description = "Taza papka (Folder) jaratıw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + C", Description = "Nusqa alıw (Copy)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + V", Description = "Qoyıw (Paste)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + X", Description = "Qıyıp alıw (Cut)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Z", Description = "Aqırǵı háreketti biykarlaw (Undo)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Y", Description = "Biykarlanǵan háreketti qaytarıw (Redo)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + D", Description = "Barlıq aynalardı jıynap, is stolin kórsetiw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + E", Description = "Fayl basqarıwshısın (File Explorer) ashıw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + L", Description = "Kompyuterdi qulıplaw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + R", Description = "\"Run\" (Orınlaw) aynasın ashıw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + Esc", Description = "Wazıypalar menejerın (Task Manager) ashıw" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + V", Description = "Waqtınshalıq xotira (Clipboard) tariyxın ashıw" });

            // === OFFICE ===
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + B", Description = "Tekstti qalınlastırıw (Bold)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + I", Description = "Tekstti qıyalastırıw (Italic)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + U", Description = "Tekst astına sızıw (Underline)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Tekstti shep qırǵaqqa tegislew (Align Left)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + E", Description = "Tekstti orayǵa tegislew (Center)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Tekstti oń qırǵaqqa tegislew (Align Right)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Tekstti eki shetke tegislew (Justify)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + X", Description = "Tańlanǵan bólimdi qıyıp alıw (Cut)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + S", Description = "Hújjetti saqlaw (Save)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + N", Description = "Taza hújjet jaratıw (New)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + P", Description = "Baspadan shıǵarıw (Print)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + F", Description = "Sóz yamasa tekst izlew (Find)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + A", Description = "Barlıq tekstti belgílew (Select All)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + K", Description = "Tekstke silteme (Link) qosıw" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Enter", Description = "Taza betke ótiw (Page Break)" });

            // === BRAUZERLER ===
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + T", Description = "Taza ayna (Tab) ashıw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + W", Description = "Járiy aynanı (Tab) jabıw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Q", Description = "Brauzerdı tolıq jabıw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + T", Description = "Jabılıp ketken aqırǵı aynanı qayta ashıw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Mánzil (URL) qatarın belgílew" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Betti jańalaw (Refresh)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + H", Description = "Kiriw tariyxın (History) kóriw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Júklengen fayllar dizimin (Downloads) ashıw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + D", Description = "Járiy betti betbelgilerge (Bookmarks) saqlaw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Tab", Description = "Ashıq aynalar (Tab) arasında óńge ótiw" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + Delete", Description = "Tariyxtı, keshtı hám kukilerdí tazalaw" });
        }
        else if (lang == "ru")
        {
            // === СИСТЕМА ===
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Shift + S", Description = "Выделить область и сделать скриншот" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "PrtSc", Description = "Сделать скриншот экрана" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Alt + Tab", Description = "Быстрое переключение между окнами" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Tab", Description = "Просмотр задач (Task View) и виртуальных столов" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + N", Description = "Создать новую папку" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + C", Description = "Копировать (Copy)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + V", Description = "Вставить (Paste)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + X", Description = "Вырезать (Cut)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Z", Description = "Отменить действие (Undo)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Y", Description = "Повторить действие (Redo)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + D", Description = "Свернуть все окна и показать рабочий стол" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + E", Description = "Открыть Проводник (File Explorer)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + L", Description = "Заблокировать компьютер" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + R", Description = "Открыть окно \"Выполнить\" (Run)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + Esc", Description = "Открыть Диспетчер задач (Task Manager)" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + V", Description = "Журнал буфера обмена (Clipboard)" });

            // === OFFICE ===
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + B", Description = "Жирный текст (Bold)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + I", Description = "Курсив (Italic)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + U", Description = "Подчёркнутый (Underline)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Выровнять по левому краю (Align Left)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + E", Description = "Выровнять по центру (Center)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Выровнять по правому краю (Align Right)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Выровнять по ширине (Justify)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + X", Description = "Вырезать выделенное (Cut)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + S", Description = "Сохранить документ (Save)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + N", Description = "Создать новый документ (New)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + P", Description = "Печать (Print)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + F", Description = "Поиск текста (Find)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + A", Description = "Выделить всё (Select All)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + K", Description = "Вставить ссылку (Link)" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Enter", Description = "Перейти на новую страницу (Page Break)" });

            // === БРАУЗЕРЫ ===
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + T", Description = "Новая вкладка (Tab)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + W", Description = "Закрыть текущую вкладку" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Q", Description = "Закрыть браузер полностью" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + T", Description = "Открыть последнюю закрытую вкладку" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Выделить адресную строку (URL)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Обновить страницу (Refresh)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + H", Description = "Просмотр истории (History)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Список загрузок (Downloads)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + D", Description = "Добавить в закладки (Bookmarks)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Tab", Description = "Переключение между вкладками вправо" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + Delete", Description = "Очистить историю, кэш и куки" });
        }
        else // en
        {
            // === SYSTEM ===
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Shift + S", Description = "Select area and take screenshot" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "PrtSc", Description = "Take a screenshot of the screen" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Alt + Tab", Description = "Switch between open apps and windows" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + Tab", Description = "Task View - see virtual desktops and open windows" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + N", Description = "Create new folder" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + C", Description = "Copy" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + V", Description = "Paste" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + X", Description = "Cut" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Z", Description = "Undo action" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Y", Description = "Redo action" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + D", Description = "Show desktop" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + E", Description = "Open File Explorer" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + L", Description = "Lock computer" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + R", Description = "Open Run dialog" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + Esc", Description = "Open Task Manager" });
            systemCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Win + V", Description = "Open clipboard history" });

            // === OFFICE ===
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + B", Description = "Bold text" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + I", Description = "Italic text" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + U", Description = "Underline text" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Align text left" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + E", Description = "Align text center" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Align text right" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Justify text" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + X", Description = "Cut selected text" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + S", Description = "Save document" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + N", Description = "Create new document" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + P", Description = "Print" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + F", Description = "Find text" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + A", Description = "Select all" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + K", Description = "Insert link" });
            officeCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Enter", Description = "Insert page break" });

            // === BROWSERS ===
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + T", Description = "Open new tab" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + W", Description = "Close current tab" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Q", Description = "Close browser completely" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + T", Description = "Reopen last closed tab" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + L", Description = "Select address bar (URL)" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + R", Description = "Refresh page" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + H", Description = "View history" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + J", Description = "Open downloads list" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + D", Description = "Bookmark current page" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Tab", Description = "Switch to next tab" });
            browserCat.Hotkeys.Add(new HotkeyItem { KeyCombo = "Ctrl + Shift + Delete", Description = "Clear history, cache and cookies" });
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
