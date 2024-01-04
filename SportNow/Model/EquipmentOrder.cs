using System;
namespace SportNow.Model
{
    public class EquipmentOrder
    {
        public string id { get; set; }
        public string name { get; set; }
        public string estado { get; set; }
        public string data { get; set; }
        public string memberid { get; set; }
        public string equipamentid { get; set; }
        public string paymentid { get; set; }
        public string paymentorderid { get; set; }
        public string entidade { get; set; }
        public string referencia_mb { get; set; }
        public double valor { get; set; }

        public EquipmentOrder()
        {
        }
    }
}
