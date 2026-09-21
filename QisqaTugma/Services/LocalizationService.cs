using System;
using System.Collections.Generic;

namespace QisqaTugma.Services;

public static class LocalizationService
{
    public static string CurrentLanguage { get; set; } = "uz";
    public static event Action? LanguageChanged;

    private static readonly Dictionary<string, Dictionary<string, string>> Dictionaries = new()
    {
        {
            "uz", new Dictionary<string, string>
            {
                {"Settings_Title", "Key Master - Sozlamalar"},
                {"Tray_Settings", "⚙️ Sozlamalar"},
                {"Tray_Exit", "❌ Chiqish"},
                {"Tab_Appearance", "👁️ Ko'rinish"},
                {"Tab_Features", "🚀 Funksiyalar"},
                {"Label_Language", "Til / Language:"},
                {"Label_TextColor", "Matn rangi:"},
                {"Label_BgColor", "Orqa fon rangi:"},
                {"Label_FontSize", "Shrift o'lchami:"},
                {"Label_Position", "Ekrandagi pozitsiya:"},
                {"Label_Duration", "Ko'rinish davomiyligi:"},
                {"Feature_ShowAll", "Barcha tugmalarni ko'rsatish"},
                {"Feature_OnlyFunctional", "Faqat funksional tugmalar (Ctrl, Alt, Shift, Win, F1-F12...)"},
                {"Feature_VirtualKeyboard", "Virtual klaviatura (Ctrl+F12)"},
                {"Feature_Spotlight", "Ctrl 2x bosish — qoramtir fon (Spotlight)"},
                {"Feature_KeySound", "Tugma ovozi (mexanik klaviatura)"},
                {"Feature_MouseClicks", "Sichqoncha bosilishini ko'rsatish (chap, o'ng, scroll)"},
                {"Feature_ClickAnimation", "Sichqoncha bosish animatsiyasi"},
                {"Feature_HotkeyDesc", "Qisqa tugma ta'riflarini ko'rsatish"},
                {"Button_Database", "Ma'lumotlar bazasi"},
                {"Header_Extra", "⚙️ Qo'shimcha"},
                {"Label_AnimType", "Klik animatsiya turi:"},
                {"Label_ClickColor", "Klik animatsiya rangi:"},
                {"Label_SpotlightRadius", "Spotlight radiusi:"},
                {"Button_Save", "Saqlash"},
                {"Button_Close", "Yopish"},
                {"Pos_TopLeft", "Yuqori-Chap"},
                {"Pos_TopCenter", "Yuqori-Markaz"},
                {"Pos_TopRight", "Yuqori-O'ng"},
                {"Pos_CenterLeft", "Markaz-Chap"},
                {"Pos_Center", "Markaz"},
                {"Pos_CenterRight", "Markaz-O'ng"},
                {"Pos_BottomLeft", "Pastki-Chap"},
                {"Pos_BottomCenter", "Pastki-Markaz"},
                {"Pos_BottomRight", "Pastki-O'ng"},
                {"Anim_Ripple", "To'lqin"},
                {"Anim_Shrink", "Kichrayish"},
                {"Anim_Flash", "Yaltirash"},
                {"Anim_Ring", "Halqa"},
                {"Message_Saved", "Sozlamalar saqlandi!"},
                {"MsgTitle_Info", "Key Master"},
                {"Mouse_Left", "Chap tugma"},
                {"Mouse_Right", "O'ng tugma"},
                {"Mouse_WheelUp", "Scroll yuqoriga"},
                {"Mouse_WheelDown", "Scroll pastga"},
                {"Dict_Title", "Qisqa tugmalar bazasi"},
                {"Dict_AddTab", "➕ Bo'lim qo'shish"},
                {"Dict_DeleteTab", "🗑️ Bo'limni o'chirish"},
                {"Dict_ColHotkey", "Qisqa tugma (Masalan: Ctrl+C)"},
                {"Dict_ColDesc", "Ta'rifi (Vazifasi)"},
                {"Dict_AddRow", "➕ Qator qo'shish"},
                {"Dict_DeleteRow", "🗑️ O'chirish"}
            }
        },
        {
            "kaa", new Dictionary<string, string>
            {
                {"Settings_Title", "Key Master - Sazlawlar"},
                {"Tray_Settings", "⚙️ Sazlawlar"},
                {"Tray_Exit", "❌ Shığıw"},
                {"Tab_Appearance", "👁️ Kórinis"},
                {"Tab_Features", "🚀 Funkciyalar"},
                {"Label_Language", "Til / Language:"},
                {"Label_TextColor", "Tekst reńi:"},
                {"Label_BgColor", "Arqa fon reńi:"},
                {"Label_FontSize", "Shrift ólshemi:"},
                {"Label_Position", "Ekrandaǵı poziciyası:"},
                {"Label_Duration", "Kórinis dawamlılıǵı:"},
                {"Feature_ShowAll", "Barlıq túymelerdi kórsetiw"},
                {"Feature_OnlyFunctional", "Tek funkcional túymeler (Ctrl, Alt, Shift, Win, F1-F12...)"},
                {"Feature_VirtualKeyboard", "Virtual klaviatura (Ctrl+F12)"},
                {"Feature_Spotlight", "Ctrl 2x basıw — qarańǵı fon (Spotlight)"},
                {"Feature_KeySound", "Túyme dawısı (mexanik klaviatura)"},
                {"Feature_MouseClicks", "Tıshqansha basılıwın kórsetiw (shep, oń, scroll)"},
                {"Feature_ClickAnimation", "Tıshqansha basıw animaciyası"},
                {"Feature_HotkeyDesc", "Qısqa túyme táriyplerin kórsetiw"},
                {"Button_Database", "Maǵlıwmatlar bazası"},
                {"Header_Extra", "⚙️ Qosımsha"},
                {"Label_AnimType", "Klik animaciya túri:"},
                {"Label_ClickColor", "Klik animaciya reńi:"},
                {"Label_SpotlightRadius", "Spotlight radiusı:"},
                {"Button_Save", "Saqlaw"},
                {"Button_Close", "Jabıw"},
                {"Pos_TopLeft", "Joqarı-Shep"},
                {"Pos_TopCenter", "Joqarı-Oray"},
                {"Pos_TopRight", "Joqarı-Oń"},
                {"Pos_CenterLeft", "Oray-Shep"},
                {"Pos_Center", "Oray"},
                {"Pos_CenterRight", "Oray-Oń"},
                {"Pos_BottomLeft", "Tómengi-Shep"},
                {"Pos_BottomCenter", "Tómengi-Oray"},
                {"Pos_BottomRight", "Tómengi-Oń"},
                {"Anim_Ripple", "Tolqın"},
                {"Anim_Shrink", "Kishireyiw"},
                {"Anim_Flash", "Jaltıraw"},
                {"Anim_Ring", "Sheńber"},
                {"Message_Saved", "Sazlawlar saqlandı!"},
                {"MsgTitle_Info", "Key Master"},
                {"Mouse_Left", "Shep túyme"},
                {"Mouse_Right", "Oń túyme"},
                {"Mouse_WheelUp", "Scroll joqarıǵa"},
                {"Mouse_WheelDown", "Scroll tómengi"},
                {"Dict_Title", "Qısqa túymeler bazası"},
                {"Dict_AddTab", "➕ Bólim qosıw"},
                {"Dict_DeleteTab", "🗑️ Bólimdi óshiriw"},
                {"Dict_ColHotkey", "Qısqa túyme (Mısalı: Ctrl+C)"},
                {"Dict_ColDesc", "Táriypi (Wazıypası)"},
                {"Dict_AddRow", "➕ Qatar qosıw"},
                {"Dict_DeleteRow", "🗑️ Óshiriw"}
            }
        },
        {
            "en", new Dictionary<string, string>
            {
                {"Settings_Title", "Key Master - Settings"},
                {"Tray_Settings", "⚙️ Settings"},
                {"Tray_Exit", "❌ Exit"},
                {"Tab_Appearance", "👁️ Appearance"},
                {"Tab_Features", "🚀 Features"},
                {"Label_Language", "Language:"},
                {"Label_TextColor", "Text Color:"},
                {"Label_BgColor", "Background Color:"},
                {"Label_FontSize", "Font Size:"},
                {"Label_Position", "On-screen Position:"},
                {"Label_Duration", "Display Duration:"},
                {"Feature_ShowAll", "Show all keystrokes"},
                {"Feature_OnlyFunctional", "Only functional keys (Ctrl, Alt, Shift, Win, F1-F12...)"},
                {"Feature_VirtualKeyboard", "Virtual keyboard (Ctrl+F12)"},
                {"Feature_Spotlight", "Double Ctrl — dark overlay (Spotlight)"},
                {"Feature_KeySound", "Keystroke sound (mechanical keyboard)"},
                {"Feature_MouseClicks", "Show mouse clicks (left, right, scroll)"},
                {"Feature_ClickAnimation", "Mouse click animation"},
                {"Feature_HotkeyDesc", "Show hotkey descriptions"},
                {"Button_Database", "Hotkey Database"},
                {"Header_Extra", "⚙️ Extra"},
                {"Label_AnimType", "Click animation type:"},
                {"Label_ClickColor", "Click animation color:"},
                {"Label_SpotlightRadius", "Spotlight radius:"},
                {"Button_Save", "Save"},
                {"Button_Close", "Close"},
                {"Pos_TopLeft", "Top-Left"},
                {"Pos_TopCenter", "Top-Center"},
                {"Pos_TopRight", "Top-Right"},
                {"Pos_CenterLeft", "Center-Left"},
                {"Pos_Center", "Center"},
                {"Pos_CenterRight", "Center-Right"},
                {"Pos_BottomLeft", "Bottom-Left"},
                {"Pos_BottomCenter", "Bottom-Center"},
                {"Pos_BottomRight", "Bottom-Right"},
                {"Anim_Ripple", "Ripple"},
                {"Anim_Shrink", "Shrink"},
                {"Anim_Flash", "Flash"},
                {"Anim_Ring", "Ring"},
                {"Message_Saved", "Settings saved!"},
                {"MsgTitle_Info", "Key Master"},
                {"Mouse_Left", "Left click"},
                {"Mouse_Right", "Right click"},
                {"Mouse_WheelUp", "Scroll up"},
                {"Mouse_WheelDown", "Scroll down"},
                {"Dict_Title", "Hotkey Database"},
                {"Dict_AddTab", "➕ Add category"},
                {"Dict_DeleteTab", "🗑️ Delete category"},
                {"Dict_ColHotkey", "Hotkey (e.g. Ctrl+C)"},
                {"Dict_ColDesc", "Description (Action)"},
                {"Dict_AddRow", "➕ Add row"},
                {"Dict_DeleteRow", "🗑️ Delete"}
            }
        },
        {
            "ru", new Dictionary<string, string>
            {
                {"Settings_Title", "Key Master - Настройки"},
                {"Tray_Settings", "⚙️ Настройки"},
                {"Tray_Exit", "❌ Выход"},
                {"Tab_Appearance", "👁️ Внешний вид"},
                {"Tab_Features", "🚀 Функции"},
                {"Label_Language", "Язык:"},
                {"Label_TextColor", "Цвет текста:"},
                {"Label_BgColor", "Цвет фона:"},
                {"Label_FontSize", "Размер шрифта:"},
                {"Label_Position", "Позиция на экране:"},
                {"Label_Duration", "Продолжительность:"},
                {"Feature_ShowAll", "Показывать все нажатия"},
                {"Feature_OnlyFunctional", "Только функциональные клавиши (Ctrl, Alt, Shift...)"},
                {"Feature_VirtualKeyboard", "Виртуальная клавиатура (Ctrl+F12)"},
                {"Feature_Spotlight", "Двойной Ctrl — затемнение экрана (Spotlight)"},
                {"Feature_KeySound", "Звук клавиш (механическая клавиатура)"},
                {"Feature_MouseClicks", "Отображать клики мыши (левая, правая, скролл)"},
                {"Feature_ClickAnimation", "Анимация клика мыши"},
                {"Feature_HotkeyDesc", "Показывать описания горячих клавиш"},
                {"Button_Database", "База горячих клавиш"},
                {"Header_Extra", "⚙️ Дополнительно"},
                {"Label_AnimType", "Тип анимации клика:"},
                {"Label_ClickColor", "Цвет анимации клика:"},
                {"Label_SpotlightRadius", "Радиус Spotlight:"},
                {"Button_Save", "Сохранить"},
                {"Button_Close", "Закрыть"},
                {"Pos_TopLeft", "Сверху-Слева"},
                {"Pos_TopCenter", "Сверху-По центру"},
                {"Pos_TopRight", "Сверху-Справа"},
                {"Pos_CenterLeft", "В центре-Слева"},
                {"Pos_Center", "По центру"},
                {"Pos_CenterRight", "В центре-Справа"},
                {"Pos_BottomLeft", "Снизу-Слева"},
                {"Pos_BottomCenter", "Снизу-По центру"},
                {"Pos_BottomRight", "Снизу-Справа"},
                {"Anim_Ripple", "Рябь (Ripple)"},
                {"Anim_Shrink", "Сжатие (Shrink)"},
                {"Anim_Flash", "Вспышка (Flash)"},
                {"Anim_Ring", "Кольцо (Ring)"},
                {"Message_Saved", "Настройки сохранены!"},
                {"MsgTitle_Info", "Key Master"},
                {"Mouse_Left", "Левый клик"},
                {"Mouse_Right", "Правый клик"},
                {"Mouse_WheelUp", "Скролл вверх"},
                {"Mouse_WheelDown", "Скролл вниз"},
                {"Dict_Title", "База горячих клавиш"},
                {"Dict_AddTab", "➕ Добавить вкладку"},
                {"Dict_DeleteTab", "🗑️ Удалить вкладку"},
                {"Dict_ColHotkey", "Клавиши (например: Ctrl+C)"},
                {"Dict_ColDesc", "Описание (Действие)"},
                {"Dict_AddRow", "➕ Добавить строку"},
                {"Dict_DeleteRow", "🗑️ Удалить"}
            }
        }
    };

    public static string GetString(string key)
    {
        if (Dictionaries.TryGetValue(CurrentLanguage, out var dict) && dict.TryGetValue(key, out var value))
        {
            return value;
        }
        
        // Fallback to uzbek
        if (Dictionaries["uz"].TryGetValue(key, out var fallbackValue))
        {
            return fallbackValue;
        }

        return key;
    }

    public static void NotifyLanguageChanged()
    {
        LanguageChanged?.Invoke();
    }
}
