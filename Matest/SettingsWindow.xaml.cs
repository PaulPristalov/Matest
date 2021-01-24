using System;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Controls;

namespace Matest
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private static CultureInfo culture = CultureInfo.CreateSpecificCulture("en-us");
        private static ResourceManager resMan = new ResourceManager(
            "Matest.Localization.Language", typeof(SettingsWindow).Assembly);

        public SettingsWindow()
        {
            InitializeComponent();

            SetSettings();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            // Find all text- and checkboxes
            foreach (TabItem item in SettingsFields.Items)
            {
                var stackPanel = (StackPanel)item.Content;

                foreach (Grid grid in stackPanel.Children)
                {
                    foreach (var el in grid.Children)
                    {
                        // Get value, check it and set to dictionaries
                        if (el.GetType() == typeof(TextBox))
                        {
                            var textbox = (TextBox)el;
                            int res = Check(textbox, out bool success);

                            // If it is incorrcet input
                            if (!success)
                                return;

                            Settings.IntSettings[textbox.Name] = res;
                        }
                        else if (el.GetType() == typeof(CheckBox))
                        {
                            var checkbox = (CheckBox)el;

                            Settings.BoolSettings[checkbox.Name] = (bool)checkbox.IsChecked;
                        }
                    }
                }
            }

            // If all active examle is off
            bool isExamplesActive = Settings.BoolSettings["activePlus"] ||
                Settings.BoolSettings["activeMinus"] ||
                Settings.BoolSettings["activeMulti"] ||
                Settings.BoolSettings["activeDivide"] ||
                Settings.BoolSettings["activeSqr"] ||
                Settings.BoolSettings["activeSqrt"];

            if (!isExamplesActive)
            {
                MessageBox.Show(resMan.GetString("ActiveExamples", culture));
                // By default active plus
                Settings.BoolSettings["activePlus"] = true;
                return;
            }

            Settings.Save();
            MessageBox.Show(resMan.GetString("Saved", culture));
        }

        /// <summary>
        /// Set settings to the settings fields
        /// </summary>
        private void SetSettings()
        {
            // Find all text- and checkboxes
            foreach (TabItem item in SettingsFields.Items)
            {
                var stackPanel = (StackPanel)item.Content;

                foreach (Grid grid in stackPanel.Children)
                {
                    foreach (var el in grid.Children)
                    {
                        // Set value in them
                        if (el.GetType() == typeof(TextBox))
                        {
                            var textbox = (TextBox)el;
                            textbox.Text = (Settings.IntSettings[textbox.Name]).ToString();
                        }
                        else if (el.GetType() == typeof(CheckBox))
                        {
                            var checkbox = (CheckBox)el;
                            checkbox.IsChecked = Settings.BoolSettings[checkbox.Name];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check settings input
        /// </summary>
        /// <param name="tb"> Current textbox </param>
        /// <param name="success"> If checking successful </param>
        /// <returns></returns>
        private static int Check(TextBox tb, out bool success)
        {
            success = true;

            // If input is a number
            if (int.TryParse(tb.Text, out int value))
            {
                // If sqrt value below 0
                if (tb.Name.Contains("Sqrt") && value < 0)
                {
                    MessageBox.Show(resMan.GetString("RootBelow0", culture));
                    success = false;
                }

                // If it is max value and it below than min value
                if (tb.Name.Contains("max") &&
                    value < Settings.IntSettings["min" + tb.Name.Substring(3)])
                {
                    MessageBox.Show(resMan.GetString("MaxMin", culture));
                    success = false;
                }

                // If example count below or equals 0
                if (tb.Name == "ExamplesCount" && value <= 0)
                {
                    MessageBox.Show(resMan.GetString("NumOfExamplesBelow0", culture));
                    success = false;
                }
            }
            else
            {
                MessageBox.Show(resMan.GetString("IncorrectInput", culture));
                success = false;
            }

            return value;
        }
    }
}
