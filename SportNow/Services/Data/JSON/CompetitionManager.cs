using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;

namespace SportNow.Services.Data.JSON
{
	public class CompetitionManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Competition> competitions { get; private set; }

		public List<Competition_Participation> competition_participations { get; private set; }

		public List<Payment> payments { get; private set; }
		

		public CompetitionManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<List<Competition>> GetFutureCompetitions(string memberid)
		{
			Debug.Print("GetFutureCompetitions");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Future_Competitions + "?userid=" + memberid, string.Empty));
			try {

				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					competitions = JsonConvert.DeserializeObject<List<Competition>>(content);
				}
				return competitions;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
}

		public async Task<List<Competition>> GetFutureCompetitionsAll()
		{
			Debug.Print("GetFutureCompetitionsAll");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Future_Competitions_All, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					competitions = JsonConvert.DeserializeObject<List<Competition>>(content);
				}
				return competitions;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<List<Competition>> GetCompetitionByID(string userid, string competitionid)
		{
			//Debug.Print("CompetitionManager.GetCompetitionByID "+ Constants.RestUrl_Get_CompetitionByID + "?competitionid=" + competitionid + "&userid=" + userid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_CompetitionByID+ "?competitionid="+ competitionid+ "&userid=" + userid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					competitions = JsonConvert.DeserializeObject<List<Competition>>(content);
				}
				return competitions;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}


		public async Task<List<Competition>> GetCompetitionByParticipationID(string userid, string competitionparticipationid)
		{
			Debug.Print("GetCompetitionByParticipationID");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_CompetitionByParticipationID + "?competitionparticipationid=" + competitionparticipationid + "&userid=" + userid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					competitions = JsonConvert.DeserializeObject<List<Competition>>(content);
				}
				return competitions;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<List<Competition_Participation>> GetFutureCompetitionParticipations(string memberid)
		{
			Debug.Print("GetFutureCompetitionParticipations");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Future_CompetitionParticipations+"?userid="+memberid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					competition_participations = JsonConvert.DeserializeObject<List<Competition_Participation>>(content);
				}
				return competition_participations;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<List<Competition_Participation>> GetPastCompetitionParticipations(string memberid)
		{
			Debug.Print("GetPastCompetitionParticipations");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Past_CompetitionParticipations + "?userid=" + memberid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					competition_participations = JsonConvert.DeserializeObject<List<Competition_Participation>>(content);
				}
				return competition_participations;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}


		public async Task<List<Competition_Participation>> GetCompetitionCall(string competitionid)
		{
			Debug.Print("CompetitionManager.GetCompetitionCall "+ Constants.RestUrl_Get_Competition_Call + "?competitionid=" + competitionid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Competition_Call + "?competitionid=" + competitionid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					competition_participations = JsonConvert.DeserializeObject<List<Competition_Participation>>(content);
				}
				return competition_participations;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
}


		public async Task<List<Payment>> GetCompetitionParticipation_Payment(List<Competition> competitions)
		{
			Debug.Print("GetCompetitionParticipation_Payment");
			var competitionString = "";
			foreach (Competition competition in competitions)
			{
				competitionString = competitionString + "'" + competition.participationid+"', ";
			}
			competitionString = competitionString.Substring(0, competitionString.Length - 2);

			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_CompetitionParticipation_Payment + "?competitionparticipationid=" + competitionString, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					payments = JsonConvert.DeserializeObject<List<Payment>>(content);
				}

				return payments;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<Payment> GetCompetitionParticipation_Payment(Competition competition)
		{
			var competitionString = "'" + competition.participationid + "'";
            Debug.Print("GetCompetitionParticipation_Payment " + Constants.RestUrl_Get_CompetitionParticipation_Payment + "?competitionparticipationid=" + competitionString);

            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_CompetitionParticipation_Payment + "?competitionparticipationid=" + competitionString, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					payments = JsonConvert.DeserializeObject<List<Payment>>(content);
				}

				return payments[0];
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<string> Update_Competition_Participation_Status(string competition_participationid, string status)
		{
			Debug.Print("Update_Competition_Participation_Status "+ Constants.RestUrl_Update_CompetitionParticipation_Status + "?competitionparticipationid=" + competition_participationid + "&status=" + status);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_CompetitionParticipation_Status + "?competitionparticipationid=" + competition_participationid+"&status="+status, string.Empty));
			try { 
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					List<Result> updateResultList = JsonConvert.DeserializeObject<List<Result>>(content);

					return updateResultList[0].result;
				}
				else
				{
					return "-2";
				}
			}
			catch
			{
				Debug.WriteLine("http request error");
				return "-3";
			}
		}

	}
}