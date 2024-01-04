using System;
using Xamarin.Forms;

namespace SportNow.Model.Charts
{
    public class Attendance_Stat
    {
        public string type { get; set; }
        public string name { get; set; }
        public double class_count_presente { get; set; }
        public double class_count_ausente { get; set; }
        public double class_count_total { get; set; }
        public double attendance_percentage { get; set; }

        /*public Attendance_Stat(string type)
        {
            this.type = type;
            this.name = "";
            this.class_count_presente = 0;
            this.class_count_ausente = 0;
            this.class_count_total = 0;
            this.attendance_percentage = 0;
        }*/

        public Attendance_Stat(string name)
        {
            this.name = name;
            this.type = "";
            this.class_count_presente = 0;
            this.class_count_ausente = 0;
            this.class_count_total = 0;
            this.attendance_percentage = 0;
        }
    }


}
