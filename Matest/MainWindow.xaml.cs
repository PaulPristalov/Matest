using System;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Input;
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

        private CultureInfo culture = CultureInfo.CreateSpecificCulture("en-us");
        private ResourceManager resMan = new ResourceManager(
            "Matest.Localization.Language", typeof(MainWindow).Assembly);

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
                MessageBox.Show(resMan.GetString("IncorrectInput", culture));
                return;
            }

            // If answer is wrong
            if (a != ex.Answer)
                MessageBox.Show(resMan.GetString("WrongAnswer", culture) + ex.Answer);
            else
                rightAnswers++;

            // If examples has ended
            if (exCounter == Settings.IntSettings["ExamplesCount"])
            {
                dt.Stop();
                MessageBox.Show($"{resMan.GetString("Result", culture)}: {rightAnswers} / {numberOfExamples}\n" +
                    $"{resMan.GetString("Time", culture)}: {timer} s");
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
