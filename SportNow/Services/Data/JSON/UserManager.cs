using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;

namespace SportNow.Services.Data.JSON
{
	public class UserManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Member> members { get; private set; }

		public UserManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task <bool> Login(User user)
		{
			try
			{
				Uri uri = new Uri(string.Format(Constants.RestUrl_Login + "?username=" + user.Username +"&password="+user.Password, string.Empty));
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content="+ content);
					members = JsonConvert.DeserializeObject<List<Member>>(content);

					if (members.Count > 0)
					{
						Debug.WriteLine("login ok");
						return true;
					}
					else
					{
						Debug.WriteLine("login Not ok");
						return false;
					}

				}
				else {
					Debug.WriteLine("login not ok");
					return false;
				}
			}
			catch
			{
				Debug.WriteLine("http request error");
				return false;
			}
		}

		public async Task<List<Member>> GetMembers(User user)
		{
			try
			{
				Uri uri = new Uri(string.Format(Constants.RestUrl_Login + "?username=" + user.Username + "&password=" + user.Password, string.Empty));
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					members = JsonConvert.DeserializeObject<List<Member>>(content);
				}
				else
				{
					Debug.WriteLine("login not ok");
				}
				return members;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}
		}
	}
}