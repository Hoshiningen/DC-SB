using System.Windows;
using System.Windows.Controls;
using DC_SB.Utils;

namespace DC_SB.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private Settings oldSettings;
        private Settings newSettings;
        public Settings NewSettings
        {
            get { return newSettings; }
        }
        private Input input;

        public SettingsWindow(Settings settings, Input input)
        {
            InitializeComponent();

            oldSettings = settings;
            newSettings = new Settings(oldSettings);
            DataContext = newSettings;
            this.input = input;
        }

        private void KeyBinding_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var bindKeyDialog = new BindKeyWindow();
            bindKeyDialog.Owner = this;

            input.KeyUp += bindKeyDialog.Key_Up;

            bindKeyDialog.ShowDialog();
            bindKeyDialog.Close();

            input.KeyUp -= bindKeyDialog.Key_Up;

            if (bindKeyDialog.Result != null)
            {
                DataGrid dataGrid = ((DataGrid)e.Source);
                ((KeyPrompt)dataGrid.SelectedItem).Keys = bindKeyDialog.Result;
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            Save_Settings();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (oldSettings != newSettings)
            {
                var saveDialog = new ConfirmWindow("Changes were not saved. Save changes?", "Save changes?");
                saveDialog.Owner = this;
                saveDialog.ShowDialog();
                if (!saveDialog.Result.HasValue)
                {
                    e.Cancel = true;
                }
                else if (saveDialog.Result.Value)
                {
                    Save_Settings();
                }
            }
        }

        private void Save_Settings()
        {
            oldSettings = newSettings;
            DialogResult = true;
            //implement real saving
        }
    }
}
