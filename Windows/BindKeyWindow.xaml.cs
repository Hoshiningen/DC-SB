using System.Windows;
using System.Collections.Generic;
using DC_SB.Utils;
using System.Collections.ObjectModel;
using System;

namespace DC_SB.Windows
{
    /// <summary>
    /// Interaction logic for BindKeyWindow.xaml
    /// </summary>
    public partial class BindKeyWindow : Window
    {
        public ObservableCollection<Input.VKeys> Result { get; private set; }

        public BindKeyWindow()
        {
            InitializeComponent();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Result = new ObservableCollection<Input.VKeys>();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Key_Up(Input.VKeys key, List<Input.VKeys> pressedKeys)
        {
            Result = new ObservableCollection<Input.VKeys>(pressedKeys) { key };
            Dispatcher.BeginInvoke(new Action(() => Close()));
        }
    }
}
