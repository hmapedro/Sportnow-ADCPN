using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SportNow.Model
{
    public class Examination_Result
    {
        public string id { get; set; }
        public string name { get; set; }
        public string membername { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string grade { get; set; }
        public string evaluation { get; set; }
        public string result { get; set; }
        public string video { get; set; }

        public List<Examination_Technic_Result> examination_technics_result;
    }
}
