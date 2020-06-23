using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace File_Iterator
{
    public class IteratorViewModel : PropChanged
    {
        public IteratorViewModel() {

            FinalizeFileCommand = new RelayCommand(finalizeFile);
            SearchLocationCommand = new RelayCommand(searchLocation);
        }

        public bool IsTrue => true;
       
        private void searchLocation(object obj)
        {
            Directory = new DirectoryInfo(Location);

            if (!Directory.Exists)
                return;

            var subDirectories = Directory.GetDirectories()
                               .Where(d => (d.Attributes & FileAttributes.ReparsePoint) == 0
                                      && (d.Attributes & FileAttributes.System) == 0);
            var options = new EnumerationOptions()
            {
                RecurseSubdirectories = true,
                IgnoreInaccessible = true
            };


            List<FileInfo> files = new List<FileInfo>();

            foreach (var d in subDirectories)
                files.AddRange(d.GetFiles("*.*", options).Where(s => s.Extension.EndsWith(".cs") || s.Extension.EndsWith(".xaml")));

            files.ForEach(generateEntry);

            TotalExtensions = (from f in files select f.Extension).Distinct().Count();

            TotalFiles = Files.Count;
        }

      

        private void finalizeFile(object obj)
        {
            if (!(obj is FileHolder holder))
                return;

            Files.Remove(holder);

            if (holder.IsDirty)
                BadFiles.Add(holder);
            else
                GoodFiles.Add(holder);
        }
        
        public string Location
        {
            get => _location;
            set { _location = value; OnPropChanged(); }
        }
        private string _location = @"C:\Users\fmanv\source\repos\BinarDataGenerator";


        private DirectoryInfo Directory { get; set; }

        private int _totalFiles;
        public int TotalFiles
        {
            get { return _totalFiles; }
            set { _totalFiles = value; OnPropChanged(); }
        }

        private int _totalExtensions;
        public int TotalExtensions
        {
            get { return _totalExtensions; }
            set { _totalExtensions = value; OnPropChanged(); }
        }

        private int _dirtyPercent = 30;
        public int DirtyPercent
        {
            get { return _dirtyPercent; }
            set { _dirtyPercent = value; OnPropChanged(); }
        }

        private int _dirtyCount = 789;
        public int DirtyCount
        {
            get { return _dirtyCount; }
            set { _dirtyCount = value; OnPropChanged(); }
        }

        public ObservableCollection<FileHolder> Files { get; set; } = new ObservableCollection<FileHolder>();
        public ObservableCollection<FileHolder> GoodFiles { get; set; } = new ObservableCollection<FileHolder>();
        public ObservableCollection<FileHolder> BadFiles { get; set; } = new ObservableCollection<FileHolder>();

        public RelayCommand FinalizeFileCommand { get; }
        public RelayCommand SearchLocationCommand { get; }

        public FileHolder SelectedFile { get => _selectedFile; set { _selectedFile = value; OnPropChanged();  } }
        private FileHolder _selectedFile;

        public LineHolder SelectedLine { get => _selectedLine; set { _selectedLine = value; OnPropChanged(); } }
        private LineHolder _selectedLine;
      
        private void generateEntry(FileInfo obj)
        {
            var temp = new FileHolder(obj);
            if (temp.Valid)
                Files.Add(temp);
        }
    }

   

    public class PropChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
