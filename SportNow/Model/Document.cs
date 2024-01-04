using System;
using Xamarin.Forms;

namespace SportNow.Model
{
    public class Document
    {
        public string id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string active_date { get; set; }
        public string expiry_date { get; set; }
        public string type { get; set; }
        public string typeString { get; set; }
        public string imagesource { get; set; }
        public object imagesourceObject { get; set; }
        public object statusimage { get; set; }
        					
    public override string ToString()
        {
            return name;
        }
    }
}
