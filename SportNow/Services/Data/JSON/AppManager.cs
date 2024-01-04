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
	public class AppManager
	{
		//IRestService restService;

		HttpClient client;


		public AppManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);
		}


        public async Task<MinimumVersion> GetMinimumVersion()
		{
			Debug.WriteLine("AppManager.GetMinimumVersion "+ Constants.RestUrl_Get_Minimum_Version);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Minimum_Version));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content = " + content);
					List<MinimumVersion> VersionList = JsonConvert.DeserializeObject<List<MinimumVersion>>(content);

					return VersionList[0];

                }
				else
				{
					Debug.WriteLine("login not ok");
					return null;
				}	
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