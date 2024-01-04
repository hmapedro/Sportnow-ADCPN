using System;
namespace SportNow.Model
{
    public class Competition
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
        public string participationcategory { get; set; }
        public string participationclassification { get; set; }
        public string participationconfirmed { get; set; }
        public string participationimage { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
