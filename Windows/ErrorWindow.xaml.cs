using System;
using System.Windows;

namespace DC_SB.Windows
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ShowError(string message)
        {
            if (!Text.Text.Contains(message)) Text.Text += message + Environment.NewLine;
            if (!IsActive) Show();
        }

        public void ShowErrorDialog(string message)
        {
            if (!Text.Text.Contains(message)) Text.Text += message + Environment.NewLine;
            if (!IsActive) ShowDialog();
        }
    }
}
