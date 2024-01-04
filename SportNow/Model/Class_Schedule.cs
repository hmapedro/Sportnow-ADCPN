using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SportNow.Model
{
    public class Class_Schedule : INotifyPropertyChanged
    {
        public string classid { get; set; }
        public string dojo { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string date { get; set; }
        public string begintime { get; set; }
        public string endtime { get; set; }
        public string classfullname { get; set; }
        public string datestring { get; set; }
        public string imagesource { get; set; }
        public object imagesourceObject { get; set; }
        public string Participationimage { get; set; }
        public string participationimage
        {
            get
            {
                return Participationimage;
            }
            set
            {
                if (Participationimage != value)
                {
                    Participationimage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string classattendanceid { get; set; }
        public string classattendancestatus { get; set; }

        public override string ToString()
        {
            return name;
        }

        public Class_Schedule (string classid, string dojo, string name, string date, string datestring, string imagesource, string participationimage)
        {
            this.classid = classid;
            this.dojo = dojo;
            this.name = name;
            this.date = date;
            this.datestring = datestring;
            this.imagesource = imagesource;
            this.participationimage = participationimage;
            this.classfullname = this.name + " \n " + this.dojo;
            this.classattendanceid = "";
            this.classattendancestatus = "";
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
