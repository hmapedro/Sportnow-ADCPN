using System;
namespace SportNow.Model
{
    public class Award
    {
        public string id { get; set; }
        public string name { get; set; }
        public string tipo { get; set; }
        public string ano { get; set; }
        public string mes { get; set; }
        public string imagem { get; set; }

        public override string ToString()
        {
            return name;
        }

    }
}
