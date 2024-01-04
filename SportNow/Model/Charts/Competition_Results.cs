using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace SportNow.Model.Charts
{
    public class Competition_Results
    {

        public List<Competition_Result> Data { get; set; }


        public Competition_Results(List<Competition_Participation> competition_participations)
        {
			Debug.Print("AQUI " + competition_participations.Count);
			Data = new List<Competition_Result>();
			Data.Add(new Competition_Result("1"));
			Data.Add(new Competition_Result("2"));
			Data.Add(new Competition_Result("3"));
			Data.Add(new Competition_Result("P"));
			foreach (Competition_Participation competition_participation in competition_participations)
			{
				if (competition_participation.classificacao == "1")
				{
					Data[0].count++;
				}
				else if (competition_participation.classificacao == "2")
				{
					Data[1].count++;

				}
				else if (competition_participation.classificacao == "3")
				{
					Data[2].count++;

				}
				else
				{
					Data[3].count++;
				}
			}
			Debug.Print("AQUII competition_results[0].count = " + Data[0].count);
			this.Print();
			//return competition_results;
		}

		public void Print()
		{
			foreach (Competition_Result competition_result in Data)
			{
				Debug.Print("AQUII2 "+competition_result.classificacao + " " + competition_result.count);
			}

			
		}
    }


}
