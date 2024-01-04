using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SportNow.Model
{
    public class MonthFee: INotifyPropertyChanged
    {
        public MonthFee() { }

        public string id { get; set; }
        public string name { get; set; }
        public string membernickname { get; set; }
        public string classname { get; set; }
        public string status { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string division { get; set; }
        public string value { get; set; }
        public string dojo { get; set; }

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
                return name;
            }
        }
}
