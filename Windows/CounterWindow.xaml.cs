using DC_SB.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DC_SB.Windows
{
    /// <summary>
    /// Interaction logic for CounterWindow.xaml
    /// </summary>
    public partial class CounterWindow : Window
    {
        public Counter Result { get; private set; }
        private Counter counter;

        public CounterWindow()
        {
            InitializeComponent();
            counter = new Counter();
            DataContext = counter;
        }

        public CounterWindow(Counter counter)
        {
            InitializeComponent();
            this.counter = counter;
            DataContext = counter;
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            counter.ChooseFile();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            long count;
            int increment;
            if (counter.IsValid() && long.TryParse(Count.Text, out count) && int.TryParse(Increment.Text, out increment))
            {
                Result = counter;
                Close();
            }
            else
            {
                Storyboard flash = FindResource("Flash") as Storyboard;
                if (flash != null)
                {
                    List<TextBox> toFlash = new List<TextBox>();
                    if (counter.Name == null || counter.Name.Trim() == "") toFlash.Add(NameField);
                    if (counter.FilePath == null || counter.FilePath == "") toFlash.Add(FileLabel);
                    foreach (TextBox textBox in toFlash) textBox.BeginStoryboard(flash);
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
