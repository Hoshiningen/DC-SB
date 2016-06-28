using System.Windows;
using System.Windows.Controls;

namespace DC_SB.Windows
{
    /// <summary>
    /// Interaction logic for ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        public string Message
        {
            get { return (string)Label.Content; }
            set { Label.Content = value; }
        }
        public bool? Result { get; private set; }

        public ConfirmWindow(string message)
        {
            InitializeComponent();
            Message = message;
        }

        public ConfirmWindow(string message, string title)
        {
            InitializeComponent();
            Message = message;
            Title = title;
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            Close();
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            Close();
        }
    }
}
