using System;
using Xamarin.Forms;

namespace SportNow.Model.Charts
{
    public class Competition_Result
    {
        public string classificacao { get; set; }
        public int count { get; set; }

        public Competition_Result(string classificacao)
        {
            this.classificacao = classificacao;
            count = 0;
        }
    }


}
