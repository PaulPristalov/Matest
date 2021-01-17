using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Matest
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
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

                            // If all active examle is off
                            bool isExamplesActive = Settings.BoolSettings["activePlus"] ||
                                Settings.BoolSettings["activeMinus"] ||
                                Settings.BoolSettings["activeMulti"] ||
                                Settings.BoolSettings["activeDivide"] ||
                                Settings.BoolSettings["activeSqr"] ||
                                Settings.BoolSettings["activeSqrt"];

                            if (!isExamplesActive)
                            {
                                MessageBox.Show("You must to active at least one kind of examples");
                                // By default active plus
                                Settings.BoolSettings["activePlus"] = true;
                                return;
                            }
                        }
                    }
                }
            }

            Settings.Save();
            MessageBox.Show("Settigs successfuly saved");
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
            int value = 0;
            success = true;

            // If input is a number
            if (int.TryParse(tb.Text, out value))
            {
                // If sqrt value below 0
                if (tb.Name.Contains("Sqrt") && value < 0)
                {
                    MessageBox.Show("Root can't be below 0");
                    success = false;
                }

                // If it is max value and it below than min value
                if (tb.Name.Contains("max") &&
                    value < Settings.IntSettings["min" + tb.Name.Substring(3)])
                {
                    MessageBox.Show("Max value can't be below than min");
                    success = false;
                }

                // If example count below or equals 0
                if (tb.Name == "ExamplesCount" && value <= 0)
                {
                    MessageBox.Show("Number of examples can't be below or equals 0");
                    success = false;
                }
            }
            else
            {
                MessageBox.Show("Incorrect input");
                success = false;
            }

            return value;
        }
    }
}
