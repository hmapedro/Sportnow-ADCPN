using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;

namespace SportNow.Services.Data.JSON
{
	public class ExaminationManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Examination_Program> examination_programs { get; private set; }
		public List<Examination_Technique> examination_techniques { get; private set; }


		public ExaminationManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<List<Examination_Program>> GetExaminationProgramAll()
		{
			Debug.Print("GetExaminationProgramAll");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Examination_Program_All, string.Empty));
			try { 
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					examination_programs = JsonConvert.DeserializeObject<List<Examination_Program>>(content);


				}

				for (int i = 0; i < examination_programs.Count; i++)
				{
					examination_programs[i].examination_techniques = await this.GetExaminationProgram_Techniques(examination_programs[i].id);
					//Debug.Print("examination_programs[i].examination_techniques.Count="+examination_programs[i].examination_techniques.Count);


					examination_programs[i] = createTextsExaminationPrograms(examination_programs[i]);
				}


				/*			foreach (Examination_Program examination_program in examination_programs) {
								examination_program.examination_techniques = await this.GetExaminationProgram_Techniques(examination_program.id);

								createTextsExaminationPrograms(examination_program);
							}*/

				Debug.Print("examination_programs[0].kihonText=" + examination_programs[0].kihonText);
				return examination_programs;
				}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<List<Examination_Technique>> GetExaminationProgram_Techniques(string examination_programid)
		{
			//Debug.Print("GetExaminationProgram_Techniques");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Examination_Program_Techniques + "?examinationprogramid=" + examination_programid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					examination_techniques = JsonConvert.DeserializeObject<List<Examination_Technique>>(content);
				}
				return examination_techniques;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}


		public Examination_Program createTextsExaminationPrograms(Examination_Program examination_program)
		{
			foreach (Examination_Technique examination_technique in examination_program.examination_techniques)
			{
				Debug.Print("examination_technique.video = " + examination_technique.video     );
				examination_program.video = "https://www.youtube.com/playlist?list=PLmuRAGZci9g9oSwrNoKXXp3a6iIPO1-Y1";
				if (examination_technique.type == "kihon")
				{
					if (examination_program.kihonText != null)
					{ 
						examination_program.kihonText = examination_program.kihonText + "\n" + examination_technique.order + " - " + examination_technique.name;
					}
					else
					{
						examination_program.kihonText = examination_technique.order + " - " + examination_technique.name;
					}
				}
				if (examination_technique.type == "kata")
				{
					if (examination_program.kataText != null)
					{
						examination_program.kataText = examination_program.kataText + "\n" + examination_technique.order + " - " + examination_technique.name;
					}
					else
					{
						examination_program.kataText = examination_technique.order + " - " + examination_technique.name;
					}
					
				}
				if (examination_technique.type == "kumite")
				{
					if (examination_program.kumiteText != null)
					{
						examination_program.kumiteText = examination_program.kumiteText + "\n" + examination_technique.order + " - " + examination_technique.name;
					}
					else
					{
						examination_program.kumiteText = examination_technique.order + " - " + examination_technique.name;
					}
					
				}
				if (examination_technique.type == "shiai_kumite")
				{
					if (examination_program.shiaikumiteText != null)
					{
						examination_program.shiaikumiteText = examination_program.shiaikumiteText + "\n" + examination_technique.order + " - " + examination_technique.name;
					}
					else
					{
						examination_program.shiaikumiteText = examination_technique.order + " - " + examination_technique.name;
					}
					
				}
			}
			return examination_program;
		}


	}
}