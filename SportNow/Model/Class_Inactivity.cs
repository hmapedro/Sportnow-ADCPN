using System;
namespace SportNow.Model
{
    public class Class_Inactivity
    {
        public string id { get; set; }
        public string name { get; set; }
        public string classid { get; set; }
        public string classname { get; set; }
        public string begin_date { get; set; }
        public string end_date { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
