using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;
using SportNow.Views;
using System.Collections.ObjectModel;

namespace SportNow.Services.Data.JSON
{
	public class MonthFeeManager
	{
		//IRestService restService;

		HttpClient client;

		public ObservableCollection<MonthFee> monthFees;

		public List<Payment> payments { get; private set; }

		public MonthFeeManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);
		}

		public async Task<int> Update_MonthFee_Status_byID(string monthFeeId, string status, string paymentId)
		{
			Debug.WriteLine("MonthFeeManager.Update_MonthFee_Status_byID " + Constants.RestUrl_Update_MonthFee_Status_byID + "?userid=" + App.original_member.id + "&monthfeeid=" + monthFeeId + "&status=" + status + "&paymentId=" + paymentId);

			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_MonthFee_Status_byID + "?userid="+App.original_member.id+"&monthfeeid=" + monthFeeId + "&status=" + status + "&paymentId="+ paymentId, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				var result = 0;
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
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

		public async Task<int> Update_MonthFee_Value_byID(string monthFeeId, string value)
		{
			Debug.WriteLine("MonthFeeManager.Update_MonthFee_Value_byID " + Constants.RestUrl_Update_MonthFee_Value_byID + "?userid=" + App.original_member.id + "&monthfeeid=" + monthFeeId + "&value=" + value);

			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_MonthFee_Value_byID + "?userid=" + App.original_member.id + "&monthfeeid=" + monthFeeId + "&value=" + value, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				var result = 0;
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
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

        public async Task<ObservableCollection<MonthFee>> GetMonthFeesbyDojo(string dojo, string year, string month)
        {
            Debug.WriteLine("GetMonthFeesbyDojo");
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_MonthFees_byDojo + "?dojo=" + dojo + "&year=" + year + "&month=" + month));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);


                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    monthFees = JsonConvert.DeserializeObject<ObservableCollection<MonthFee>>(content);

                }
                else
                {
                    Debug.WriteLine("login not ok");
                }
                return monthFees;
            }
            catch (Exception e)
            {
                Debug.WriteLine("http request error");
                Debug.Print(e.StackTrace);
                return null;
            }

        }

		public async Task<ObservableCollection<MonthFee>> GetMonthFeesbyClass(string aula, string year, string month)
        {
            Debug.WriteLine("GetMonthFeesbyDojo " + Constants.RestUrl_Get_MonthFees_byClass + "?aula=" + aula + "&year=" + year + "&month=" + month);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_MonthFees_byClass + "?aula=" + aula + "&year=" + year + "&month=" + month));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);


                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    monthFees = JsonConvert.DeserializeObject<ObservableCollection<MonthFee>>(content);

                }
                else
                {
                    Debug.WriteLine("login not ok");
                }
                return monthFees;
            }
            catch (Exception e)
            {
                Debug.WriteLine("http request error");
                Debug.Print(e.StackTrace);
                return null;
            }

        }

        public async Task<ObservableCollection<MonthFee>> GetMonthFeesbyStudent(string userid, string year)
		{
			Debug.WriteLine("GetMonthFeesbyStudent");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_MonthFees_byStudent + "?userid=" + userid + "&year=" + year));
			Debug.WriteLine(Constants.RestUrl_Get_MonthFees_byStudent + " ? userid = " + userid + " & year = " + year);
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);


				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					monthFees = JsonConvert.DeserializeObject<ObservableCollection<MonthFee>>(content);
				}
				else
				{
					Debug.WriteLine("login not ok");
				}
				return monthFees;
			}
			catch (Exception e)
			{
				Debug.WriteLine("http request error");
				Debug.Print(e.StackTrace);
				return null;
			}

		}

		public async Task<string> Has_MonthFeesStudent(string userid)
		{
			Debug.WriteLine("Get_has_MonthFeesStudent "+ Constants.RestUrl_Has_MonthFeesStudent + "?userid=" + userid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Has_MonthFeesStudent + "?userid=" + userid));
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
					Debug.WriteLine("error getting has month fees student");
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

        public async Task<string> Get_Has_DelayedMonthFees(string userid)
        {
            Debug.WriteLine("Get_Has_DelayedMonthFees " + Constants.Get_Has_DelayedMonthFees + "?userid=" + userid);
            Uri uri = new Uri(string.Format(Constants.Get_Has_DelayedMonthFees + "?userid=" + userid));
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
                    Debug.WriteLine("error getting has month fees student");
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

        public async Task<List<Payment>> GetMonthFee_Payment(string monthFeeId)
		{
			Debug.Print("GetMonthFee_Payment " + Constants.RestUrl_Get_MonthFee_Payment + "?monthfeeid=" + monthFeeId);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_MonthFee_Payment + "?monthfeeid=" + monthFeeId, string.Empty));
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

		public async Task<string> CreateMonthFee(string original_memberid, string memberid, string membername, string classid, string value, string year, string month, string status, string paymentid, string create_payment)
		{
			Debug.Print("CreateMonthFee begin " + Constants.RestUrl_Create_MonthFee + "?original_memberid=" + original_memberid + "&memberid=" + memberid + "&membername=" + membername + "&classid=" + classid + "&value=" + value + "&year=" + year + "&month=" + month + "&status=" + status + "&paymentid=" + paymentid + "&create_payment=" + create_payment);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Create_MonthFee + "?original_memberid=" + original_memberid + "&memberid=" + memberid + "&membername=" + membername + "&classid=" + classid + "&value=" + value + "&year=" + year + "&month=" + month + "&status=" + status + "&paymentid=" + paymentid + "&create_payment=" + create_payment, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				string content = await response.Content.ReadAsStringAsync();

				if (response.IsSuccessStatusCode)
				{
					//return true;
					//string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					List<Result> createResultList = JsonConvert.DeserializeObject<List<Result>>(content);

					return createResultList[0].result;

				}
				else
				{
					Debug.WriteLine("error creating payment MBWay");
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