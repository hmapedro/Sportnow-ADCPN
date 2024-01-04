using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;

namespace SportNow.Views
{
	public class ExaminationSessionCallPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			//competition_participation = App.competition_participation;
			initSpecificLayout();

		}

		protected override void OnDisappearing()
		{
			//relativeLayout = null;
			collectionViewExaminationSessionCall = null;	
			registerButton = null;
		}



		private CollectionView collectionViewExaminationSessionCall;

		private Examination_Session examination_session;

		private List<Examination> examination_sessionCall;

		Button registerButton;

		Label examinationSessionNameLabel;
		Label nameTitleLabel;
		Label categoryTitleLabel;

		public void initLayout()
		{
			Title = "CONVOCATÓRIA";
		}


		public async void initSpecificLayout()
		{
			examination_sessionCall = await GetExamination_SessionCall();

			CreateExamination_SessionCallColletionView();

		}

		
		public void CreateExamination_SessionCallColletionView()
		{

			foreach (Examination examination in examination_sessionCall)
			{
				Debug.Print("examination.estado=" + examination.estado);
				if (examination.estado == "confirmado")
				{
					examination.estadoTextColor = Color.FromRgb(96, 182, 89) ;
				}
				examination.gradeLabel = Constants.grades[examination.grade];
			}

			examinationSessionNameLabel = new Label
			{
				Text = examination_session.name,
				BackgroundColor = Color.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 25,
				TextColor = Color.White
			};

			relativeLayout.Children.Add(examinationSessionNameLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 60; // 
				})
			);

			if (examination_sessionCall.Count > 0)
            {
				nameTitleLabel = new Label
				{
					Text = "NOME",
					BackgroundColor = Color.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
					FontSize = 22,
					TextColor = Color.FromRgb(246, 220, 178),
					LineBreakMode = LineBreakMode.WordWrap
				};

				relativeLayout.Children.Add(nameTitleLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(50),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 3 * 2) - 10; // center of image (which is 40 wide)
				}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 40; // 
				})
				);

				categoryTitleLabel = new Label
				{
					Text = "EXAME PARA",
					BackgroundColor = Color.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
					FontSize = 22,
					TextColor = Color.FromRgb(246, 220, 178),
					LineBreakMode = LineBreakMode.WordWrap
				};

				relativeLayout.Children.Add(categoryTitleLabel,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 3 * 2); // center of image (which is 40 wide)
				}),
					yConstraint: Constraint.Constant(50),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 3) - 10; // center of image (which is 40 wide)
				}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 40; // 
				})
				);
			}

			collectionViewExaminationSessionCall = new CollectionView
			{
				SelectionMode = SelectionMode.None,
				ItemsSource = examination_sessionCall,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
			{
				new Label { Text = "Ainda não foi criada convocatória para esta Sessão de Exames.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.Red, FontSize = 20 },
			}
					}
				}
			};

			//collectionViewCompetitionCall.SelectionChanged += OnCollectionViewProximasCompeticoesSelectionChanged;

			collectionViewExaminationSessionCall.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					Margin = new Thickness(3)
				};

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.formValueFontSize, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "membername");
				nameLabel.SetBinding(Label.TextColorProperty, "estadoTextColor");

				Frame nameFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Color.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0)
				};
				nameFrame.Content = nameLabel;

				itemRelativeLayout.Children.Add(nameFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 3 * 2) - 10; // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 40; // 
					}));

				Label categoryLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.formValueFontSize, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				categoryLabel.SetBinding(Label.TextProperty, "gradeLabel");



				Frame categoryFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Color.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0)
				};
				categoryFrame.Content = categoryLabel;

				itemRelativeLayout.Children.Add(categoryFrame,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 3 * 2) ; // center of image (which is 40 wide)
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 3) - 10; // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 40; // 
					}));


				return itemRelativeLayout;
			});

			DateTime currentTime = DateTime.Now.Date;
			DateTime registrationbegindate_datetime = DateTime.Parse(examination_session.registrationbegindate).Date;
			DateTime registrationlimitdate_datetime = DateTime.Parse(examination_session.registrationlimitdate).Date;
			Debug.Print("event_v.registrationbegindate = " + examination_session.registrationbegindate + " " + registrationbegindate_datetime);
			Debug.Print("event_v.registrationlimitdate = " + examination_session.registrationlimitdate + " " + registrationlimitdate_datetime);
			Debug.Print("registrationlimitdate_datetime - currentTime).Days = "+ (registrationlimitdate_datetime - currentTime).Days);
			bool registrationOpened = false;

			if (((currentTime - registrationbegindate_datetime).Days >= 0) & ((registrationlimitdate_datetime - currentTime).Days >= 0))
			{
				registrationOpened = true;
			}

			//Já inscrito ou não convocado
			if ((examination_session.participationconfirmed == "confirmado") | (examination_session.participationconfirmed == null) | (registrationOpened == false))	
			{
					relativeLayout.Children.Add(collectionViewExaminationSessionCall,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(100),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height) - 100; // 
					})
				);
			}
			else {
				relativeLayout.Children.Add(collectionViewExaminationSessionCall,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(100),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width); // center of image (which is 40 wide)
								}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height) - 170; // 
					})
				);

				registerButton = new Button
				{
					Text = "INSCREVER",
					BackgroundColor = Color.FromRgb(96, 182, 89),
					TextColor = Color.White,
					FontSize = App.itemTitleFontSize,
					WidthRequest = 100,
					HeightRequest = 50
				};

				Frame frame_registerButton = new Frame
				{
					BorderColor = Color.FromRgb(96, 182, 89),
					WidthRequest = 100,
					HeightRequest = 50,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = 0
				};

				frame_registerButton.Content = registerButton;
				registerButton.Clicked += OnRegisterButtonClicked;

				relativeLayout.Children.Add(frame_registerButton,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height) - 60; // 
					}),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant(50)
				);

			}

		}

		public ExaminationSessionCallPageCS(Examination_Session examination_session)
		{
			this.BackgroundColor = Color.FromRgb(25, 25, 25);
			this.examination_session = examination_session;
			this.initLayout();
			//this.initSpecificLayout();

		}

		async Task<List<Examination>> GetExamination_SessionCall()
		{
			Debug.WriteLine("AQUI 1 GetExamination_SessionCall");
			ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();

			List<Examination> examination_sessionCall = await examination_sessionManager.GetExamination_SessionCall(examination_session.id);
			if (examination_sessionCall == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return examination_sessionCall;
		}

		async void OnRegisterButtonClicked(object sender, EventArgs e)
		{
			registerButton.IsEnabled = false;

			if (examination_session.participationconfirmed != null)
			{
				await Navigation.PushAsync(new ExaminationSessionPaymentPageCS(examination_session));
			}
		}
	}
}
