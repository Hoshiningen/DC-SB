using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace DC_SB.Utils
{
    public class Sound : INotifyPropertyChanged
    {
        public List<string> FileNames { get; private set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value == name) return;
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private int volume;
        public int Volume
        {
            get { return volume; }
            set
            {
                if (value == volume) return;
                volume = value;
                OnPropertyChanged("Volume");
            }
        }
        private bool loop;
        public bool Loop
        {
            get { return loop; }
            set
            {
                loop = value;
                OnPropertyChanged("Loop");
            }
        }

        private List<string> filePaths;
        public List<string> FilePaths
        {
            get { return filePaths; }
            set
            {
                filePaths = value;
                FileNames.Clear();
                foreach (string filePath in filePaths)
                {
                    FileNames.Add(Path.GetFileName(filePath));
                    if (!IniFile.Portable && !File.Exists(filePath)) ErrorHandler.Raise("File {0} could not be found.", filePath);
                }
                OnPropertyChanged("FilePaths");
                OnPropertyChanged("FileNames");
            }
        }
        private ObservableCollection<Input.VKeys> keys;
        public ObservableCollection<Input.VKeys> Keys
        {
            get { return keys; }
            set
            {
                keys = value;
                OnPropertyChanged("Keys");
            }
        }

        public Sound()
        {
            FileNames = new List<string>();
            filePaths = new List<string>();
            keys = new ObservableCollection<Input.VKeys>();
            Volume = 50;
        }

        public Sound(Sound sound)
        {
            name = sound.Name;
            volume = sound.Volume;
            loop = sound.Loop;
            FileNames = new List<string>();
            filePaths = new List<string>(sound.FilePaths);
            keys = new ObservableCollection<Input.VKeys>(sound.Keys);
        }

        public Sound(string name, int volume, bool loop, List<string> filePaths, ObservableCollection<Input.VKeys> keys)
        {
            FileNames = new List<string>();
            FilePaths = filePaths;
            Keys = keys;
            Name = name;
            Volume = volume;
            Loop = loop;
            FilePaths = filePaths;
        }

        public bool IsValid()
        {
            if (Name == null || Name.Trim() == "") return false;
            if (Keys.Count == 0) return false;
            if (FilePaths.Count == 0) return false;
            foreach (string filePath in FilePaths)
            {
                if (!File.Exists(filePath)) return false;
            }
            return true;
        }

        public void ChooseFile()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Title = "Choose sound file/s";
            fileDialog.Filter = "sound files (*.wma; *.mp3; *.wav; *.ogg; *.m4a)|*.wma;*.mp3;*.wav;*.ogg;*.m4a";
            fileDialog.AddExtension = true;
            fileDialog.RestoreDirectory = true;
            fileDialog.Multiselect = true;
            bool? result = fileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                var newList = new List<string>();
                foreach (string filePath in fileDialog.FileNames)
                {
                    newList.Add(filePath);
                }
                FilePaths = newList;
            }
        }

        public void BindKey(Input input, Window owner)
        {
            var bindKeyDialog = new Windows.BindKeyWindow();
            bindKeyDialog.Owner = owner;

            input.KeyUp += bindKeyDialog.Key_Up;

            bindKeyDialog.ShowDialog();
            bindKeyDialog.Close();

            input.KeyUp -= bindKeyDialog.Key_Up;

            if (bindKeyDialog.Result != null)
            {
                Keys = bindKeyDialog.Result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
