using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SportNow.Model
{
    public class Examination : INotifyPropertyChanged
    {
        public string id { get; set; }
        public string typeLabel { get; set; }
        public string grade { get; set; }
        public string gradeLabel { get; set; }
        public string date { get; set; }
        public string place { get; set; }
        public string examiner { get; set; }
        public string number { get; set; }
        public string image { get; set; }
        public string estado { get; set; }
        public Color estadoTextColor { get; set; } = Color.White;
        public string membername { get; set; }
        public int memberage { get; set; }
        public bool selected { get; set; }
        public Color SelectedColor { get; set; }
        public Color selectedColor
        {
            get
            {
                return SelectedColor;
            }
            set
            {
                if (SelectedColor != value)
                {
                    SelectedColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return grade;
        }
    }
}
