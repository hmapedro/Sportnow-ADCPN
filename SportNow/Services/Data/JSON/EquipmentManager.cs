using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using SportNow.Model;

namespace SportNow.Services.Data.JSON
{
	public class EquipmentManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Equipment> equipments { get; private set; }

        public List<EquipmentOrder> equipmentorders { get; private set; }

        

        public EquipmentManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);

		}

		public async Task<List<Equipment>> GetEquipments()
		{
			Debug.Print("GetEquipments");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Equipments, string.Empty));
			try
			{ 
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					equipments = JsonConvert.DeserializeObject<List<Equipment>>(content);
				}
				return equipments;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return null;
			}

		}

		public async Task<string> CreateEquipmentOrder(string userid, string username, string equipmentid, string equipmentname)
		{
			Debug.Print("CreateEquipmentOrder "+ Constants.RestUrl_Create_EquipmentOrder + "?userid=" + userid + "&username=" + username + "&equipmentid=" + equipmentid + "&equipmentname=" + equipmentname);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Create_EquipmentOrder + "?userid="+userid + "&username="+username+"&equipmentid="+equipmentid+"&equipmentname="+ equipmentname, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.Print("content=" + content);
					List<Result> createResultList = JsonConvert.DeserializeObject<List<Result>>(content);

					return createResultList[0].result;
				}
				return "-1";
			}
			catch
			{
				Debug.WriteLine("http request error");
				return "-2";
			}
		}

        public async Task<EquipmentOrder> GetEquipment_Order_byID(string equipment_orderID)
        {
            Debug.Print("GetEquipment_Order_byID " + Constants.RestUrl_Get_EquipmentOrder_byID + "?equipmentorderid=" + equipment_orderID);
            Uri uri = new Uri(string.Format(Constants.RestUrl_Get_EquipmentOrder_byID + "?equipmentorderid="+ equipment_orderID, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    //return true;
                    string content = await response.Content.ReadAsStringAsync();
                    equipmentorders = JsonConvert.DeserializeObject<List<EquipmentOrder>>(content);
                }
                return equipmentorders[0];
            }
            catch
            {
                Debug.WriteLine("http request error");
                return null;
            }

        }

    }
}