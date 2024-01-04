using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Views.Profile;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
//Ausing Acr.UserDialogs;
using System.Text.RegularExpressions;
using System.Net;
using SportNow.Services.Camera;
using System.IO;
using SportNow.Views.CompleteRegistration;
using SkiaSharp;
using Xamarin.Essentials;

namespace SportNow.Views.CompleteRegistration
{
    public class CompleteRegistration_Profile_PageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{
			await UpdateMemberInfo(true);
		}

		private ScrollView scrollView;

		private Member member;

		MenuButton geralButton;
		MenuButton moradaButton;
		MenuButton encEducacaoButton;
		MenuButton infoEscolarButton;

		StackLayout stackButtons;
		private Grid gridGeral;
		private Grid gridMorada;
		private Grid gridEncEducacao;
		private Grid gridInfoEscolar;

		RoundImage memberPhotoImage;
		FormValue nameValue;
		FormValue emailValue;
		FormValue nifValue;
		FormValueEditDate birthdateValue;
		FormValueEdit cc_numberValue;
		FormValueEditPicker countryValue;
		FormValueEditPicker genderValue;
		FormValueEdit phoneValue;
        FormValueEditLongText addressValue;
		FormValueEdit cityValue;
		FormValueEditCodPostal postalcodeValue;
		FormValueEdit nameEmergencyContactValue;
		FormValueEdit phoneEmergencyContactValue;
		FormValueEdit EncEducacao1NomeValue;
		FormValueEdit EncEducacao1PhoneValue;
		FormValueEdit EncEducacao1MailValue;
		FormValueEdit EncEducacao2NomeValue;
		FormValueEdit EncEducacao2PhoneValue;
		FormValueEdit EncEducacao2MailValue;

		FormValueEdit schoolNameValue;
		FormValueEdit schoolNumberValue;
		FormValueEdit schoolYearValue;
		FormValueEdit schoolClassValue;

		FormValueEditLongText comentariosValue;

		bool enteringPage = true;

		Stream stream;


		public void initLayout()
		{
			Title = "COMPLETAR DADOS PESSOAIS";
		}


		public async void initSpecificLayout()
		{

			member = App.member;

			scrollView = new ScrollView { Orientation = ScrollOrientation.Vertical };

			relativeLayout.Children.Add(scrollView,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(270 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 340 * App.screenHeightAdapter; // center of image (which is 40 wide)
				})
			);


			int countStudents = App.original_member.students_count;

			CreatePhoto();
			CreateName();
			CreateStackButtons();
			CreateGridGeral();
			CreateGridMorada();
			CreateGridEncEducacao();
			CreateGridInfoEscolar();
			
			
			CreateGridButtons();

			OnGeralButtonClicked(null, null);

			RegisterButton confirmPersonalDataButton = new RegisterButton("CONTINUAR", 100, 50);
			confirmPersonalDataButton.button.Clicked += confirmPersonalDataButtonClicked;

