using System;
namespace SportNow.Model
{
    public class Belt
    {
        public string gradecode { get; set; }
        public string grade { get; set; }
        public string image { get; set; }
        public bool hasgrade { get; set; } = false;

        public Belt()
        {
        }
    }
}
