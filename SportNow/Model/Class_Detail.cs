using System;
namespace SportNow.Model
{
    public class Class_Detail
    {
        public string id { get; set; }
        public string member_name { get; set; }
        public string dojo { get; set; }
        public string name { get; set; }
        public string day { get; set; }
        public string begin_time { get; set; }
        public string end_time { get; set; }
        public string begin_date { get; set; }
        public string end_date { get; set; }
        public string imagesource { get; set; }
        public object imagesourceObject { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
