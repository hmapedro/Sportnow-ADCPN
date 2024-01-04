using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;
using System.Collections.ObjectModel;

namespace SportNow.Services.Data.JSON
{
	public class ExaminationSessionManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Examination_Session> examination_sessions{ get; private set; }
		public List<Examination> examinations { get; private set; }
		public List<Payment> payments { get; private set; }


		public ExaminationSessionManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<List<Examination_Session>> GetFutureExaminationSessions(string memberid)
		{
			Debug.Print("GetFutureExaminationSessions");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Future_Examination_Sessions + "?userid=" + memberid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					examination_sessions = JsonConvert.DeserializeObject<List<Examination_Session>>(content);
				}
				return examination_sessions;
			}
			catch 
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<Examination_Session> GetExamination_SessionByID(string userid, string examination_session_id)
		{
			Debug.Print("GetExamination_SessionByID");
			Uri uri = new Uri(string.Format(Constants.RestUrl_GetExamination_SessionByID + "?userid="+userid+ "&examinationsessionid=" + examination_session_id, string.Empty));
			try
			{ 
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					examination_sessions = JsonConvert.DeserializeObject<List<Examination_Session>>(content);
				}
				return examination_sessions[0];
			}
			catch 
			{
				Debug.WriteLine("http request error");
				return null;
			}
}

		
		public async Task<List<Payment>> GetExamination_Payment(string examinationid)
		{
			Debug.Print("GetExamination_Payment examinationid = "+ examinationid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Examination_Payment + "?examinationid=" + examinationid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.Print("content aqui = " + content);
					payments = JsonConvert.DeserializeObject<List<Payment>>(content);
					Debug.Print("content aqui1 = " + content);
				}
				return payments;
			}
			catch 
			{
				Debug.Print("http request error");
				return null;
			}
		}

		public async Task<List<Examination>> GetExamination_SessionCall(string examinationsessionid)
		{
			Debug.Print("GetExamination_SessionCall");
			Uri uri = new Uri(string.Format(Constants.RestUrl_GetExamination_SessionCall + "?examinationsessionid=" + examinationsessionid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.Print("content=" + content);
					examinations = JsonConvert.DeserializeObject<List<Examination>>(content);
				}
				else {
					Debug.Print("GetExamination_SessioCall IsSuccessStatusCode error");
				}
				return examinations;
			}
			catch (Exception e)
			{
				Debug.WriteLine("http request error "+e.Message);
				return null;
			}
		}

		public async Task<ObservableCollection<Examination>> GetExamination_SessionCall_obs(string examinationsessionid)
		{
			Debug.Print("GetExamination_SessionCall");
			ObservableCollection<Examination> examinations_obs = new ObservableCollection<Examination>();
			Uri uri = new Uri(string.Format(Constants.RestUrl_GetExamination_SessionCall + "?examinationsessionid=" + examinationsessionid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.Print("content=" + content);
					examinations_obs = JsonConvert.DeserializeObject<ObservableCollection<Examination>>(content);
				}
				else
				{
					Debug.Print("GetExamination_SessioCall IsSuccessStatusCode error");
				}
				return examinations_obs;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}
	}
}