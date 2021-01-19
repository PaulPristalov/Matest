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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Matest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int exCounter; // Current number of example
        private int rightAnswers; // Right answers counter
        private int timer = 0;
        private int numberOfExamples; // Count of examples (all examples)

        private Example ex; // Current example
        private DispatcherTimer dt; // Timer

        public MainWindow()
        {
            InitializeComponent();

            Restart();
        }

        private void DtTick(object sender, EventArgs e)
        {
            timer++;
            TimerText.Text = timer.ToString();
        }

        private void Answer_KeyDown(object sender, KeyEventArgs e)
        {
            // Check input
            if (e.Key == Key.Enter)
            {
                CheckAnswer(Answer.Text.ToString());
                Answer.Text = "";
            }
        }

        private void Restart_Button_Click(object sender, RoutedEventArgs e)
        {
            dt.Stop();
            Restart();
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.Show(); // Open setting window
            Hide(); // Close this window
        }

        /// <summary>
        /// Set new example to textblock
        /// </summary>
        public void SetNewExample()
        {
            ex = ExampleGenerator.NewExample();
            ExampleText.Text = ex.ExampleStr + " = ";

            // Increment examle counter
            exCounter++;
            ExampleCounterText.Text = $"{exCounter} / {Settings.IntSettings["ExamplesCount"]}";
        }

        public void CheckAnswer(string answ)
        {
            // If user has entered not a number
            if (!double.TryParse(answ, out double a))
            {
                MessageBox.Show("Incorrect input!");
                return;
            }

            // If answer is wrong
            if (a != ex.Answer)
                MessageBox.Show("Wrong answer! Answer is " + ex.Answer);
            else
                rightAnswers++;

            // If examples has ended
            if (exCounter == Settings.IntSettings["ExamplesCount"])
            {
                dt.Stop();
                MessageBox.Show($"Your result: {rightAnswers} / {numberOfExamples}\n" +
                    $"Time: {timer} seconds");
                Restart();
            }
            else
                SetNewExample();
        }

        public void Restart()
        {
            numberOfExamples = Settings.IntSettings["ExamplesCount"];

            exCounter = 0;
            rightAnswers = 0;
            timer = 0;

            // Set timer
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += DtTick;
            dt.Start();
            // Set timer text to 0
            TimerText.Text = timer.ToString();

            SetNewExample();
        }
    }
}
