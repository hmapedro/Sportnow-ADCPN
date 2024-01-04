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
    public class ScheduleCollection //: INotifyPropertyChanged
    {

        //public ObservableCollection<Class_Attendance> items { get; set; }
        public ObservableCollection<Class_Schedule> Items { get; set; }
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

        public ScheduleCollection()
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