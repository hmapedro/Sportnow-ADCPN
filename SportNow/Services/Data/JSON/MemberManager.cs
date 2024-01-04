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
	public class MemberManager
	{
		//IRestService restService;

		HttpClient client;

		public List<Member> members;
		public List<Document> documents;

		public ObservableCollection<Member> members_observable;

		public List<Payment> payments { get; private set; }


		public MemberManager()
		{
			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);
		}

		public async Task<int> updateToken(string memberid, string token)
		{
			Debug.WriteLine("MemberManager.updateToken " + memberid + " " + token);
			if ((memberid == null) | (token == null) | (memberid == "") | (token == ""))
			{
				Debug.WriteLine("MemberManager.updateToken memberid or token empty");
				return -4;
			}
			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Token + "?userid=" + memberid + "&token=" + token, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					List<Login> loginResultList = JsonConvert.DeserializeObject<List<Login>>(content);
					return loginResultList[0].login;
				}
				else
				{
					Debug.WriteLine("login not ok");
					return -2;
				}
			}
			catch
			{
				Debug.WriteLine("http request error");
				return -3;
			}
		}


		/*public async Task<string> Login(User user)
		{
			var return_value = "0";
			Debug.WriteLine("Login");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Login + "?username=" + user.Username + "&password=" + user.Password, string.Empty));
			try
			{

				return_value = "-3";
				HttpResponseMessage response = await client.GetAsync(uri);
				return_value = "-4";
				if (response.IsSuccessStatusCode)
				{
					//return true;
					return_value = "-5";
					string content = await response.Content.ReadAsStringAsync();
					return_value = content;
					Debug.WriteLine("content=" + content);
					List<Login> loginResultList= JsonConvert.DeserializeObject<List<Login>>(content);
					Debug.WriteLine("loginResultList=" + loginResultList[0].login);
					return_value = "-7";
					return "1";
				}
				else
				{
					Debug.WriteLine("login not ok");
					return "-2";
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("http request error "+ e.ToString());
				return return_value+ e.ToString();
			}
		}*/

		public async Task<string> Login(User user)
		{
			Debug.WriteLine("Login " + Constants.RestUrl_Login + "?username=" + user.Username + "&password=" + user.Password);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Login + "?username=" + user.Username + "&password=" + user.Password, string.Empty));
			Debug.WriteLine("Login " + uri.ToString());
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					List<Login> loginResultList = JsonConvert.DeserializeObject<List<Login>>(content);
					return Convert.ToString(loginResultList[0].login);
				}
				else
				{
					Debug.WriteLine("login not ok");
					return "-2";
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("http request error");
				return "-3 " + e.Message + " " + e.InnerException.Message; ;
			}
		}

		public async Task<List<Member>> GetMembers(User user)
		{
			Debug.WriteLine("GetMembers " + Constants.RestUrl_Get_Member_Info + "?username=" + user.Username);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Member_Info + "?username=" + user.Username));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);


				if (response.IsSuccessStatusCode)
				{
					Debug.WriteLine("login ok AQUI");
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content = " + content);
					members = JsonConvert.DeserializeObject<List<Member>>(content);

				}
				else
				{
					Debug.WriteLine("login not ok");
				}
				return members;
			}
			catch (Exception e)
			{
				Debug.WriteLine("http request error");
				Debug.Print(e.StackTrace);
				return null;
			}

		}

		public async Task<List<Member>> GetMemberStudents(string memberid)
		{
			Debug.WriteLine("GetMemberStudents " + Constants.RestUrl_Get_Member_Students_Info + "?userid=" + memberid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Member_Students_Info + "?userid=" + memberid));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);


				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content = " + content);
					members = JsonConvert.DeserializeObject<List<Member>>(content);

				}
				else
				{
					Debug.WriteLine("login not ok");
				}
				return members;
			}
			catch (Exception e)
			{
				Debug.WriteLine("http request error");
				Debug.Print(e.StackTrace);
				return null;
			}

		}

		public async Task<List<Member>> GetStudentsDojo(string memberid, string dojo)
		{
			Debug.WriteLine("GetStudentsDojo dojo=" + dojo);
			Debug.Print(Constants.RestUrl_Get_Students_Dojo_Info + "?userid=" + memberid + "&dojo=" + dojo);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Students_Dojo_Info + "?userid=" + memberid + "&dojo=" + dojo));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);


				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content = " + content);
					members = JsonConvert.DeserializeObject<List<Member>>(content);

				}
				else
				{
					Debug.WriteLine("login not ok");
				}
				return members;
			}
			catch (Exception e)
			{
				Debug.WriteLine("http request error");
				Debug.Print(e.StackTrace);
				return null;
			}

		}

		public async Task<List<Member>> GetStudentsClass(string memberid, string classid)
		{
			Debug.Print("GetStudentsClass " + Constants.RestUrl_Get_Students_Dojo_ByClass_Info + "?userid=" + memberid + "&classid=" + classid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Students_Dojo_ByClass_Info + "?userid=" + memberid + "&classid=" + classid));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);


				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content = " + content);
					members = JsonConvert.DeserializeObject<List<Member>>(content);

				}
				else
				{
					Debug.WriteLine("login not ok");
				}
				return members;
			}
			catch (Exception e)
			{
				Debug.WriteLine("http request error");
				Debug.Print(e.StackTrace);
				return null;
			}

		}

		public async Task<int> GetMemberStudents_Count(string memberid)
		{
			Debug.WriteLine("MemberManager.GetMemberStudents_Count " + Constants.RestUrl_Get_Member_Students_Count + "?userid=" + memberid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Member_Students_Count + "?userid=" + memberid, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content = " + content);
					List<Result> createResultList = JsonConvert.DeserializeObject<List<Result>>(content);

					return Convert.ToInt32(createResultList[0].result);
				}
				else
				{
					Debug.WriteLine("login not ok");
					return -1;
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine("http request error");
				Debug.Print(e.StackTrace);
				return -2;
			}
		}


		public async Task<int> GetExaminations(Member member)
		{
			Debug.WriteLine("GetExaminations");
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Examinations + "?userid=" + member.id, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				var result = 0;
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					member.examinations = JsonConvert.DeserializeObject<List<Examination>>(content);
					result = 1;
				}
				else
				{
					Debug.WriteLine("login not ok");
					result = -1;
				}

				return result;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return -1;
			}
		}

		public async Task<int> GetPastFees(Member member)
		{
			Debug.WriteLine("GetPastFees "+ Constants.RestUrl_Get_Past_Fees + "?userid=" + member.id);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Past_Fees + "?userid=" + member.id, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				var result = 0;
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					member.pastFees = JsonConvert.DeserializeObject<List<Fee>>(content);
					result = 1;
				}
				else
				{
					Debug.WriteLine("error getting fees");
					result = -1;
				}

				return result;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return -1;
			}
		}


		public async Task<int> GetCurrentFees(Member member)
		{
			Debug.WriteLine("GetCurrentFees " + Constants.RestUrl_Get_Current_Fees + "?userid=" + member.id);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Current_Fees + "?userid=" + member.id, string.Empty));
			try
			{


				HttpResponseMessage response = await client.GetAsync(uri);
				var result = 0;
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					List<Fee> feesTemp = JsonConvert.DeserializeObject<List<Fee>>(content);
					if ((feesTemp != null) & (feesTemp.Count > 0))

					{
						member.currentFee = feesTemp[0];
					}
					result = 1;
				}
				else
				{
					Debug.WriteLine("error getting fees");
					result = -1;
				}

				return result;
			}
			catch (Exception e)
			{
				Debug.WriteLine("MemberManager.GetCurrentFees http request error " + e.ToString());
				return -1;
			}
		}

		public async Task<List<Fee>> GetFees(string memberid, string season)
		{
			Debug.WriteLine("GetFees " + Constants.RestUrl_Get_Fees + "?userid=" + memberid + "&season=" + season);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Fees + "?userid=" + memberid + "&season=" + season, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					List<Fee> feesTemp = JsonConvert.DeserializeObject<List<Fee>>(content);
					return feesTemp;
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
				Debug.WriteLine("MemberManager.GetCurrentFees http request error " + e.ToString());
				return null;
			}
		}

		public async Task<int> CreateFee(Member member, string period)
		{
			Debug.WriteLine("CreateFee begin " + Constants.RestUrl_Create_Fee + "?userid=" + member.id + "&period=" + period);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Create_Fee + "?userid=" + member.id + "&period=" + period, string.Empty));
			try
			{
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
					Debug.WriteLine("error creating fee");
					result = -1;
				}

				return result;
			}
			catch
			{
				Debug.WriteLine("http request error");
				return -1;
			}
		}


		public async Task<string> CreateAllFees(string original_memberid, string memberid, string membername, string membertype, string season)
		{
			Debug.WriteLine("CreateAllFees begin " + Constants.RestUrl_Create_All_Fees + "?original_memberid=" + original_memberid + "&memberid=" + memberid + "&membername=" + membername + "&membertype=" + membertype + "&season=" + season);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Create_All_Fees + "?original_memberid=" + original_memberid + "&memberid=" + memberid + "&membername=" + membername + "&membertype=" + membertype + "&season=" + season, string.Empty));

			List<Fee> feesTemp = new List<Fee>();
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);
				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					List<Result> createResultList = JsonConvert.DeserializeObject<List<Result>>(content);
					return createResultList[0].result;
				}
				else
				{
					Debug.WriteLine("error creating all fees");
				}
				return "-1";
			}
			catch
			{
				Debug.WriteLine("http request error");
				return "-1";
			}
		}

		public async Task<int> ChangePassword(string email, string newpassword)
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Password + "?username=" + email + "&password=" + newpassword, string.Empty));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("content=" + content);
					List<Login> loginResultList = JsonConvert.DeserializeObject<List<Login>>(content);

					return loginResultList[0].login;
				}
				else
				{
					Debug.WriteLine("login not ok");
					return -2;
				}
			}
			catch
			{
				Debug.WriteLine("http request error");
				return -3;
			}
		}


		public async Task<int> RecoverPassword(string email)
		{
			Uri uri = new Uri(string.Format(Constants.RestUrl_Recover_Password + "?username=" + email, string.Empty));
			Debug.Print(Constants.RestUrl_Recover_Password + "?username=" + email);
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					//return true;
					string content = await response.Content.ReadAsStringAsync();
					List<Login> loginResultList = JsonConvert.DeserializeObject<List<Login>>(content);

					return loginResultList[0].login;
				}
				else
				{
					Debug.WriteLine("login not ok");
					return -2;
				}
			}
			catch
			{
				Debug.WriteLine("http request error");
				return -3;
			}
		}

		public async Task<string> UpdateMemberInfo(string original_member_id, Member member)
		{

			string member_first_name = member.name.Substring(0, member.name.IndexOf(" "));
			string member_last_name = member.name.Substring(member.name.IndexOf(" ") + 1);

			Debug.Print("UpdateMemberInfo " + Constants.RestUrl_Update_Member_Info + "?original_member_id="+original_member_id+"&memberid=" + member.id
				+ "&memberfirstname=" + member_first_name + "&memberlastname=" + member_last_name + "&membernif=" + member.nif + "&membercc=" + member.cc_number
				+ "&memberphone=" + member.phone + "&memberemail=" + member.email + "&memberaddress=" + member.address
				+ "&membercity=" + member.city + "&memberpostalcode=" + member.postalcode
				+ "&gender=" + member.gender + "&country=" + member.country + "&birthdate=" + member.birthdate
				+ "&nameEmergencyContact=" + member.nameEmergencyContact + "&phoneEmergencyContact=" + member.phoneEmergencyContact
				+ "&membernameenc1=" + member.name_enc1 + "&memberemailenc1=" + member.mail_enc1 + "&memberphoneenc1=" + member.phone_enc1
				+ "&membernameenc2=" + member.name_enc2 + "&memberemailenc2=" + member.mail_enc2 + "&memberphoneenc2=" + member.phone_enc2
				+ "&schoolname=" + member.schoolname + "&schoolnumber=" + member.schoolnumber + "&schoolyear=" + member.schoolyear + "&schoolclass=" + member.schoolclass
				+ "&comments=" + member.comentarios + "&n_familiares=" + member.n_familiares);

			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Member_Info + "?original_member_id="+original_member_id+"&memberid=" + member.id
				+ "&memberfirstname=" + member_first_name + "&memberlastname=" + member_last_name + "&membernif=" + member.nif + "&membercc=" + member.cc_number
				+ "&memberphone=" + member.phone + "&memberemail=" + member.email + "&memberaddress=" + member.address
				+ "&membercity=" + member.city + "&memberpostalcode=" + member.postalcode
				+ "&gender=" + member.gender + "&country=" + member.country + "&birthdate=" + member.birthdate
				+ "&nameEmergencyContact=" + member.nameEmergencyContact + "&phoneEmergencyContact=" + member.phoneEmergencyContact
				+ "&membernameenc1=" + member.name_enc1 + "&memberemailenc1=" + member.mail_enc1 + "&memberphoneenc1=" + member.phone_enc1
				+ "&membernameenc2=" + member.name_enc2 + "&memberemailenc2=" + member.mail_enc2 + "&memberphoneenc2=" + member.phone_enc2
				+ "&schoolname=" + member.schoolname + "&schoolnumber=" + member.schoolnumber + "&schoolyear=" + member.schoolyear + "&schoolclass=" + member.schoolclass
				+ "&comments=" + member.comentarios + "&n_familiares=" + member.n_familiares
				, string.Empty));


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
					Debug.WriteLine("error updating member info");
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

		public async Task<List<Payment>> GetFeePayment(string feeid)
		{
			Debug.Print("GetFeePayment "+ Constants.RestUrl_Get_FeePayment + "?feeid=" + feeid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_FeePayment + "?feeid=" + feeid, string.Empty));
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

		public async Task<List<Member>> GetMembers_To_Approve()
		{
			Debug.WriteLine("GetMembers_To_Approve - " + Constants.RestUrl_Get_Members_To_Approve);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Members_To_Approve));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);


				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("GetMembers_To_Approve - content = " + content);
					members = JsonConvert.DeserializeObject<List<Member>>(content);

				}
				else
				{
					Debug.WriteLine("GetMembers_To_Approve - not ok");
				}
				return members;
			}
			catch (Exception e)
			{
				Debug.WriteLine("GetMembers_To_Approve - http request error");
				Debug.Print(e.StackTrace);
				return null;
			}

		}

		public async Task<string> Update_Member_Approved_Status(string userid, string name, string email, string status, string classid)
		{


			Debug.Print("Update_Member_Approved_Status " + Constants.RestUrl_Update_Member_Approved_Status + "?userid=" + userid + "&name=" + name + "&email=" + email + "&status=" + status + "&classid=" + classid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Member_Approved_Status + "?userid=" + userid + "&name=" + name + "&email=" + email + "&status=" + status + "&classid=" + classid, string.Empty));
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
					Debug.WriteLine("error updating member info");
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

		public async Task<string> Update_Member_Authorizations(string userid, string assembleia, string regulamento, string tratamentodados, string registoimagem, string fotosocio, string whatsapp)
		{
			Debug.Print("Update_Member_Approved_Status " + Constants.RestUrl_Update_Member_Authorizations + "?userid=" + userid + "&assembleia=" + assembleia + "&regulamento=" + regulamento + "&tratamentodados=" + tratamentodados + "&registoimagem=" + registoimagem + "&fotosocio=" + fotosocio + "&whatsapp=" + whatsapp);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Update_Member_Authorizations + "?userid=" + userid + "&assembleia=" + assembleia + "&regulamento=" + regulamento + "&tratamentodados=" + tratamentodados + "&registoimagem=" + registoimagem + "&fotosocio=" + fotosocio + "&whatsapp=" + whatsapp, string.Empty));
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
					Debug.WriteLine("error updating member info");
					result = "-1";
				}

				return result;
			}
			catch
			{
				Debug.WriteLine("Update_Member_Approved_Status - http request error");
				return "-1";
			}
		}

		public async Task<string> Upload_Member_Photo(Stream stream)//Stream mfile, string fileName)
		{
			var url = Constants.RestUrl_Upload_Member_Photo;
			url += "?userid=" + App.member.id; //any parameters you want to send to the php page.
			Debug.Print("Upload_Member_Photo " + url);
			try
			{
				//HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(Constants.serverbase);
				MultipartFormDataContent form = new MultipartFormDataContent();
				StreamContent content = new StreamContent(stream);

				string fName = App.member.id + "_photo";

				content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
				{
					Name = "fileToUpload",
					FileName = fName
				};
				form.Add(content);
				var response = await client.PostAsync(url, form);
				var result = response.Content.ReadAsStringAsync().Result;

			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception Caught: " + e.ToString());
				return "-1";
			}
			return "0";
		}

		public async Task<string> Delete_Member_Photo(string memberid)
		{
			Debug.WriteLine("Delete_Member_Photo " + Constants.RestUrl_Delete_Member_Photo + "?userid=" + memberid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Delete_Member_Photo + "?userid=" + memberid, string.Empty));
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
					Debug.WriteLine("error deleting member photo");
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

		public async Task<int> Upload_Member_Document(Stream stream, string userid, string filename, string documentname, string status, string type, string startdate, string enddate)//Stream mfile, string fileName)
		{
			var url = Constants.RestUrl_Upload_Member_Document;
			url += "?userid=" + userid + "&filename=" + filename + "&documentname=" + documentname + "&status=" + status + "&type=" + type + "&startdate=" + startdate + "&enddate=" + enddate;
			Debug.Print("Upload_Member_Document " + url);
			try
			{
				//HttpClient client = new HttpClient(new Xamarin.Android.Net.AndroidClientHandler());
				client.BaseAddress = new Uri(Constants.serverbase);
				//client.BaseAddress = new Uri("http://adcpncrmazure.adcpn.pt/adcpn");
				MultipartFormDataContent form = new MultipartFormDataContent();

				StreamContent content = new StreamContent(stream);

				content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
				{
					Name = "fileToUpload",
					FileName = filename
				};
				form.Add(content);
				var response = await client.PostAsync(url, form);
				var result = response.Content.ReadAsStringAsync().Result;

			}
			catch (Exception e)
			{
				Debug.WriteLine("Exception Caught: " + e.ToString());
				return -1;
			}
			return 1;
		}

		public async Task<List<Document>> Get_Member_Documents(string userid)
		{
			Debug.WriteLine("Get_Member_Documents - " + Constants.RestUrl_Get_Member_Documents + "?userid=" + userid);
			Uri uri = new Uri(string.Format(Constants.RestUrl_Get_Member_Documents + "?userid=" + userid));
			try
			{
				HttpResponseMessage response = await client.GetAsync(uri);


				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					Debug.WriteLine("Get_Member_Documents - content = " + content);
					documents = JsonConvert.DeserializeObject<List<Document>>(content);

				}
				else
				{
					Debug.WriteLine("Get_Member_Documents - not ok");
				}
				return documents;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Get_Member_Documents - http request error");
				Debug.Print(e.StackTrace);
				return null;
			}

		}

        public async Task<string> createNewMember(Member member)
        {
            string query = Constants.RestUrl_Create_New_Member + "?nome=" + member.name
                + "&tipo_praticante=" + member.member_type + "&data_nascimento=" + member.birthdate + "&nif=" + member.nif
                + "&email=" + member.email + "&federado=" + member.number_fnkp;
            Debug.Print("createNewMember - " + query);
            Uri uri = new Uri(string.Format(query, string.Empty));
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
                    Debug.WriteLine("error updating member info");
                    result = "-3";
                }

                return result;
            }
            catch
            {
                Debug.WriteLine("http request error");
                return "-4";
            }
        }


    }
}