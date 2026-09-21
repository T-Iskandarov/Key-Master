using System.Windows;
using System.Windows.Controls;
using QisqaTugma.Models;
using QisqaTugma.Services;

namespace QisqaTugma.Views;

public partial class HotkeyDictionaryWindow : Window
{
    private HotkeyDictionaryData _data = new();
    
    public HotkeyDictionaryWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _data = DictionaryService.Load();
        UpdateTexts();
        RefreshTabs();
    }

    private void UpdateTexts()
    {
        Title = LocalizationService.GetString("Dict_Title");
        BtnAddTab.Content = LocalizationService.GetString("Dict_AddTab");
        BtnDeleteTab.Content = LocalizationService.GetString("Dict_DeleteTab");
        BtnAddRow.Content = LocalizationService.GetString("Dict_AddRow");
        // "O'chirish" isn't explicitly Dict_DeleteRow in localization, we can use "Button_Close" or similar, but let's just use "➖"
        BtnDeleteRow.Content = "➖ " + LocalizationService.GetString("Dict_DeleteTab").Replace("➖ ", "");
    }

    private void RefreshTabs()
    {
        int selectedIndex = CategoriesTabControl.SelectedIndex;
        CategoriesTabControl.Items.Clear();

        string colHotkey = LocalizationService.GetString("Dict_ColHotkey");
        string colDesc = LocalizationService.GetString("Dict_ColDesc");

        foreach (var category in _data.Categories)
        {
            var tabItem = new TabItem { Header = category.Name, Tag = category };
            
            var dataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                SelectionMode = DataGridSelectionMode.Single,
                HeadersVisibility = DataGridHeadersVisibility.Column
            };

            dataGrid.Columns.Add(new DataGridTextColumn 
            { 
                Header = colHotkey, 
                Binding = new System.Windows.Data.Binding("KeyCombo"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            });
            
            dataGrid.Columns.Add(new DataGridTextColumn 
            { 
                Header = colDesc, 
                Binding = new System.Windows.Data.Binding("Description"),
                Width = new DataGridLength(2, DataGridLengthUnitType.Star)
            });
            
            dataGrid.ItemsSource = category.Hotkeys;
            tabItem.Content = dataGrid;
            CategoriesTabControl.Items.Add(tabItem);
        }

        if (CategoriesTabControl.Items.Count > 0)
        {
            CategoriesTabControl.SelectedIndex = (selectedIndex >= 0 && selectedIndex < CategoriesTabControl.Items.Count) ? selectedIndex : 0;
        }
    }

    private void AddCategory_Click(object sender, RoutedEventArgs e)
    {
        string promptText = LocalizationService.GetString("Dict_PromptTabName");
        string titleText = LocalizationService.GetString("Dict_AddTab").Replace("➕ ", "");
        string name = PromptDialog.Show(promptText, titleText);
        if (!string.IsNullOrWhiteSpace(name))
        {
            if (_data.Categories.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Bu nomdagi bo'lim allaqachon mavjud!", "Xatolik", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _data.Categories.Add(new HotkeyCategory { Name = name });
            SaveData();
            RefreshTabs();
            CategoriesTabControl.SelectedIndex = CategoriesTabControl.Items.Count - 1;
        }
    }

    private void DeleteCategory_Click(object sender, RoutedEventArgs e)
    {
        if (CategoriesTabControl.SelectedItem is TabItem selectedTab && selectedTab.Tag is HotkeyCategory category)
        {
            if (MessageBox.Show($"'{category.Name}' bo'limini o'chirishga ishonchingiz komilmi?", "O'chirish", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _data.Categories.Remove(category);
                SaveData();
                RefreshTabs();
            }
        }
    }

    private void AddHotkey_Click(object sender, RoutedEventArgs e)
    {
        if (CategoriesTabControl.SelectedItem is TabItem selectedTab && selectedTab.Tag is HotkeyCategory category)
        {
            string key = NewKeyCombo.Text.Trim();
            string desc = NewDescription.Text.Trim();

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(desc))
            {
                MessageBox.Show("Tugma va ta'rifni kiriting!", "Xatolik", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (category.Hotkeys.Any(h => h.KeyCombo.Equals(key, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Bu qisqa tugma ushbu bo'limda allaqachon mavjud!", "Xatolik", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            category.Hotkeys.Add(new HotkeyItem { KeyCombo = key, Description = desc });
            NewKeyCombo.Text = "";
            NewDescription.Text = "";
            SaveData();
            RefreshTabs();
        }
    }

    private void DeleteHotkey_Click(object sender, RoutedEventArgs e)
    {
        if (CategoriesTabControl.SelectedItem is TabItem selectedTab && selectedTab.Tag is HotkeyCategory category)
        {
            if (selectedTab.Content is DataGrid grid && grid.SelectedItem is HotkeyItem item)
            {
                category.Hotkeys.Remove(item);
                SaveData();
                RefreshTabs();
            }
            else
            {
                MessageBox.Show("O'chirish uchun ro'yxatdan birorta tugmani tanlang!", "Eslatma", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    private void SaveData()
    {
        DictionaryService.Save(_data);
    }

    private void CategoriesTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Just to prevent DataGrid row edit auto save issues, we explicitly save when closing or adding.
    }
}

public static class PromptDialog
{
    public static string Show(string text, string caption)
    {
        var prompt = new Window
        {
            Width = 400,
            Height = 160,
            Title = caption,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            ResizeMode = ResizeMode.NoResize
        };

        var stack = new StackPanel { Margin = new Thickness(20) };
        stack.Children.Add(new TextBlock { Text = text, Margin = new Thickness(0, 0, 0, 10) });
        var textBox = new TextBox { Margin = new Thickness(0, 0, 0, 15) };
        stack.Children.Add(textBox);

        var btnStack = new StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Right };
        var btnOk = new System.Windows.Controls.Button { Content = "OK", Width = 80, Margin = new Thickness(0, 0, 10, 0), IsDefault = true };
        var btnCancel = new System.Windows.Controls.Button { Content = LocalizationService.GetString("Button_Close"), Width = 80, IsCancel = true };

        btnOk.Click += (sender, e) => { prompt.DialogResult = true; prompt.Close(); };
        btnStack.Children.Add(btnOk);
        btnStack.Children.Add(btnCancel);
        stack.Children.Add(btnStack);

        prompt.Content = stack;
        textBox.Focus();

        return prompt.ShowDialog() == true ? textBox.Text : string.Empty;
    }
}
