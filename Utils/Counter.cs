using Microsoft.Win32;
using System.ComponentModel;
using System.IO;

namespace DC_SB.Utils
{
    public class Counter : INotifyPropertyChanged
    {
        public string FileName { get; private set; }
        public bool ValidFile { get; private set; }

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

        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                FileName = Path.GetFileName(value);
                filePath = value;
                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    long number;
                    if (content == "")
                    {
                        ValidFile = true;
                        Count = 0;
                        
                    }
                    else if (long.TryParse(content, out number))
                    {
                        ValidFile = true;
                        Count = number;
                    }
                    else
                    {
                        ValidFile = false;
                        ErrorHandler.Raise("File {0} of counter {1} has unexpected content.", filePath, Name);
                    }
                }
                else
                {
                    ValidFile = false;
                    ErrorHandler.Raise("File {0} of counter {1} could not be found.", filePath, Name);
                }
                OnPropertyChanged("FileName");
                OnPropertyChanged("FilePath");
                OnPropertyChanged("ValidFile");
            }
        }

        private long count;
        public long Count
        {
            get { return count; }
            set
            {
                if (File.Exists(FilePath))
                {
                    File.WriteAllText(FilePath, value.ToString());
                    count = value;
                    OnPropertyChanged("Count");
                }
                else
                {
                    ErrorHandler.Raise("File {0} of counter {1} could not be found.", FilePath, Name);
                }
            }
        }

        public Counter()
        {
        }

        public Counter(Counter counter)
        {
            name = counter.Name;
            filePath = counter.FilePath;
            count = counter.Count;
            ValidFile = counter.ValidFile;
        }

        public Counter(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }

        public bool IsValid()
        {
            if (Name == null || Name.Trim() == "") return false;
            if (!File.Exists(FilePath)) return false;
            return true;
        }

        public void ChooseFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Choose counter file";
            fileDialog.Filter = "txt files|*.txt";
            fileDialog.AddExtension = true;
            fileDialog.RestoreDirectory = true;
            fileDialog.Multiselect = false;
            bool? result = fileDialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                FilePath = fileDialog.FileName;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
