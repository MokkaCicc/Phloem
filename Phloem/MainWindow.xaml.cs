using Phloem.ViewModels;
using System;
using System.Windows.Threading;
using System.Windows;
using Microsoft.Win32;

namespace Phloem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// This is the ViewModel of this view. It is only used to set the DataContext
        /// </summary>
        private readonly GameViewModel GameViewModel = new();

        /// <summary>
        /// This timer will jump to the next generation at a given interval.
        /// </summary>
        // TODO[baptiste]: Timer belongs to the view or viewmodel? Both seems
        // acceptable.
        private readonly DispatcherTimer Timer = new();

        /// <summary>
        /// Gets if the game is paused or not.
        /// </summary>
        private bool IsPaused = true;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = GameViewModel;
            Timer.Interval = TimeSpan.FromMilliseconds((int)IntervalSlider.Value);
            Timer.Tick += TimerTick;
        }

        /// <summary>
        /// Method called by the timer at a given interval.
        /// </summary>
        private void TimerTick(object? sender, EventArgs e)
        {
            // TODO[baptiste]: Can I pass a timer events to the ViewModel with
            // an ICommand?
            GameViewModel.Next(null!);
        }

        /// <summary>
        /// Method called by the import button.
        /// </summary>
        private void ImportClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog browser = new()
            {
                FileName = "Select a text file",
                Filter = "Text files (*.txt)|*.txt",
                Title = "Open text file"
            };
            if (browser.ShowDialog() == true)
            {
                bool error = GameViewModel.Import(browser.FileName);
                if (error)
                {
                    MessageBox.Show("The config file is incorrect", "Error", MessageBoxButton.OK);
                }
            }
        }

        /// <summary>
        /// Method called by the reset button.
        /// </summary>
        private void ResetClick(object sender, RoutedEventArgs e)
        {
            if (!IsPaused)
            {
                Pause();
            }
        }

        /// <summary>
        /// Method called by the pause button.
        /// </summary>
        private void PauseClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        /// <summary>
        /// Method called by the next button.
        /// </summary>
        private void NextClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsPaused)
            {
                Pause();
            }
        }

        /// <summary>
        /// Method called by the next10 button.
        /// </summary>
        private void Next10Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsPaused)
            {
                Pause();
            }
        }

        /// <summary>
        /// Pause the game.
        /// </summary>
        private void Pause()
        {
            Timer.Stop();
            PauseButton.Content = "Play";
            IsPaused = true;
        }

        /// <summary>
        /// Resume the game.
        /// </summary>
        private void Resume()
        {
            Timer.Start();
            PauseButton.Content = "Pause";
            IsPaused = false;
        }

        /// <summary>
        /// method called by the slider.
        /// </summary>
        private void SliderChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            var interval = (int)IntervalSlider.Value;
            GameViewModel.IntervalNumber = interval;
            Timer.Interval = TimeSpan.FromMilliseconds(interval);
        }
    }
}