			relativeLayout.Children.Add(confirmPersonalDataButton,
				xConstraint: Constraint.Constant(10),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (60 * App.screenHeightAdapter); // 
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 20 * App.screenHeightAdapter); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant((50 * App.screenHeightAdapter))
			);
		}

		public void CreateName()
		{
			Label nameLabel = new Label
			{
				Text = member.nickname,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.WordWrap,
				FontSize = App.bigTitleFontSize,
				TextTransform = TextTransform.Uppercase
			};
			relativeLayout.Children.Add(nameLabel,
				xConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(160 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 20 * App.screenHeightAdapter); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant((70 * App.screenHeightAdapter))
			);

		}

			public void CreatePhoto()
		{

			memberPhotoImage = new RoundImage();

			if (App.member.consentimento_fotosocio == "1")
			{
				WebResponse response;
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Constants.images_URL + App.member.id + "_photo");
			
				request.Method = "HEAD";
				bool exists;
				try
				{
					response = request.GetResponse();
					Debug.Print("response.Headers.GetType()= " + response.Headers.GetType());
					exists = true;
				}
				catch (Exception ex)
				{
					exists = false;
				}

				Debug.Print("Photo exists? = " + exists);

				if (exists)
				{
					memberPhotoImage.Source = new UriImageSource
					{
						Uri = new Uri(Constants.images_URL + App.member.id + "_photo"),
						CachingEnabled = false,
						CacheValidity = new TimeSpan(0, 0, 0, 1)
					};
			
				}
				else
				{
					memberPhotoImage.Source = "iconadicionarfoto.png";
				}

				var memberPhotoImage_tap = new TapGestureRecognizer();
				memberPhotoImage_tap.Tapped += memberPhotoImageTappedAsync;
				memberPhotoImage.GestureRecognizers.Add(memberPhotoImage_tap);

				/*relativeLayout.Children.Add(memberPhotoImage,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 2) - (80 * App.screenHeightAdapter);
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(160 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(160 * App.screenHeightAdapter) // size of screen -80
				);*/
			}
			else
			{
				memberPhotoImage.Source = "iconadicionarfoto.png";

				var memberPhotoImage_tap = new TapGestureRecognizer();
				memberPhotoImage_tap.Tapped += memberPhotoImageTapped_NotAuthorized_Async;
				memberPhotoImage.GestureRecognizers.Add(memberPhotoImage_tap);

			}

			relativeLayout.Children.Add(memberPhotoImage,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 2) - (80 * App.screenHeightAdapter);
				}),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.Constant(180 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(180 * App.screenHeightAdapter) // size of screen -80
			);

		}

		public CompleteRegistration_Profile_PageCS()
		{
			this.initLayout();
			this.initSpecificLayout();

			/*MessagingCenter.Subscribe<byte[]>(this, "memberPhoto", (args) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					showActivityIndicator();
					streamSource = new MemoryStream(args);
					memberPhotoImage.photo.Source = ImageSource.FromStream(() => new MemoryStream(args));


					MemberManager memberManager = new MemberManager();
					memberManager.Upload_Member_Photo(streamSource);
                    hideActivityIndicator();
                    //AUserDialogs.Instance.HideLoading();   //Hide loader
                });

			});*/

		}

		public void CreateStackButtons() {
			var width = Constants.ScreenWidth;
			Debug.WriteLine("Constants.ScreenWidth " + width);
			var buttonWidth = (width - (50 * App.screenWidthAdapter)) / 4;

			geralButton = new MenuButton("GERAL", buttonWidth, 40 * App.screenHeightAdapter);
			geralButton.button.Clicked += OnGeralButtonClicked;
			moradaButton = new MenuButton("MORADA", buttonWidth, 40 * App.screenHeightAdapter);
			moradaButton.button.Clicked += OnMoradaButtonClicked;
			encEducacaoButton = new MenuButton("ENC\nEDUCAÇÃO",buttonWidth, 40 * App.screenHeightAdapter);
			encEducacaoButton.button.Clicked += OnEncEducacaoButtonClicked;
			infoEscolarButton = new MenuButton("INFO\nESCOLAR", buttonWidth, 40 * App.screenHeightAdapter);
			infoEscolarButton.button.Clicked += OnInfoEscolarButtonClicked;

			stackButtons = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Children =
				{
					geralButton,
					moradaButton,
					encEducacaoButton,
					infoEscolarButton
				}
			};

			relativeLayout.Children.Add(stackButtons,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(220 * App.screenHeightAdapter),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter) // size of screen -80
			);

			geralButton.activate();
			infoEscolarButton.deactivate();
			moradaButton.deactivate();
			encEducacaoButton.deactivate();
		}

		public void CreateGridButtons()
		{

			Image changePasswordImage = new Image
			{
				Source = "botaoalterarpass.png",
				Aspect = Aspect.AspectFit
			};

			TapGestureRecognizer changePasswordImage_tapEvent = new TapGestureRecognizer();
			changePasswordImage_tapEvent.Tapped += OnChangePasswordButtonClicked;
			changePasswordImage.GestureRecognizers.Add(changePasswordImage_tapEvent);

			relativeLayout.Children.Add(changePasswordImage,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 50);
				}),
				yConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(40 * App.screenHeightAdapter), // size of screen -80
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter) // size of screen -80
			);

			//gridButtons.Children.Add(changePasswordImage, 0, 0);
		}

		public void CreateGridGeral()
		{

			gridGeral = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			//gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			//gridGeral.RowDefinitions.Add(new RowDefinition { Height = 1 });
			gridGeral.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			gridGeral.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			/*FormLabel nameLabel = new FormLabel { Text = "NOME", HorizontalTextAlignment = TextAlignment.Start };
			nameValue = new FormValue(member.name);*/

			FormLabel nifLabel = new FormLabel { Text = "NIF" };
			nifValue = new FormValue(member.nif);

			FormLabel birthdateLabel = new FormLabel { Text = "NASCIMENTO" };
			birthdateValue = new FormValueEditDate(member.birthdate);//?.ToString("yyyy-MM-dd"));

			FormLabel cc_numberLabel = new FormLabel { Text = "CC" };
			cc_numberValue = new FormValueEdit(member.cc_number, Keyboard.Numeric);

			List<string> gendersList = new List<string>();
			foreach (KeyValuePair<string, string> entry in Constants.genders)
			{
				gendersList.Add(entry.Value);
			}

			FormLabel genderLabel = new FormLabel { Text = "GÉNERO" };
			genderValue = new FormValueEditPicker(Constants.genders[member.gender], gendersList);


			List<string> countriesList = new List<string>();
			foreach (KeyValuePair<string, string> entry in Constants.countries)
			{
				countriesList.Add(entry.Value);
			}
			FormLabel countryLabel = new FormLabel { Text = "NACIONALIDADE" };
			countryValue = new FormValueEditPicker(Constants.countries[member.country], countriesList);

			FormLabel phoneLabel = new FormLabel { Text = "TELEFONE" };
			phoneValue = new FormValueEdit(member.phone, Keyboard.Telephone);

			FormLabel comentariosLabel = new FormLabel { Text = "COMENTÁRIOS" };
			comentariosValue = new FormValueEditLongText(member.comentarios, Keyboard.Text);
			comentariosValue.MinimumHeightRequest = 80 * App.screenHeightAdapter;
			comentariosValue.HeightRequest = 80 * App.screenHeightAdapter;


			/*FormLabel emailLabel = new FormLabel { Text = "EMAIL" };
			emailValue = new FormValue(member.email);*/

			/*gridGeral.Children.Add(nameLabel, 0, 0);
			gridGeral.Children.Add(nameValue, 1, 0);*/

			gridGeral.Children.Add(nifLabel, 0, 0);
			gridGeral.Children.Add(nifValue, 1, 0);

			gridGeral.Children.Add(cc_numberLabel, 0, 1);
			gridGeral.Children.Add(cc_numberValue, 1, 1);

			gridGeral.Children.Add(birthdateLabel, 0, 2);
			gridGeral.Children.Add(birthdateValue, 1, 2);

			gridGeral.Children.Add(genderLabel, 0, 3);
			gridGeral.Children.Add(genderValue, 1, 3);

			gridGeral.Children.Add(countryLabel, 0, 4);
			gridGeral.Children.Add(countryValue, 1, 4);

			gridGeral.Children.Add(phoneLabel, 0, 5);
			gridGeral.Children.Add(phoneValue, 1, 5);


			comentariosValue.entry.AutoSize = EditorAutoSizeOption.Disabled;
			comentariosValue.entry.Focused += editor_Focused;


			gridGeral.Children.Add(comentariosLabel, 0, 6);
			gridGeral.Children.Add(comentariosValue, 1, 6);

			/*gridGeral.Children.Add(emailLabel, 0, 7);
			gridGeral.Children.Add(emailValue, 1, 7);*/
		}

		async void editor_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
		{
			if (Device.RuntimePlatform == Device.iOS)
			{
				Debug.Print("scrollView.ScrollY = " + scrollView.ScrollY);
				scrollView.ScrollToAsync(scrollView.ScrollX, 300, true);
			}
			Debug.Print("scrollView.ScrollY after = " + scrollView.ScrollY);
		}

		public void CreateGridInfoEscolar()
		{

			gridInfoEscolar = new Grid { Padding = 10 };
			gridInfoEscolar.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInfoEscolar.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInfoEscolar.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInfoEscolar.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInfoEscolar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			gridInfoEscolar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			FormLabel schoolNameLabel = new FormLabel { Text = "Escola" };
			schoolNameValue = new FormValueEdit(member.schoolname);

			FormLabel schoolNumberLabel = new FormLabel { Text = "Número" };
			schoolNumberValue = new FormValueEdit(member.schoolnumber);


			FormLabel schoolYearLabel = new FormLabel { Text = "Ano" };
			schoolYearValue = new FormValueEdit(member.schoolyear);

			FormLabel schoolClassLabel = new FormLabel { Text = "Turma" };
			schoolClassValue = new FormValueEdit(member.schoolclass);


			gridInfoEscolar.Children.Add(schoolNameLabel, 0, 0);
			gridInfoEscolar.Children.Add(schoolNameValue, 1, 0);

			gridInfoEscolar.Children.Add(schoolNumberLabel, 0, 1);
			gridInfoEscolar.Children.Add(schoolNumberValue, 1, 1);

			gridInfoEscolar.Children.Add(schoolYearLabel, 0, 2);
			gridInfoEscolar.Children.Add(schoolYearValue, 1, 2);

			gridInfoEscolar.Children.Add(schoolClassLabel, 0, 3);
			gridInfoEscolar.Children.Add(schoolClassValue, 1, 3);

		}

		public void CreateGridMorada()
		{

			gridMorada = new Grid { Padding = 10 };
			gridMorada.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMorada.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMorada.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMorada.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			gridMorada.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

            FormLabel addressLabel = new FormLabel { Text = "ENDEREÇO" };
			addressValue = new FormValueEditLongText(member.address, Keyboard.Text);
            addressValue.MinimumHeightRequest = 60 * App.screenHeightAdapter;
            addressValue.HeightRequest = 60 * App.screenHeightAdapter;

            addressValue.entry.AutoSize = EditorAutoSizeOption.Disabled;
            addressValue.entry.Focused += editor_Focused;

            FormLabel postalcodeLabel = new FormLabel { Text = "CÓD POSTAL" };
			postalcodeValue = new FormValueEditCodPostal(member.postalcode);

			FormLabel cityLabel = new FormLabel { Text = "LOCALIDADE" };
			cityValue = new FormValueEdit(member.city);


			/*gridMorada.Children.Add(phoneLabel, 0, 1);
			gridMorada.Children.Add(phoneValue, 1, 1);*/

			gridMorada.Children.Add(addressLabel, 0, 0);
			gridMorada.Children.Add(addressValue, 1, 0);

			gridMorada.Children.Add(postalcodeLabel, 0, 1);
			gridMorada.Children.Add(postalcodeValue, 1, 1);

			gridMorada.Children.Add(cityLabel, 0, 2);
			gridMorada.Children.Add(cityValue, 1, 2);
		}

		public void CreateGridEncEducacao()
		{

			gridEncEducacao = new Grid { Padding = 10 };
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); 
			gridEncEducacao.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

			FormLabel emergencyContactLabel = new FormLabel { Text = "CONTACTO EMERGÊNCIA", FontSize = App.itemTitleFontSize };

			FormLabel nameEmergencyContactLabel = new FormLabel { Text = "NOME" };
			nameEmergencyContactValue = new FormValueEdit(member.nameEmergencyContact);

			FormLabel phoneEmergencyContactLabel = new FormLabel { Text = "TELEFONE" };
			phoneEmergencyContactValue = new FormValueEdit(member.phoneEmergencyContact, Keyboard.Telephone);

			FormLabel EncEducacao1Label = new FormLabel { Text = "ENCARREGADO DE EDUCACAO 1", FontSize = App.itemTitleFontSize };

			FormLabel EncEducacao1NomeLabel = new FormLabel { Text = "NOME" };
			EncEducacao1NomeValue = new FormValueEdit(member.name_enc1);

			FormLabel EncEducacao1PhoneLabel = new FormLabel { Text = "TELEFONE" };
			EncEducacao1PhoneValue = new FormValueEdit(member.phone_enc1, Keyboard.Telephone);

			FormLabel EncEducacao1MailLabel = new FormLabel { Text = "MAIL" };
			EncEducacao1MailValue = new FormValueEdit(member.mail_enc1, Keyboard.Email);

			FormLabel EncEducacao2Label = new FormLabel { Text = "ENCARREGADO DE EDUCACAO 2", FontSize = App.itemTitleFontSize };

			FormLabel EncEducacao2NomeLabel = new FormLabel { Text = "NOME" };
			EncEducacao2NomeValue = new FormValueEdit(member.name_enc2);

			FormLabel EncEducacao2PhoneLabel = new FormLabel { Text = "TELEFONE" };
			EncEducacao2PhoneValue = new FormValueEdit(member.phone_enc2, Keyboard.Telephone);

			FormLabel EncEducacao2MailLabel = new FormLabel { Text = "MAIL" };
			EncEducacao2MailValue = new FormValueEdit(member.mail_enc2, Keyboard.Email);


			gridEncEducacao.Children.Add(emergencyContactLabel, 0, 0);
			Grid.SetColumnSpan(emergencyContactLabel, 2);

			gridEncEducacao.Children.Add(nameEmergencyContactLabel, 0, 1);
			gridEncEducacao.Children.Add(nameEmergencyContactValue, 1, 1);

			gridEncEducacao.Children.Add(phoneEmergencyContactLabel, 0, 2);
			gridEncEducacao.Children.Add(phoneEmergencyContactValue, 1, 2);


			gridEncEducacao.Children.Add(EncEducacao1Label, 0, 3);
			Grid.SetColumnSpan(EncEducacao1Label, 2);

			gridEncEducacao.Children.Add(EncEducacao1NomeLabel, 0, 4);
			gridEncEducacao.Children.Add(EncEducacao1NomeValue, 1, 4);

			gridEncEducacao.Children.Add(EncEducacao1PhoneLabel, 0, 5);
			gridEncEducacao.Children.Add(EncEducacao1PhoneValue, 1, 5);

			gridEncEducacao.Children.Add(EncEducacao1MailLabel, 0, 6);
			gridEncEducacao.Children.Add(EncEducacao1MailValue, 1, 6);

			gridEncEducacao.Children.Add(EncEducacao2Label, 0, 7);
			Grid.SetColumnSpan(EncEducacao2Label, 2);

			gridEncEducacao.Children.Add(EncEducacao2NomeLabel, 0, 8);
			gridEncEducacao.Children.Add(EncEducacao2NomeValue, 1, 8);

			gridEncEducacao.Children.Add(EncEducacao2PhoneLabel, 0, 9);
			gridEncEducacao.Children.Add(EncEducacao2PhoneValue, 1, 9);

			gridEncEducacao.Children.Add(EncEducacao2MailLabel, 0, 10);
			gridEncEducacao.Children.Add(EncEducacao2MailValue, 1, 10);
		}


		async void OnGeralButtonClicked(object sender, EventArgs e)
		{
			geralButton.activate();
			infoEscolarButton.deactivate();
			moradaButton.deactivate();
			encEducacaoButton.deactivate();

			scrollView.Content = gridGeral;

			if (enteringPage == false) {
				await UpdateMemberInfo(false);
				enteringPage = false;
			}
			

		}

		async void OnMoradaButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnMoradaButtonClicked");

			geralButton.deactivate();
			moradaButton.activate();
			encEducacaoButton.deactivate();
			infoEscolarButton.deactivate();

			scrollView.Content = gridMorada;

			await UpdateMemberInfo(false);
		}

		async void OnEncEducacaoButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnEncEducacaoButtonClicked");

			geralButton.deactivate();
			moradaButton.deactivate();
			encEducacaoButton.activate();
			infoEscolarButton.deactivate();

			scrollView.Content = gridEncEducacao;

			await UpdateMemberInfo(false);
		}


		async void OnInfoEscolarButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnIdentificacaoButtonClicked");
			geralButton.deactivate();
			moradaButton.deactivate();
			encEducacaoButton.deactivate();
			infoEscolarButton.activate();

			scrollView.Content = gridInfoEscolar;

			await UpdateMemberInfo(false);
		}

		async void OnLogoutButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnLogoutButtonClicked");

			Application.Current.Properties.Remove("EMAIL");
			Application.Current.Properties.Remove("PASSWORD");
			Application.Current.Properties.Remove("SELECTEDUSER");

			App.member = null;
			App.members = null;

			Application.Current.SavePropertiesAsync();


			Application.Current.MainPage = new NavigationPage(new LoginPageCS(""))
			{
				BarBackgroundColor = Color.White,
				BarTextColor = Color.Black
			};
		}

		async void OnChangePasswordButtonClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ChangePasswordPageCS(member));
		}


		async Task<string> UpdateMemberInfo(bool validate)
		{
			Debug.Print("UpdateMemberInfo");
			if (App.member != null)
			{
				if (string.IsNullOrEmpty(postalcodeValue.entry.Text))
				{
					postalcodeValue.entry.Text = "";
				}

                if (cc_numberValue.entry.Text == "")
                {
                    await DisplayAlert("DADOS INVÁLIDOS", "O número de identificação (CC) introduzido não é válido.", "OK");
                    //OnGeralButtonClicked(null, null);
                    return "-1";
                }

                if (!Regex.IsMatch(birthdateValue.entry.Text, @"^\d{4}$|^\d{4}-((0?\d)|(1[012]))-(((0?|[12])\d)|3[01])$"))
                {
                    await DisplayAlert("DADOS INVÁLIDOS", "A data de nascimento introduzida não é válida.", "Ok");
                    return "-1";
                }
                else
                {
                    if ((DateTime.Parse(birthdateValue.entry.Text) - DateTime.Now).Days > 0)
                    {
                        await DisplayAlert("DADOS INVÁLIDOS", "A data de nascimento introduzida não é válida.", "Ok");
                        return "-1";
                    }
                }

                if (genderValue.picker.SelectedItem.ToString() == "Não Definido")
                {
                    await DisplayAlert("DADOS INVÁLIDOS", "Defina por favor o Género.", "OK");
                    return "-1";
                }

                /*if (nameValue.entry.Text == "")
				{
					nameValue.entry.Text = App.member.name;
					UserDialogs.Instance.Alert(new AlertConfig() { Title = "DADOS INVÁLIDOS", Message = "O nome introduzido não é válido.", OkText = "Ok" });
					return "-1";
				}*/
                if ((phoneValue.entry.Text != "") & (phoneValue.entry.Text != null))
				{
					if ((phoneValue.entry.Text.Length > 0) & (phoneValue.entry.Text.Length < 9))
					{
                        await DisplayAlert("DADOS INVÁLIDOS", "O telefone introduzido tem de ter pelo menos 9 dígitos.", "OK");
						OnGeralButtonClicked(null, null);
						return "-1";
					}
					else if (!Regex.IsMatch(phoneValue.entry.Text, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$"))
					{
						phoneValue.entry.Text = App.member.phone;
						OnGeralButtonClicked(null, null);
                        await DisplayAlert("DADOS INVÁLIDOS", "O telefone introduzido não é válido.", "OK");
                        //UserDialogs.Instance.Alert(new AlertConfig() { Title = "DADOS INVÁLIDOS", Message = "O telefone introduzido não é válido.", OkText = "Ok" });
                        return "-1";
					}
				}

                if ((postalcodeValue.entry.Text == "") | (addressValue.entry.Text == "") | (cityValue.entry.Text == ""))
                {

                    await DisplayAlert("DADOS INVÁLIDOS", "A morada não pode ser vazia.", "OK");
                    //OnMoradaButtonClicked(null, null);
                    return "-1";
                }

                if ((postalcodeValue.entry.Text != "") & (!Regex.IsMatch((postalcodeValue.entry.Text), @"^\d{4}-\d{3}$")))
                {
                    await DisplayAlert("DADOS INVÁLIDOS", "O código postal introduzido não é válido.", "OK");
                    //OnMoradaButtonClicked(null, null);
                    return "-1";
                }

                //App.member.name = nameValue.entry.Text;
                //App.member.email = emailValue.label.Text;
                App.member.birthdate = birthdateValue.entry.Text; //DateTime.Parse(birthdateValue.entry.Text);
				//App.member.nif = nifValue.entry.Text;
				App.member.cc_number = cc_numberValue.entry.Text;
				App.member.country = Constants.KeyByValue(Constants.countries, countryValue.picker.SelectedItem.ToString());
				App.member.gender = Constants.KeyByValue(Constants.genders, genderValue.picker.SelectedItem.ToString());
				App.member.phone = phoneValue.entry.Text;
				App.member.address = addressValue.entry.Text;
				App.member.city = cityValue.entry.Text;
				App.member.postalcode = postalcodeValue.entry.Text;
				App.member.nameEmergencyContact = nameEmergencyContactValue.entry.Text;
				App.member.phoneEmergencyContact = phoneEmergencyContactValue.entry.Text;
				App.member.name_enc1 = EncEducacao1NomeValue.entry.Text;
				App.member.phone_enc1 = EncEducacao1PhoneValue.entry.Text;
				App.member.mail_enc1 = EncEducacao1MailValue.entry.Text;
				App.member.name_enc2 = EncEducacao2NomeValue.entry.Text;
				App.member.phone_enc2 = EncEducacao2PhoneValue.entry.Text;
				App.member.mail_enc2 = EncEducacao2MailValue.entry.Text;

				App.member.schoolname = schoolNameValue.entry.Text;
				App.member.schoolnumber = schoolNumberValue.entry.Text;
				App.member.schoolyear = schoolYearValue.entry.Text;
				App.member.schoolclass = schoolClassValue.entry.Text;

				App.member.comentarios = comentariosValue.entry.Text;

				MemberManager memberManager = new MemberManager();
				var result = await memberManager.UpdateMemberInfo(App.original_member.id, App.member);
				if (result == "-1")
				{
					Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
					{
						BarBackgroundColor = Color.FromRgb(15, 15, 15),
						BarTextColor = Color.White
					};
					return "-1";
				}
				return result;
			}
			return "";
		}

		void memberPhotoImageTappedAsync(object sender, System.EventArgs e)
		{
			displayMemberPhotoImageActionSheet();
		}

		async void memberPhotoImageTapped_NotAuthorized_Async(object sender, System.EventArgs e)
		{
			bool display_result = await DisplayAlert("Consentimento Foto Sócio", "Para poder fazer upload da sua foto, tem de dar o seu consentimento para que possamos fazer o tratamento da mesma. Pretende dar consentimento?", "Sim", "Não");
			if (display_result == true)
			{
				showActivityIndicator();
				MemberManager memberManager = new MemberManager();
				App.member.consentimento_fotosocio = "1";

				relativeLayout.Children.Remove(memberPhotoImage);
				CreatePhoto();

				var result = await memberManager.Update_Member_Authorizations(App.member.id, App.member.consentimento_assembleia, App.member.consentimento_regulamento, App.member.consentimento_dados, App.member.consentimento_imagem, App.member.consentimento_fotosocio, App.member.consentimento_whatsapp);
                hideActivityIndicator();
            }
		}

		async Task<string> displayMemberPhotoImageActionSheet()
		{
			var actionSheet = await DisplayActionSheet("Fotografia Sócio " + App.member.nickname, "Cancel", null, "Tirar Foto", "Galeria de Imagens");

			MemberManager memberManager = new MemberManager();
			string result = "";
			switch (actionSheet)
			{
				case "Cancel":
					break;
				case "Tirar Foto":
					TakeAPhotoTapped();
					break;
				case "Galeria de Imagens":
					OpenGalleryTapped();
					break;
			}

			/*Device.BeginInvokeOnMainThread(() =>
			{
				var fileName = SetImageFileName();
				DependencyService.Get<CameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
			});*/

			return "";
		}

        async void OpenGalleryTapped()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Por favor escolha uma foto"
            });

            if (result != null)
            {
                Stream stream_aux = await result.OpenReadAsync();
                Stream localstream = await result.OpenReadAsync();

                memberPhotoImage.Source = ImageSource.FromStream(() => localstream);
                if (Device.RuntimePlatform == Device.iOS)
                {
                    memberPhotoImage.Rotation = 0;
                    stream = RotateBitmap(stream_aux, 0);
                }
                else
                {
                    memberPhotoImage.Rotation = 90;
                    stream = RotateBitmap(stream_aux, 90);
                }

                MemberManager memberManager = new MemberManager();
                memberManager.Upload_Member_Photo(stream);
            }
        }

        async void TakeAPhotoTapped()
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                Stream stream_aux = await result.OpenReadAsync();
                Stream localstream = await result.OpenReadAsync();

                memberPhotoImage.Source = ImageSource.FromStream(() => localstream);
                memberPhotoImage.Rotation = 90;
                stream = RotateBitmap(stream_aux, 90);

                MemberManager memberManager = new MemberManager();
                memberManager.Upload_Member_Photo(stream);
            }

        }

        public Stream RotateBitmap(Stream _stream, int angle)
        {
            Stream streamlocal = null;
            SKBitmap bitmap = SKBitmap.Decode(_stream);
            SKBitmap rotatedBitmap = new SKBitmap(bitmap.Height, bitmap.Width);
            if (angle != 0)
            {
                using (var surface = new SKCanvas(rotatedBitmap))
                {
                    surface.Translate(rotatedBitmap.Width, 0);
                    surface.RotateDegrees(angle);
                    surface.DrawBitmap(bitmap, 0, 0);
                }
            }
            else
            {
                rotatedBitmap = bitmap;

                /*using (var surface = new SKCanvas(rotatedBitmap))
                {
                    surface.Translate(rotatedBitmap.Width, 0);
                    surface.DrawBitmap(bitmap, 0, 0);
                }*/
            }

            using (MemoryStream memStream = new MemoryStream())
            using (SKManagedWStream wstream = new SKManagedWStream(memStream))
            {
                rotatedBitmap.Encode(wstream, SKEncodedImageFormat.Jpeg, 40);
                byte[] data = memStream.ToArray();
                streamlocal = new MemoryStream(data);
            }
            return streamlocal;

        }

        /*void TakeAPhotoTapped()
		{

			Device.BeginInvokeOnMainThread(() =>
			{
				var fileName = "memberPhoto";//SetImageFileName();
				Debug.Print("fileName TakeAPhotoTapped = " + fileName);
				DependencyService.Get<CameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
			});
		}

		void OpenGalleryTapped()
		{
			Debug.Print("OpenGalleryTapped");
			Device.BeginInvokeOnMainThread(() =>
			{
				var fileName = "memberPhoto";//SetImageFileName();
				Debug.Print("fileName OpenGalleryTapped = " + fileName);
				try
				{
					CameraInterface cameraInterface = DependencyService.Get<CameraInterface>();
					//cameraInterface = new DependencyService.Get<CameraInterface>();
					cameraInterface.LaunchGallery(FileFormatEnum.JPEG, fileName);
					//DependencyService.Get<CameraInterface>().LaunchGallery(FileFormatEnum.JPEG, "teste.jpeg");
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.Print(ex.Message);
					System.Diagnostics.Debug.Print(ex.StackTrace);
				}
				//loadedDocument.Source = fileName;
			});
		}

		private string SetImageFileName(string fileName = null)
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				if (fileName != null)
					App.ImageIdToSave = fileName;
				else
					App.ImageIdToSave = App.DefaultImageId;

				return App.ImageIdToSave;
			}
			else
			{
				if (fileName != null)
				{
					App.ImageIdToSave = fileName;
					return fileName;
				}
				else
					return null;
			}
		}*/

        async void confirmPersonalDataButtonClicked(object sender, EventArgs e)
		{

            string result = await UpdateMemberInfo(true);
			if (result!="-1")
			{
                await Navigation.PushAsync(new CompleteRegistration_Payment_PageCS());
            }
  
		}
	}

}