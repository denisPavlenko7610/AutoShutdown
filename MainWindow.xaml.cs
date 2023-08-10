using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows.Threading;


namespace ShutdownApp
{
    public partial class MainWindow : Window
    {
        private int remainingSeconds = 120; // 2 minutes
        private bool isCountdownPaused = false;
        private DispatcherTimer countdownTimer;

        public MainWindow()
        {
            InitializeComponent();
            SetTitleBarColor();
            CenterWindowOnScreen();
            InitializeCountdownTimer();
            StartCountdown();
        }

        private static void SetTitleBarColor()
        {

        }

        private void InitializeCountdownTimer()
        {
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;

            Left = (screenWidth - windowWidth) / 2;
            Top = (screenHeight - windowHeight) / 2;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                UpdateCountdownDisplay();
            }
            else
            {
                countdownTimer.Stop();
                ShutdownComputer();
            }
        }


        private void UpdateCountdownDisplay()
        {
            int hours = remainingSeconds / 3600;
            int minutes = (remainingSeconds % 3600) / 60;
            int seconds = remainingSeconds % 60;

            if (hours > 0)
            {
                countdownText.Text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
            }
            else
            {
                countdownText.Text = $"{minutes:D2}:{seconds:D2}";
            }
        }

        private void StartCountdown()
        {
            countdownTimer.Start();
            UpdateCountdownDisplay();
        }

        private void ShutdownComputer()
        {
            string shutdownCommand = "/s /f /t 0";
            Process.Start("shutdown", shutdownCommand);
        }

        private void ToggleCountdownPause()
        {
            if (!isCountdownPaused)
            {
                countdownTimer.Stop();
                stopShutdownButton.Content = "Resume";
            }
            else
            {
                countdownTimer.Start();
                stopShutdownButton.Content = "Pause";
            }

            isCountdownPaused = !isCountdownPaused;
        }

        private void stopShutdownButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleCountdownPause();
        }

        private void StopShutdown()
        {
            countdownTimer.Stop();
            stopShutdownButton.IsEnabled = false;
            add5MinButton.IsEnabled = true;
        }

        private void add5MinButton_Click(object sender, RoutedEventArgs e)
        {
            Add5Minutes();
        }

        private void Add5Minutes()
        {
            remainingSeconds += 5 * 60;
            UpdateCountdownDisplay();
        }
    }
}
