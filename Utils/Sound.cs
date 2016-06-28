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
        public const string DEFAULT_FILENAME = "No File Chosen";
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
                    if (!File.Exists(filePath)) ErrorHandler.Raise("File {0} could not be found.", filePath);
                    FileNames.Add(Path.GetFileName(filePath));
                }
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
            FilePaths = new List<string>();
            Keys = new ObservableCollection<Input.VKeys>();
            FileNames.Add(DEFAULT_FILENAME);
        }

        public Sound(Sound sound)
        {
            Name = sound.Name;
            FileNames = new List<string>();
            FilePaths = new List<string>(sound.FilePaths);
            Keys = new ObservableCollection<Input.VKeys>(sound.Keys);
        }

        public Sound(string name, List<string> filePaths, ObservableCollection<Input.VKeys> keys)
        {
            FileNames = new List<string>();
            FilePaths = filePaths;
            Keys = keys;
            Name = name;
            FilePaths = filePaths;
        }

        public bool IsValid()
        {
            if (Name == null || Name.Trim() == "") return false;
            if (Keys.Count == 0) return false;
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
            fileDialog.Filter = "sound files (*.wma; *.mp3; *.wav)|*.wma;*.mp3;*.wav";
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
