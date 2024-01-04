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
	public class CompetitionCallPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			//competition_participation = App.competition_participation;
			initSpecificLayout();

		}

		protected override void OnDisappearing()
		{
			//relativeLayout = null;
			if (collectionViewCompetitionCall != null)
			{
				relativeLayout.Children.Remove(collectionViewCompetitionCall);
				collectionViewCompetitionCall = null;
			}

			if (frame_registerButton!= null)
			{
				relativeLayout.Children.Remove(frame_registerButton);
				frame_registerButton = null;
			}
						
		}



		private CollectionView collectionViewCompetitionCall;

		private Competition competition;
		private List<Competition> competitions;

		private Competition_Participation competition_participation; 

		private List<Competition_Participation> competitionCall;

		RegisterButton registerButton;

        Frame frame_registerButton;

		Label competitionNameLabel;
		Label nameTitleLabel;
		Label categoryTitleLabel;

		public void initLayout()
		{
			Title = "CONVOCATÓRIA";
		}


		public async void initSpecificLayout()
		{
			competitionCall = await GetCompetitionCall();

			CreateCompetitionCallColletionView();

		}

		
		public void CreateCompetitionCallColletionView()
		{

			foreach (Competition_Participation competition_participation in competitionCall)
			{
				Debug.Print("competition_participation name = " + competition_participation.name);

				if (competition_participation.estado == "confirmado")
				{
					competition_participation.estadoTextColor = App.topColor;
				}
				else if (competition_participation.estado == "cancelado")
				{
					competition_participation.estadoTextColor = Color.FromRgb(233, 93, 85);
				}
				else
				{
					competition_participation.estadoTextColor = App.normalTextColor;
                }
			}

			competitionNameLabel = new Label
			{
				Text = competition.name,
				BackgroundColor = Color.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 25,
				TextColor = App.topColor
			};

			relativeLayout.Children.Add(competitionNameLabel,
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

			if (competitionCall.Count > 0)
            {
				nameTitleLabel = new Label
				{
					Text = "NOME",
					BackgroundColor = Color.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
					FontSize = 22,
					TextColor = App.bottomColor,
					LineBreakMode = LineBreakMode.WordWrap
				};

				relativeLayout.Children.Add(nameTitleLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(60),
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
					Text = "ESCALÃO",
					BackgroundColor = Color.Transparent,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Start,
					FontSize = 22,
					TextColor = App.bottomColor,
                    LineBreakMode = LineBreakMode.WordWrap
				};

				relativeLayout.Children.Add(categoryTitleLabel,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 3 * 2); // center of image (which is 40 wide)
				}),
					yConstraint: Constraint.Constant(60),
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




			collectionViewCompetitionCall = new CollectionView
			{
				SelectionMode = SelectionMode.None,
				ItemsSource = competitionCall,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Ainda não foi criada convocatória para esta competição.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.Red, FontSize = 20 },
							}
					}
				}
			};

			//collectionViewCompetitionCall.SelectionChanged += OnCollectionViewProximasCompeticoesSelectionChanged;

			collectionViewCompetitionCall.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					Margin = new Thickness(3)
				};

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 15, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "membername");
				nameLabel.SetBinding(Label.TextColorProperty, "estadoTextColor");

				Frame nameFrame = new Frame
				{
					BorderColor = App.bottomColor,
                    BackgroundColor = Color.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0),
					HasShadow = false
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

				Label categoryLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 13, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				categoryLabel.SetBinding(Label.TextProperty, "categoria");



				Frame categoryFrame = new Frame
				{
					BorderColor = App.bottomColor,
                    BackgroundColor = Color.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0),
                    HasShadow = false
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

			if ((competition.participationconfirmed == "confirmado") | (competition.participationconfirmed == null))
			{

				relativeLayout.Children.Add(collectionViewCompetitionCall,
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
				relativeLayout.Children.Add(collectionViewCompetitionCall,
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

				registerButton = new RegisterButton("INSCREVER", 100, 50);

				registerButton.button.Clicked += OnRegisterButtonClicked;

				relativeLayout.Children.Add(registerButton,
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

		public CompetitionCallPageCS(List<Competition> competitions, Competition_Participation competition_participation)
		{
			this.competitions = competitions;
			this.competition = competitions[0];
			this.competition_participation = competition_participation;
			this.initLayout();
			//this.initSpecificLayout();

		}

		async Task<List<Competition_Participation>> GetCompetitionCall()
		{
			Debug.WriteLine("GetCompetitionCall");
			CompetitionManager competitionManager = new CompetitionManager();

			List<Competition_Participation> futureCompetitionParticipations = await competitionManager.GetCompetitionCall(competition.id);
			if (futureCompetitionParticipations == null)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return futureCompetitionParticipations;
		}

		async void OnRegisterButtonClicked(object sender, EventArgs e)
		{

			registerButton.button.IsEnabled = false;

			if (competition.participationconfirmed != null)
			{
				Debug.WriteLine("OnRegisterButtonClicked competition_participation.name " + competition.name);

				await Navigation.PushAsync(new CompetitionPaymentPageCS(competition));

			}

		}

	}
}
