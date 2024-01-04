using System;
using Xamarin.Forms;

namespace SportNow.Model
{
    public class Dojo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string morada { get; set; }
        public string estado { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
