using System;
namespace SportNow.Model
{
    public class Fee
    {
        public string id { get; set; }
        public string name { get; set; }
        public string tipo { get; set; }
        public string tipo_desc { get; set; }
        public string imagem { get; set; }
        public string periodo { get; set; }
        public string estado { get; set; }
        public string entidade { get; set; }
        public string referencia { get; set; }
        public double valor { get; set; }

        public override string ToString()
        {
            return name;
        }

    }
}
