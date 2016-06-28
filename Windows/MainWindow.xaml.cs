using System.Windows;
using DC_SB.Utils;
using DC_SB.Windows;
using System.Windows.Interop;
using System;
using AutoUpdaterDotNET;

namespace DC_SB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Settings settings;
        private Logic logic;
        private Input input;

        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AutoUpdater.Start("http://kalejin.eu/dc+sb/dc+sb.xml");
            ErrorHandler.Owner = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            settings = new Settings();
            DataContext = settings;
            logic = new Logic(settings);

            input = new Input();
            input.Install(new WindowInteropHelper(this).Handle);
            logic.RegisterEvents(input);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(settings, input);
            settingsWindow.Owner = this;
            logic.Pause();
            logic.UnregisterEvents(input);
            settingsWindow.ShowDialog();
            logic.RegisterEvents(input);
            if (settingsWindow.DialogResult.HasValue && settingsWindow.DialogResult.Value)
            {
                settings = settingsWindow.NewSettings;
                DataContext = settings;
                logic.Settings = settings;
                settings.Save();
            }
        }

        private void AddCounter_Click(object sender, RoutedEventArgs e)
        {
            var counterWindow = new CounterWindow();
            counterWindow.Owner = this;
            logic.UnregisterEvents(input);
            counterWindow.ShowDialog();
            logic.RegisterEvents(input);
            if (counterWindow.Result != null)
            {
                settings.Counters.Add(counterWindow.Result);
                settings.Save();
            }
        }

        private void RemoveCounter_Click(object sender, RoutedEventArgs e)
        {
            var counter = Counters.SelectedItem as Counter;
            if (counter != null)
            {
                var deleteDialog = new ConfirmWindow(string.Format("Do you really want to remove counter {0}?", counter.Name), "Delete counter");
                deleteDialog.Owner = this;
                deleteDialog.ShowDialog();
                if (deleteDialog.Result.HasValue && deleteDialog.Result.Value)
                {
                    settings.Counters.Remove(counter);
                    settings.Save();
                }
            }
        }

        private void Counters_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = ((System.Windows.Controls.DataGridRow)sender).GetIndex();
            var counter = new Counter(settings.Counters[index]);
            var counterWindow = new CounterWindow(counter);
            counterWindow.Owner = this;
            logic.UnregisterEvents(input);
            counterWindow.ShowDialog();
            logic.RegisterEvents(input);
            if (counterWindow.Result != null)
            {
                settings.Counters[index] = counterWindow.Result;
                settings.Save();
            }
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            var counter = Counters.SelectedItem as Counter;
            if (counter != null)
            {
                counter.Count--;
            }
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            var counter = Counters.SelectedItem as Counter;
            if (counter != null)
            {
                counter.Count++;
            }
        }

        private void AddSound_Click(object sender, RoutedEventArgs e)
        {
            var soundWindow = new SoundWindow(input);
            soundWindow.Owner = this;
            logic.UnregisterEvents(input);
            soundWindow.ShowDialog();
            logic.RegisterEvents(input);
            if (soundWindow.Result != null)
            {
                settings.Sounds.Add(soundWindow.Result);
                settings.Save();
            }
        }

        private void RemoveSound_Click(object sender, RoutedEventArgs e)
        {
            var sound = Sounds.SelectedItem as Sound;
            if (sound != null)
            {
                var deleteDialog = new ConfirmWindow(string.Format("Do you really want to remove sound {0}?", sound.Name), "Delete sound");
                deleteDialog.Owner = this;
                deleteDialog.ShowDialog();
                if (deleteDialog.Result.HasValue && deleteDialog.Result.Value)
                {
                    settings.Sounds.Remove(sound);
                    settings.Save();
                }
            }
        }

        private void Sounds_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int index = ((System.Windows.Controls.DataGridRow)sender).GetIndex();
            var sound = new Sound(settings.Sounds[index]);
            var soundWindow = new SoundWindow(input, sound);
            soundWindow.Owner = this;
            logic.UnregisterEvents(input);
            soundWindow.ShowDialog();
            logic.RegisterEvents(input);
            if (soundWindow.Result != null)
            {
                settings.Sounds[index] = soundWindow.Result;
                settings.Save();
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            var sound = Sounds.SelectedItem as Sound;
            if (sound != null)
            {
                logic.Play(sound);
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            logic.Pause();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            settings.Save();
        }

        private void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex == null) return;
            ErrorHandler.RaiseDialog(ex.ToString());
            Environment.Exit(-1);
        }
    }
}
