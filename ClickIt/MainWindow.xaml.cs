using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClickIt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, int> ClickCount = new Dictionary<string, int>();
        string clickPath;
        public MainWindow()
        {
            InitializeComponent();
            var buttons = LoadButtonsFromSettings();
            AddButtonsToPanel(buttons);
            clickPath = Properties.Settings.Default.ClickPath;
        }

        private void AddButtonsToPanel(List<ButtonSetting> buttonsSettings)
        {
            foreach (var buttonSetting in buttonsSettings)
            {
                ClickCount[buttonSetting.Name] = 0;
                var button = new Button();
                button.Name = buttonSetting.Name;
                button.Content = button.Name;
                button.Width = buttonSetting.Width;
                button.Margin = new Thickness(20);
                var color = (Color)ColorConverter.ConvertFromString(buttonSetting.Color);
                button.Background = new SolidColorBrush(color);
                button.BorderThickness = new Thickness(5);
                button.FontFamily = new FontFamily(buttonSetting.FontName);
                button.FontSize = buttonSetting.FontSize;
                button.Click += this.btnTest_Click;
                stackPanelButtons.Children.Add(button);
            }
        }


        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            ClickCount[button.Name]++;

            var filename = Path.Combine(clickPath, "Click_" + button.Name + ".txt");
            using (var file = File.CreateText(filename))
            {
                file.WriteLine(ClickCount[button.Name]);
            }
        }

        private List<ButtonSetting> LoadButtonsFromSettings()
        {
            string json = File.ReadAllText("ButtonsSetting.json");
            return JsonConvert.DeserializeObject<List<ButtonSetting>>(json);
        }
    }
}
