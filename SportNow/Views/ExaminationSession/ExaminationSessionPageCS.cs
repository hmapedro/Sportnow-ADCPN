using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using Xamarin.Essentials;
using System.Globalization;

namespace SportNow.Views
{
	public class ExaminationSessionPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{

			refreshExaminationStatus(examination_session.id);
		}

		protected override void OnDisappearing()
		{
			CleanScreen();
		}



		FormValue estadoValue = new FormValue("");



		private Examination_Session examination_session;

		private List<Payment> payments;

		Button registerButton;

		private Grid gridCompetiton;

		public void initLayout()
		{
			Title = examination_session.name;
		}


		public void CleanScreen()
		{
			if (gridCompetiton != null)
			{
				relativeLayout.Children.Remove(gridCompetiton);
				gridCompetiton = null;
			}
			if (registerButton != null)
			{
				relativeLayout.Children.Remove(registerButton);
				registerButton = null;
			}
		}
		public async void initSpecificLayout()
		{
			gridCompetiton = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter});
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = 120 * App.screenHeightAdapter});
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			//gridGeral.RowDefinitions.Add(new RowDefinition { Height = 1 });
			gridCompetiton.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			gridCompetiton.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label dateLabel = new FormLabel { Text = "DATA" };
			FormValue dateValue = new FormValue(examination_session.date);

			FormLabel placeLabel = new FormLabel { Text = "LOCAL" };
			FormValue placeValue = new FormValue(examination_session.place);

			FormLabel gradeLabel = new FormLabel { Text = "EXAME PARA" };
			FormValue gradeValue;
			if (examination_session.participationgrade != null)
			{
				gradeValue = new FormValue(Constants.grades[examination_session.participationgrade]);
			}
			else
			{
				gradeValue = new FormValue("-");
			}


			FormLabel valueLabel = new FormLabel { Text = "VALOR" };

			FormValue valueValue;
			if (examination_session.participationvalue != null)
			{
				Debug.Print("examination_session.participationvalue = " + examination_session.participationvalue+".");
				double participationvalue_double = Double.Parse(examination_session.participationvalue.Replace(',', '.'), CultureInfo.InvariantCulture);
				valueValue = new FormValue(String.Format("{0:0.00}", participationvalue_double + "€"));
			}
			else
			{
				valueValue = new FormValue("-");
			}


			FormLabel websiteLabel = new FormLabel { Text = "WEBSITE" };
			FormValue websiteValue = new FormValue(examination_session.website);

			websiteValue.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async () => {
					try
					{
						await Browser.OpenAsync(examination_session.website, BrowserLaunchMode.SystemPreferred);
					}
					catch (Exception ex)
					{
						// An unexpected error occured. No browser may be installed on the device.
					}
				})
			});

			FormLabel estadoLabel = new FormLabel { Text = "ESTADO" }; ;
			estadoValue = new FormValue("");

			List<Examination> examination_sessionCall = await GetExamination_SessionCall();

			DateTime currentTime = DateTime.Now.Date;

			DateTime registrationbegindate_datetime = new DateTime();
			DateTime registrationlimitdate_datetime = new DateTime();

			Debug.Print("examination_session.registrationbegindate = " + examination_session.registrationbegindate);

			if ((examination_session.registrationbegindate != "") & (examination_session.registrationbegindate != null))
			{
				registrationbegindate_datetime = DateTime.Parse(examination_session.registrationbegindate).Date;
			}
			if ((examination_session.registrationlimitdate != "") & (examination_session.registrationlimitdate != null))
			{
				registrationlimitdate_datetime = DateTime.Parse(examination_session.registrationlimitdate).Date;
			}

			bool registrationOpened = false;
			string limitDateLabelText = "";

			if (examination_session.registrationbegindate == "")
			{
				Debug.Print("Data início de inscrições ainda não está definida");
				limitDateLabelText = "As inscrições ainda não estão abertas.";
			}
			else if ((currentTime - registrationbegindate_datetime).Days < 0)
			{
				Debug.Print("Inscrições ainda não abriram");
				limitDateLabelText = "As inscrições abrem no dia " + examination_session.registrationbegindate + ".";
			}
			else
			{
				
				Debug.Print("Inscrições já abriram " + (registrationlimitdate_datetime - currentTime).Days);
				if ((registrationlimitdate_datetime - currentTime).Days <= 0)
				{
					Debug.Print("Inscrições já fecharam");
					limitDateLabelText = "Ohhh...As inscrições já terminaram.";
				}
				else
				{
					registrationOpened = true;
					Debug.Print("Inscrições estão abertas!");
					limitDateLabelText = "As inscrições estão abertas e terminam no dia " + examination_session.registrationlimitdate + ". Contamos contigo!";
				}
			}

			Debug.Print("examination_session.participationconfirmed = " + examination_session.participationconfirmed);

			bool hasCall = false;

			if (examination_sessionCall != null)
			{
				Debug.Print("examination_sessionCall is not null");
				if (examination_sessionCall.Count != 0)
				{
					Debug.Print("examination_sessionCall count is not 0 - " + examination_sessionCall.Count);
					hasCall = true;
				}
				else {
					Debug.Print("examination_sessionCall count is 0");
				}
			}
			else
			{
				Debug.Print("examination_sessionCall is null");
			}

			registerButton = new Button();


			if (examination_session.participationconfirmed == "confirmado")
			{
				estadoValue = new FormValue("INSCRITO");
				estadoValue.label.TextColor = Color.FromRgb(96, 182, 89);
				limitDateLabelText = "BOA SORTE!";
			}
			else if (examination_session.participationconfirmed == "convocado")
			{
				estadoValue = new FormValue("NÃO INSCRITO");
				estadoValue.label.TextColor = Color.Red;

				if (registrationOpened == true)
				{
					registerButton = new Button
					{
						Text = "INSCREVER",
						BackgroundColor = Color.FromRgb(96, 182, 89),
						TextColor = Color.White,
						WidthRequest = 100,
						HeightRequest = 50,
						VerticalOptions = LayoutOptions.End
					};

					Frame frame_registerButton = new Frame
					{
						BackgroundColor = Color.Transparent,
						//BorderColor = Color.FromRgb(96, 182, 89),
						WidthRequest = 100,
						HeightRequest = 50,
						CornerRadius = 10,
						IsClippedToBounds = true,
						Padding = 0
					};

					frame_registerButton.Content = registerButton;
					registerButton.Clicked += OnRegisterButtonClicked;

					gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
					gridCompetiton.Children.Add(frame_registerButton, 0, 8);
					Grid.SetColumnSpan(frame_registerButton, 2);
				}
			}
			else if (examination_session.participationconfirmed == null)
			{
				if (hasCall == false)
				{
					estadoValue = new FormValue("-");
				}
				
				else
				{
					estadoValue = new FormValue("NÃO CONVOCADO"); 
				}
				estadoValue.label.TextColor = Color.White;
			}

			Label limitDateLabel = new Label
			{
				Text = limitDateLabelText,
				TextColor = Color.FromRgb(246, 220, 178),
				WidthRequest = 200,
				HeightRequest = 30,
				FontSize = App.titleFontSize,
				HorizontalTextAlignment = TextAlignment.Center
			};


			Debug.Print("hasCall = " + hasCall);
			if (hasCall == false)
            {
				Label convocatoriaLabel = new Label
				{
					Text = "Ainda não existe Convocatória para esta Sessão de Exames.",
					TextColor = Color.White,
					FontSize = 20 * App.screenHeightAdapter,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center
				};
				gridCompetiton.Children.Add(convocatoriaLabel, 0, 7);
				Grid.SetColumnSpan(convocatoriaLabel, 2);
			}
			else
			{
				Image convocatoriaImage = new Image
				{
					Source = "iconconvocatoria.png",
					HorizontalOptions = LayoutOptions.Start,
					HeightRequest = 50 * App.screenHeightAdapter,
				};

				Label convocatoriaLabel = new Label
				{
					Text = "Convocatória",
					TextColor = Color.White,
					FontSize = App.titleFontSize,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start
				};

				StackLayout convocatoriaStackLayout = new StackLayout
				{
					//BackgroundColor = Color.Green,
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Spacing = 20 * App.screenHeightAdapter,
					Children =
					{
						convocatoriaImage,
						convocatoriaLabel
					}
				};
				gridCompetiton.Children.Add(convocatoriaStackLayout, 0, 7);
				Grid.SetColumnSpan(convocatoriaStackLayout, 2);

				var convocatoriaStackLayout_tap = new TapGestureRecognizer();
				convocatoriaStackLayout_tap.Tapped += (s, e) =>
				{
					Navigation.PushAsync(new ExaminationSessionCallPageCS(examination_session));
				};
				convocatoriaStackLayout.GestureRecognizers.Add(convocatoriaStackLayout_tap);
			}
			
			gridCompetiton.Children.Add(dateLabel, 0, 0);
			gridCompetiton.Children.Add(dateValue, 1, 0);

			gridCompetiton.Children.Add(placeLabel, 0, 1);
			gridCompetiton.Children.Add(placeValue, 1, 1);

			gridCompetiton.Children.Add(websiteLabel, 0, 2);
			gridCompetiton.Children.Add(websiteValue, 1, 2);

			gridCompetiton.Children.Add(gradeLabel, 0, 3);
			gridCompetiton.Children.Add(gradeValue, 1, 3);

			gridCompetiton.Children.Add(valueLabel, 0, 4);
			gridCompetiton.Children.Add(valueValue, 1, 4);

			gridCompetiton.Children.Add(estadoLabel, 0, 5);
			gridCompetiton.Children.Add(estadoValue, 1, 5);

			gridCompetiton.Children.Add(limitDateLabel, 0, 6);
			Grid.SetColumnSpan(limitDateLabel, 2);

			if (App.member.isExaminador == "1")
			{
				Image examinadorImage = new Image
				{
					Source = "iconexames.png",
					HorizontalOptions = LayoutOptions.Start,
					HeightRequest = 60 * App.screenHeightAdapter,
				};

				Label examinadorLabel = new Label
				{
					Text = "Examinador",
					TextColor = Color.White,
					FontSize = App.titleFontSize,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start
				};

				StackLayout examinadorStackLayout = new StackLayout
				{
					//BackgroundColor = Color.Green,
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Spacing = 20 * App.screenHeightAdapter,
					Children =
					{
						examinadorImage,
						examinadorLabel
					}
				};
				gridCompetiton.Children.Add(examinadorStackLayout, 0, 8);
				Grid.SetColumnSpan(examinadorStackLayout, 2);

				var examinadorStackLayout_tap = new TapGestureRecognizer();
				examinadorStackLayout_tap.Tapped += (s, e) =>
				{
					Navigation.PushAsync(new ExaminationEvaluationCallPageCS(examination_session));
				};
				examinadorStackLayout.GestureRecognizers.Add(examinadorStackLayout_tap);
			}
			
			
			relativeLayout.Children.Add(gridCompetiton,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 10); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height); // center of image (which is 40 wide)
				})
			);
		}



		public ExaminationSessionPageCS(Examination_Session examination_session)
		{
			this.examination_session = examination_session;
			Debug.Print("AQUI 0 examination_session.participationid = " + examination_session.participationid);
			this.initLayout();
			//this.initSpecificLayout();
		}

		public ExaminationSessionPageCS(string examination_session_id)
		{
			this.examination_session = new Examination_Session();
			examination_session.id = examination_session_id;
			//refreshExaminationStatus(examination_session_id);
			this.initLayout();
			//this.initSpecificLayout();
		}

		async void refreshExaminationStatus(string examination_session_id)
		{
			Debug.Print("AQUI 1 examination_session_id = " + examination_session_id);
			if (examination_session_id != null)
			{
				ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();
				examination_session = await examination_sessionManager.GetExamination_SessionByID(App.member.id, examination_session_id);
			}
			initSpecificLayout();
		}

		async void OnRegisterButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("OnRegisterButtonClicked");
			registerButton.IsEnabled = false;
			Debug.Print("examination_session.participationconfirmed" + examination_session.participationconfirmed);
			if (examination_session.participationconfirmed == "convocado")
			{
				Debug.Print("AQUI examination_session.participationid" + examination_session.participationid);
				await Navigation.PushAsync(new ExaminationSessionPaymentPageCS(examination_session));

			}

		}

		async Task<List<Examination>> GetExamination_SessionCall()
		{
			Debug.WriteLine("AQUI GetExamination_SessionCall");
			ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();

			List<Examination> examination_sessionCall_i = await examination_sessionManager.GetExamination_SessionCall(examination_session.id);
			if (examination_sessionCall_i == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return examination_sessionCall_i;
		}


	}
}

