using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SportNow.Model;
using Xamarin.Forms;

namespace SportNow.ViewModel
{
    public class AttendanceCollection //: INotifyPropertyChanged
    {

        //public ObservableCollection<Examination> items { get; set; }
        public ObservableCollection<Class_Attendance> Items { get; set; }
        /*{
            get
            {
                return items;
            }
            set
            {
                Debug.Print("AQUIIIII MUDOU");
                if (items != value)
                {
                    Debug.Print("AQUIIIII MUDOU1");
                    items = value;
                    Debug.Print("AQUIIIII MUDOU2");
                    NotifyPropertyChanged();
                }
            }
        }*/

        public AttendanceCollection()
        {
           // Items = new ObservableCollection<Class_Attendance>();
        }

        

        /*protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            Debug.Print("AQUIIIII MUDOU3");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
 
        }

        public event PropertyChangedEventHandler PropertyChanged;*/
    }
}