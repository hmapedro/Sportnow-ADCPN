using System;
using Xamarin.Forms;

namespace SportNow.Model
{
    public class Competition_Participation
    {
        public string id { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string membername { get; set; }
        public string competicao_id { get; set; }
        public string competicao_name { get; set; }
        public string competicao_data { get; set; }
        public string competicao_detailed_date { get; set; }
        public string competicao_local { get; set; }
        public string competicao_tipo { get; set; }
        public string competicao_website { get; set; }
        public string imagemNome { get; set; }
        public string imagemSource { get; set; }
        public string categoria { get; set; }
        public string estado { get; set; }
        public string classificacao { get; set; }
        public string classificacaoImagem { get; set; }
        public Color classificacaoColor { get; set; }
        public Color estadoTextColor { get; set; } = Color.White;
        public string entidade { get; set; }
        public string referencia { get; set; }
        public double valor { get; set; }

    }


}
