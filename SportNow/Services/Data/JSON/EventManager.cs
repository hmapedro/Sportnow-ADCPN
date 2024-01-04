using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;

namespace SportNow.Services.Data.JSON
{
	public class EventManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Event> events { get; private set; }

		public List<Event_Participation> event_participations { get; private set; }

		public List<Payment> payments { get; private set; }
		

		public EventManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<List<Event>> GetFutureEventsAll(string memberid)
		{
			Debug.Print("GetFutureEventsAll");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Future_Events_All + "?userid=" + memberid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					events = JsonConvert.DeserializeObject<List<Event>>(content);
				}
				return events;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<List<Event>> GetImportantEvents(string memberid)
		{
			Debug.Print("GetImportantEvents "+ Constants.RestUrl_Get_Important_Events + "?userid=" + memberid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Important_Events + "?userid=" + memberid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					events = JsonConvert.DeserializeObject<List<Event>>(content);
				}
				return events;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<Event> GetEvent_byID(string memberid, string eventid)
		{
			Debug.Print("GetEvent_byID");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Event_byID + "?userid=" + memberid+ "&eventid="+ eventid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					events = JsonConvert.DeserializeObject<List<Event>>(content);
				}
				return events[0];
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<List<Event_Participation>> GetFutureEventParticipations(string memberid)
		{
			Debug.Print("GetFutureEventParticipations");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Future_EventParticipations + "?userid="+memberid, string.Empty));
			try
			{ 
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					event_participations = JsonConvert.DeserializeObject<List<Event_Participation>>(content);
				}
				return event_participations;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
}

		public async Task<List<Event_Participation>> GetPastEventParticipations(string memberid)
		{
			Debug.Print("GetPastEventParticipations");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Past_EventParticipations + "?userid=" + memberid, string.Empty));
			try
            {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					event_participations = JsonConvert.DeserializeObject<List<Event_Participation>>(content);
				}
				return event_participations;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
}

		public async Task<Event_Participation> GetEventParticipation(string eventparticipationid)
		{
			Debug.Print("GetEventParticipation");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_EventParticipation + "?eventparticipationid=" + eventparticipationid, string.Empty));
			try
			{ 
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					event_participations = JsonConvert.DeserializeObject<List<Event_Participation>>(content);
				}
				return event_participations[0];
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
}

		public async Task<string> CreateEventParticipation(string memberid, string eventid)
		{
			Debug.Print("CreateEventParticipation");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Create_EventParticipation + "?userid=" + memberid + "&eventid="+eventid, string.Empty));
			Debug.Print("CreateEventParticipation "+ uri.ToString());
			try
            {
				HttpResponseMessage response = await client.GetAsync(uri);
				var result = "0";
		
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					List<Result> createResultList = JsonConvert.DeserializeObject<List<Result>>(content);

					return createResultList[0].result;
				}
				else
				{
					Debug.WriteLine("error creating event participation");
					result = "-1";
				}

				return result;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return "-1";
			}
		}

		public async Task<List<Payment>> GetEventParticipation_Payment(string eventparticipationid)
		{
			Debug.Print("GetEventParticipation_Payment");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_EventParticipation_Payment + "?eventparticipationid=" + eventparticipationid, string.Empty));
			try
			{
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


		public async Task<string> Update_Event_Participation_Status(string competition_participationid, string status)
		{
			//Debug.Print("Update_Event_Participation_Status");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_EventParticipation_Status + "?eventparticipationid=" + competition_participationid+"&status="+status, string.Empty));
			Debug.Print("Update_Event_Participation_Status "+ uri.ToString());
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					List<Result> updateResultList = JsonConvert.DeserializeObject<List<Result>>(content);

					return updateResultList[0].result;
				}
				else
				{
					Debug.WriteLine("login not ok aqui");
					return "-2";
				}
			}
			catch
			{
				Debug.WriteLine("http request error");
				return "-2";
			}
		}

	}
}