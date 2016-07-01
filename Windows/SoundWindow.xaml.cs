using DC_SB.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DC_SB.Windows
{
    /// <summary>
    /// Interaction logic for SoundWindow.xaml
    /// </summary>
    public partial class SoundWindow : Window
    {
        public Sound Result { get; private set; }
        private Sound sound;
        private Input input;

        public SoundWindow(Input input)
        {
            InitializeComponent();
            this.input = input;
            sound = new Sound();
            DataContext = sound;
        }

        public SoundWindow(Input input, Sound sound)
        {
            InitializeComponent();
            this.input = input;
            this.sound = sound;
            DataContext = sound;
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            sound.ChooseFile();
        }

        private void BindKey_Click(object sender, RoutedEventArgs e)
        {
            sound.BindKey(input, this);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (sound.IsValid())
            {
                Result = sound;
                Close();
            }
            else
            {
                Storyboard flash = FindResource("Flash") as Storyboard;
                if (flash != null)
                {
                    List<TextBox> toFlash = new List<TextBox>();
                    if (sound.Name == null || sound.Name.Trim() == "") toFlash.Add(NameField);
                    if (sound.FileNames.Count == 0) toFlash.Add(FileLabel);
                    if (sound.Keys.Count == 0) toFlash.Add(KeyField);
                    if (toFlash.Count == 0) toFlash.Add(FileLabel);
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
