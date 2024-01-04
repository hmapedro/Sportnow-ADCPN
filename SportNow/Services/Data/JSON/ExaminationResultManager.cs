using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;

namespace SportNow.Services.Data.JSON
{
	public class ExaminationResultManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Examination_Result> examination_results { get; private set; }
		Examination_Result examination_result;
		public List<Examination_Technic_Result> examination_technics_result { get; private set; }
		

		public ExaminationResultManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<string> CreateExamination_Result(string userid, string examinationid)
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl_Create_Examination_Result + "?userid=" + userid + "&examinationid=" + examinationid, string.Empty));
			Debug.Print(Constants.RestUrl_Create_Examination_Result + "?userid=" + userid + "&examinationid=" + examinationid);
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
					Debug.WriteLine("error creating examination result");
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

		public async Task<Examination_Result> GetExamination_Result_byID(string userid, string examination_result_id)
		{


			Debug.Print("GetExamination_Result_byID");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Examination_Result_byID + "?userid=" + userid + "&examination_result_id=" + examination_result_id, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					examination_results = JsonConvert.DeserializeObject<List<Examination_Result>>(content);
				}
				examination_result = examination_results[0];
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}

			Debug.Print("RestUrl_Get_Examination_Technics_Result_byID");
			uri = new Uri(string.Format(Constants.RestUrl_Get_Examination_Technics_Result_byID + "?userid="+userid+"&examination_result_id=" + examination_result_id, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					examination_technics_result = JsonConvert.DeserializeObject<List<Examination_Technic_Result>>(content);
				}
				examination_result.examination_technics_result = examination_technics_result;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}

			return examination_result;

		}

		public async Task<string> UpdateExamination_Technic_Result(string userid, string examination_resultid, string examination_technic_resultid, int evaluation, string description)
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Examination_Technic_Result + "?userid=" + userid + "&examination_resultid=" + examination_resultid + "&examination_technic_resultid=" + examination_technic_resultid + "&evaluation=" + evaluation + "&description="+ description, string.Empty));
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
					Debug.WriteLine("error creating examination result");
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

		public async Task<string> UpdateExamination_Result(string userid, string examination_resultid, string examination_result_description)
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Examination_Result + "?userid=" + userid + "&examination_resultid=" + examination_resultid + "&examination_result_description=" + examination_result_description, string.Empty));
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
					Debug.WriteLine("error creating examination result");
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
	}
}