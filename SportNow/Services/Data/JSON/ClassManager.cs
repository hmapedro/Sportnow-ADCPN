using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;
using SportNow.Views;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;

namespace SportNow.Services.Data.JSON
{
	public class ClassManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Class_Detail> class_details { get; private set; }
		public List<Class_Attendance> class_attendances { get; private set; }
		public List<Class_Inactivity> class_inactivities { get; private set; }
		public List<Class_Schedule> class_schedules { get; private set; }


		public ClassManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<List<Class_Detail>> GetClass_DetailAll(string memberid)
		{
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Classes_Detail + "?userid=" + memberid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					class_details = JsonConvert.DeserializeObject<List<Class_Detail>>(content);
				}
				return class_details;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}

}

		public async Task<List<Class_Attendance>> GetFutureClass_Attendances(string memberid)
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Future_Classes_Attendances + "?userid=" + memberid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					class_attendances = JsonConvert.DeserializeObject<List<Class_Attendance>>(content);
				}
				return class_attendances;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}


		public async Task<List<Class_Attendance>> GetClass_Attendances(string classid, string classdate)
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Class_Attendances + "?classid=" + classid + "&classdate="+ classdate, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					class_attendances = JsonConvert.DeserializeObject<List<Class_Attendance>>(content);
				}
				return class_attendances;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<ObservableCollection<Class_Attendance>> GetClass_Attendances_obs(string classid, string classdate)
		{
			ObservableCollection<Class_Attendance> class_attendances_obs = new ObservableCollection<Class_Attendance>();
			//Debug.Print("classid = " + classid + " classdate = " + classdate);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Class_Attendances + "?classid=" + classid + "&classdate=" + classdate, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					class_attendances_obs = JsonConvert.DeserializeObject<ObservableCollection<Class_Attendance>>(content);
				}
				return class_attendances_obs;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}


		public async Task<List<Class_Inactivity>> GetClasses_Inactivities(string memberid)
		{
			Debug.WriteLine("ClassManager.GetClasses_Inactivities");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Classes_Inactivities + "?userid=" + memberid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					class_inactivities = JsonConvert.DeserializeObject<List<Class_Inactivity>>(content);
				}
				return class_inactivities;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}


		public async Task<string> CreateClass_Attendance(string memberid, string classid, string status, string date)
		{
			Debug.WriteLine("CreateClass_Attendace "+ Constants.RestUrl_Create_Classe_Attendance + "?userid=" + memberid + "&classid=" + classid + "&status=" + status + "&date=" + date);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Create_Classe_Attendance + "?userid=" + memberid + "&classid=" + classid + "&status=" + status + "&date=" + date, string.Empty));
			try {
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
					Debug.WriteLine("error creating class attendance");
					result = "-1";
				}

				return result;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return "-2";
			}
		}

		public async Task<int> UpdateClass_Attendance(string classattendanceid, string status)
		{
			Debug.WriteLine("UpdateClass_Attendance " + Constants.RestUrl_Update_Classe_Attendance + "?classattendanceid=" + classattendanceid + "&status=" + status);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Classe_Attendance + "?classattendanceid=" + classattendanceid + "&status=" + status, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);
				var result = 0;
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					//member.fees = JsonConvert.DeserializeObject<List<Fee>>(content);
					result = 1;
				}
				else
				{
					Debug.WriteLine("error updating class attendance");
					result = -1;
				}

				return result;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return -2;
			}
		}

		public async Task<List<Class_Attendance>> GetPastClass_AttendancesStats(string memberid)
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Past_Classes_AttendanceStats + "?userid=" + memberid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					class_attendances = JsonConvert.DeserializeObject<List<Class_Attendance>>(content);
				}
				return class_attendances;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}

		public async Task<List<Class_Schedule>> GetStudentClass_Schedules(string memberid, string begindate, string enddate)
		{
			Debug.WriteLine("ClassManager.GetStudentClass_Schedules " + Constants.RestUrl_Get_Student_Class_Schedules + "?userid=" + memberid + "&begindate=" + begindate + "&enddate=" + enddate); 
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Student_Class_Schedules + "?userid=" + memberid + "&begindate=" + begindate + "&enddate=" + enddate, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("ClassManager.GetStudentClass_Schedules content=" + content);
					class_schedules= JsonConvert.DeserializeObject<List<Class_Schedule>>(content);
				}
				return class_schedules;
			}
			catch
			{
				Debug.WriteLine("ClassManager.GetStudentClass_Schedules http request error");
				return null;
			}
		}

		public async Task<ObservableCollection<Class_Schedule>> GetStudentClass_Schedules_obs(string memberid, string begindate, string enddate)
		{
            Debug.WriteLine("ClassManager.GetStudentClass_Schedules_obs " + Constants.RestUrl_Get_Student_Class_Schedules + "?userid=" + memberid + "&begindate=" + begindate + "&enddate=" + enddate);
            ObservableCollection<Class_Schedule> class_schedules_obs = new ObservableCollection<Class_Schedule>();
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Student_Class_Schedules + "?userid=" + memberid + "&begindate=" + begindate + "&enddate=" + enddate, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("ClassManager.GetStudentClass_Schedules_obs content=" + content);
					class_schedules_obs = JsonConvert.DeserializeObject<ObservableCollection<Class_Schedule>>(content);
				}
				return class_schedules_obs;
			}
			catch
			{
				Debug.WriteLine("ClassManager.GetStudentClass_Schedules_obs http request error");
				return null;
			}
		}

		public async Task<List<Class_Schedule>> GetAllClass_Schedules(string memberid, string begindate, string enddate)
		{
			Debug.Print("GetAllClass_Schedules " + Constants.RestUrl_Get_All_Class_Schedules + "?userid=" + memberid + "&begindate=" + begindate + "&enddate=" + enddate);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_All_Class_Schedules + "?userid=" + memberid + "&begindate=" + begindate + "&enddate=" + enddate, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					class_schedules = JsonConvert.DeserializeObject<List<Class_Schedule>>(content);
				}
				return class_schedules;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}


        public async Task<List<Class_Detail>> GetAllClasses(string include_danca)
        {
            Debug.Print("GetAllClasses " + Constants.RestUrl_Get_All_Classes + "?include_danca=" + include_danca);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_All_Classes + "?include_danca=" + include_danca));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("content=" + content);
                    class_details = JsonConvert.DeserializeObject<List<Class_Detail>>(content);
                }
                return class_details;
            }
            catch
            {
                Debug.WriteLine("http request error");
                return null;
            }

        }

        public async Task<List<Class_Detail>> GetAllClasses()
		{
			Debug.Print("GetAllClasses " + Constants.RestUrl_Get_All_Classes);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_All_Classes, string.Empty));
            Debug.Print("GetAllClasses 2");
			try
			{
                Debug.Print("GetAllClasses 3");
                HttpResponseMessage response = await client.GetAsync(uri);
                Debug.Print("GetAllClasses 4");
                if (response.IsSuccessStatusCode)
				{
                    Debug.Print("GetAllClasses 5");
                    string content = await response.Content.ReadAsStringAsync();
					Debug.Print("content=" + content);
					class_details = JsonConvert.DeserializeObject<List<Class_Detail>>(content);
                }
                else
				{
                    Debug.Print("response.IsSuccessStatusCode=false");

                }
                Debug.Print("GetAllClasses 10");
                return class_details;
			}
			catch (Exception e)
			{
				Debug.Print("http request error "+e.Message);
				return null;
			}
		}
	}
}