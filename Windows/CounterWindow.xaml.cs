using DC_SB.Utils;
using System.Windows;
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
            if (counter.IsValid() && long.TryParse(Count.Text, out count))
            {
                Result = counter;
                Close();
            }
            else
            {
                Storyboard flash = FindResource("Flash") as Storyboard;
                if (flash != null)
                {
                    if (counter.Name == null || counter.Name.Trim() == "")
                    {
                        NameField.BeginStoryboard(flash);
                    }
                    if (counter.FileName == Counter.DEFAULT_FILENAME)
                    {
                        FileLabel.BeginStoryboard(flash);
                    }
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
