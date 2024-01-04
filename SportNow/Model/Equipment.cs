using System;
namespace SportNow.Model
{
    public class Equipment
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public double value { get; set; }
        public string valueFormatted { get; set; }
        

        public Equipment()
        {
        }
    }
}
