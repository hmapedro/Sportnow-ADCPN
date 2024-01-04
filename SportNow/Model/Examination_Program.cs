using System;
using System.Collections.Generic;

namespace SportNow.Model
{
    public class Examination_Program
    {
        public string id { get; set; }
        public string name { get; set; }
        public string grade { get; set; }
        public string type { get; set; }
        public string examinationTo_string { get; set; }
        public string image { get; set; }
        public string video { get; set; }

        public List<Examination_Technique> examination_techniques;

        public string kihonText { get; set; }
        public string kataText { get; set; }
        public string kumiteText { get; set; }
        public string shiaikumiteText { get; set; }
        public bool isExpanded { get; set; } = false;
    }
}
