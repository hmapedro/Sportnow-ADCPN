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
	public class DojoManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Dojo> dojos;

		public DojoManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);
		}


		public async Task<List<Dojo>> GetAllDojos()
		{
			Debug.WriteLine("GetDojos");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Dojo_Info));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
			

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content = "+ content);
					dojos = JsonConvert.DeserializeObject<List<Dojo>>(content);

				}
				else
				{
					Debug.WriteLine("dojos not ok");
				}
				return dojos;
			}
			catch (Exception e)
			{
				Debug.WriteLine("http request error");
				Debug.Print(e.StackTrace);
				return null;
			}

		}


	}
}