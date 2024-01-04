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
	public class AwardManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Award> awards { get; private set; }
		

		public AwardManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<List<Award>> GetAwards_Student(string memberid)
		{
			Debug.Print("GetAwards_Student "+ string.Format(Constants.RestUrl_Get_Awards_Student + "?userid=" + memberid));
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Awards_Student + "?userid=" + memberid, string.Empty));
			try {
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					awards = JsonConvert.DeserializeObject<List<Award>>(content);
				}
				return awards;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}

		}

	}
}