using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Text;

namespace File_Iterator
{
    public class FileHolder : PropChanged
    {
        public FileInfo File { get; }

        public ObservableCollection<LineHolder> Lines { get; private set; }

        public FileHolder(FileInfo obj)
        {
            if (obj.FullName.Contains(".i.") ||
                obj.FullName.Contains(".g."))
                return;

            File = obj;
            Valid = true;
            readLines();
        }

        private void readLines()
        {
            Lines = new ObservableCollection<LineHolder>();
            var content = System.IO.File.ReadAllLines(File.FullName);

            for (int i = 0; i < content.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(content[i]))
                    continue;
                Lines.Add(new LineHolder(this, i, content[i]));
            }
        }

        public bool Valid = false;

        private string _comment;

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; OnPropChanged(); }
        }


        public bool IsDirty => Lines.FirstOrDefault(x => x.Bad) != null;
    }

    public class LineHolder : PropChanged
    {
        public LineHolder(FileHolder fileHolder, int i, string content)
        {
            Owner = fileHolder;
            Index = i;
            Content = content;

            CommandSetBad = new RelayCommand(setBad);
            CommandSetGood = new RelayCommand(setGood);
        }
        internal FileHolder Owner;
        public int Index { get; }
        public string Content { get; set; }

        private bool _badLine;
        public bool Bad
        {
            get { return _badLine; }
            set { _badLine = value; OnPropChanged(); }
        }
        public RelayCommand CommandSetGood { get; }
        private void setGood(object obj)
        {
            Owner.Lines.Remove(this);
        }


        public RelayCommand CommandSetBad { get; }

        private void setBad(object obj)
        {
            Bad = !Bad;
        }

        public override string ToString()
        {
            return Content;
        }
    }
}
