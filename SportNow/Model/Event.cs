using System;
using Xamarin.Forms;

namespace SportNow.Model
{
    public class Event
    {
        public string id { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string detailed_date { get; set; }
        public string place { get; set; }
        public string type { get; set; }
        public double value { get; set; }
        public string website { get; set; }
        public string imagemNome { get; set; }
        public string imagemSource { get; set; }
        public string registrationbegindate { get; set; }
        public string registrationlimitdate { get; set; }
        public string participationid { get; set; }
        public string participationconfirmed { get; set; }
        public string participationimage { get; set; }
        public string category { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
