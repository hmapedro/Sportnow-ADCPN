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
	public class PaymentManager
	{
		//IRestService restService;

		HttpClient client;
		

		public PaymentManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<Payment> GetPayment(string paymentid)
		{
			Debug.WriteLine("GetPayment " + Constants.RestUrl_Get_Payment + "?pagamentoid=" + paymentid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Payment + "?pagamentoid=" + paymentid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					List<Payment> paymentTemp = JsonConvert.DeserializeObject<List<Payment>>(content);
					return paymentTemp[0];
				}
				else
				{
					Debug.WriteLine("error getting fees");
					return null;
				}

				return null;
			}
			catch (Exception e)
			{
				Debug.WriteLine("PaymentManager.GetPayment http request error " + e.ToString());
				return null;
			}
		}

		public async Task<string> CreateMbWayPayment(string memberid, string paymentid, string orderid, string phonenumber, string value, string email)
		{
			Debug.Print("CreateMbWayPayment begin "+ Constants.RestUrl_Create_MbWay_Payment + "?userid=" + memberid + "&paymentid=" + paymentid + "&phonenumber=" + phonenumber + "&value=" + value + "&email=" + email + "&orderid=" + orderid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Create_MbWay_Payment + "?userid=" + memberid + "&paymentid=" + paymentid + "&phonenumber=" + phonenumber + "&value=" + value + "&email=" + email + "&orderid=" + orderid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);
				string content = await response.Content.ReadAsStringAsync();
				Debug.WriteLine("content=" + content);

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