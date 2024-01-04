using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Views.Profile;

namespace SportNow.Views
{
	public class CompeticoesPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			CleanScreen();
		}

		MenuButton proximasCompeticoesButton;
		MenuButton resultadosCompeticoesButton;


		private StackLayout stackButtons;

		private CollectionView collectionViewProximasCompeticoes;
		private CollectionView collectionViewResultadosCompeticoes;

		private List<Competition> futureCompetitions;
		private List<Competition_Participation> futureCompetitionParticipations;

		private List<Competition_Participation> pastCompetitionParticipations;


		public void initLayout()
		{
			Title = "COMPETIÇÕES";

			var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "perfil.png",

			};
			toolbarItem.Clicked += OnPerfilButtonClicked;
			ToolbarItems.Add(toolbarItem);

		}


		public async void initSpecificLayout()
		{
			futureCompetitions = await GetFutureCompetitionsAll();
			futureCompetitionParticipations = await GetFutureCompetitionParticipations();
			pastCompetitionParticipations = await GetPastCompetitionParticipations();

			CreateStackButtons();
			CreateProximasCompeticoesColletion();
			CreateResultadosCompeticoesColletion();

			relativeLayout.Children.Remove(collectionViewResultadosCompeticoes);
		}

		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (stackButtons != null)
			{

				relativeLayout.Children.Remove(stackButtons);
				relativeLayout.Children.Remove(collectionViewProximasCompeticoes);
				relativeLayout.Children.Remove(collectionViewResultadosCompeticoes);

				stackButtons = null;
				collectionViewProximasCompeticoes = null;
				collectionViewResultadosCompeticoes = null;

			}

		}

		public void CreateStackButtons()
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width) / 4;


			proximasCompeticoesButton = new MenuButton("COMPETIÇÕES", buttonWidth, 60);
			proximasCompeticoesButton.button.Clicked += OnProximasCompeticoesButtonClicked;

			resultadosCompeticoesButton = new MenuButton("RESULTADOS", buttonWidth, 60);
			resultadosCompeticoesButton.button.Clicked += OnResultadosCompeticoesButtonClicked;

			stackButtons = new StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				//Spacing = 5,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40,
				Children =
				{
					proximasCompeticoesButton,
					resultadosCompeticoesButton,
				}
			};

			relativeLayout.Children.Add(stackButtons,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(40));

			proximasCompeticoesButton.activate();
			resultadosCompeticoesButton.deactivate();
		}


		public void CreateProximasCompeticoesColletion()
		{
			//COLLECTION GRADUACOES
			collectionViewProximasCompeticoes = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = futureCompetitions,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem competições agendadas.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White, FontSize = 20 },
							}
					}
				}
			};

			collectionViewProximasCompeticoes.SelectionChanged += OnCollectionViewProximasCompeticoesSelectionChanged;


			foreach (Competition competition in futureCompetitions)
			{

				/*competition.participationconfirmed = -1;
				competition.participationimage = "iconinfowhite.png";
				foreach (Competition_Participation competition_participation in futureCompetitionParticipations)
				{
					if ((competition.id == competition_participation.competicao_id) & (competition_participation.estado == "confirmado"))
					{
						competition.participationconfirmed = 1;
						competition.participationimage = "iconcheck.png";
					}
					else if ((competition.id == competition_participation.competicao_id) & (competition_participation.estado == "convocado"))
					{
						competition.participationconfirmed = 0;
						competition.participationimage = "iconinfowhite.png";
					}
				}*/
			}

			GradientBrush gradient = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 1),
			};

			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 143, 86), Convert.ToSingle(0)));
			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(246, 220, 178), Convert.ToSingle(0.5)));


			collectionViewProximasCompeticoes.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					Margin = new Thickness(10)
				};

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 20 * App.screenWidthAdapter, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "name");

				Frame nameFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Color.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(40, 0, 30, 0)
				};
				nameFrame.Content = nameLabel;

				itemRelativeLayout.Children.Add(nameFrame,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 5) - 20; // center of image (which is 40 wide)
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 5 * 4) + 10; // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 60; // 
					}));

				Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 15 * App.screenWidthAdapter, TextColor = Color.Black, LineBreakMode = LineBreakMode.NoWrap };
				dateLabel.SetBinding(Label.TextProperty, "date");

				Frame dateFrame = new Frame
				{
					Background = gradient,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = 5
				};
				dateFrame.Content = dateLabel;

				itemRelativeLayout.Children.Add(dateFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 5); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 60; // 
					}));

				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.SetBinding(Image.SourceProperty, "participationimage");

				itemRelativeLayout.Children.Add(participationImagem,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - 40);
					}),
					yConstraint: Constraint.Constant(20),
					widthConstraint: Constraint.Constant(20),
					heightConstraint: Constraint.Constant(20));

				return itemRelativeLayout;
			});



			relativeLayout.Children.Add(collectionViewProximasCompeticoes,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(80),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width); // center of image (which is 40 wide)
			}),
			heightConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height) - 100; // 
			}));

		}


		public void CreateResultadosCompeticoesColletion()
		{
			foreach (Competition_Participation competition_participation in pastCompetitionParticipations)
			{
				if (competition_participation.classificacao == "1")
				{
					competition_participation.classificacaoColor = Color.FromRgb(231, 188, 64);
				}
				else if (competition_participation.classificacao == "2")
				{
					competition_participation.classificacaoColor = Color.FromRgb(174, 174, 174);
				}
				else if (competition_participation.classificacao == "3")
				{
					competition_participation.classificacaoColor = Color.FromRgb(179, 144, 86);
				}
				else
				{
					competition_participation.classificacao = "P";
					competition_participation.classificacaoColor = Color.FromRgb(88, 191, 237);
				}
			}

			//COLLECTION RESULTADOS COMPETICOES
			collectionViewResultadosCompeticoes = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = pastCompetitionParticipations,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem resultados de competições.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White, FontSize = 20 },
							}
					}
				}
			};

			collectionViewResultadosCompeticoes.SelectionChanged += OnCollectionViewPastCompeticoesSelectionChanged;

			/*
			foreach (Competition competition in pastCompetitionParticipations)
			{
				competition.participationconfirmed = 1;
				competition.participationimage = "iconinfowhite.png";
			}*/

			GradientBrush gradient = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 1),
			};

			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 143, 86), Convert.ToSingle(0)));
			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(246, 220, 178), Convert.ToSingle(0.5)));


			collectionViewResultadosCompeticoes.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemResultatosRelativeLayout = new RelativeLayout
				{
					Margin = new Thickness(10)
				};

				Label competitionnameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 20 * App.screenWidthAdapter, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				competitionnameLabel.SetBinding(Label.TextProperty, "competicao_name");

				Frame competitionnameFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Color.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(40, 0, 30, 0)
				};
				competitionnameFrame.Content = competitionnameLabel;

				itemResultatosRelativeLayout.Children.Add(competitionnameFrame,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 5) - 20; // center of image (which is 40 wide)
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 5 * 4) + 10; // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 60; // 
					}));

				Label classificacaoLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 40 * App.screenWidthAdapter, TextColor = Color.White, LineBreakMode = LineBreakMode.NoWrap };
				classificacaoLabel.SetBinding(Label.TextProperty, "classificacao");

				Frame classificacaoFrame = new Frame
				{
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = 5
				};
				classificacaoFrame.SetBinding(Label.BackgroundColorProperty, "classificacaoColor");
				classificacaoFrame.Content = classificacaoLabel;

				itemResultatosRelativeLayout.Children.Add(classificacaoFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 5); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 60; // 
					}));

				Image participationImagem = new Image { Aspect = Aspect.AspectFill, Source= "iconcheck.png" }; //, HeightRequest = 60, WidthRequest = 60
																												   //participationImagem.SetBinding(Image.SourceProperty, "participationimage");

				itemResultatosRelativeLayout.Children.Add(participationImagem,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - 40);
					}),
					yConstraint: Constraint.Constant(20),
					widthConstraint: Constraint.Constant(20),
					heightConstraint: Constraint.Constant(20));

				return itemResultatosRelativeLayout;
			});

			relativeLayout.Children.Add(collectionViewResultadosCompeticoes,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(80),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width); // center of image (which is 40 wide)
			}),
			heightConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height) - 100; // 
			}));
		}

		public CompeticoesPageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfilePageCS());
		}


		async Task<List<Competition>> GetFutureCompetitionsAll()
		{
			Debug.WriteLine("GetFutureCompetitionsAll");
			CompetitionManager competitionManager = new CompetitionManager();

			List<Competition> futureCompetitions = await competitionManager.GetFutureCompetitionsAll();
			if (futureCompetitions == null)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return futureCompetitions;
		}

		async Task<List<Competition_Participation>> GetFutureCompetitionParticipations()
		{
			Debug.WriteLine("GetFutureCompetitionParticipation");
			CompetitionManager competitionManager = new CompetitionManager();

			List<Competition_Participation> futureCompetitionParticipations = await competitionManager.GetFutureCompetitionParticipations(App.member.id);
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

		async Task<List<Competition_Participation>> GetPastCompetitionParticipations()
		{
			Debug.WriteLine("GetPastCompetitionParticipations");
			CompetitionManager competitionManager = new CompetitionManager();

			List<Competition_Participation> pastCompetitionParticipations = await competitionManager.GetPastCompetitionParticipations(App.member.id);
			if (pastCompetitionParticipations == null)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return pastCompetitionParticipations;
		}

		async void OnProximasCompeticoesButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnProximasCompeticoesButtonClicked");

			proximasCompeticoesButton.activate();
			resultadosCompeticoesButton.deactivate();

			relativeLayout.Children.Add(collectionViewProximasCompeticoes,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(80),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
							}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 100; // 
							})
			);

			relativeLayout.Children.Remove(collectionViewResultadosCompeticoes);
			
			//collectionViewProximasCompeticoes.IsVisible = true;
			//collectionViewResultadosCompeticoes.IsVisible = false;


			/*			gridGeral.IsVisible = true;
						gridIdentificacao.IsVisible = false;
						gridMorada.IsVisible = false;
						gridEncEducacao.IsVisible = false;*/

		}

		async void OnResultadosCompeticoesButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnResultadosCompeticoesButtonClicked");

			resultadosCompeticoesButton.activate();
			proximasCompeticoesButton.deactivate();

			relativeLayout.Children.Add(collectionViewResultadosCompeticoes,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(80),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
							}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 100; // 
							})
			);


			relativeLayout.Children.Remove(collectionViewProximasCompeticoes);
			
			//collectionViewProximasCompeticoes.IsVisible = false;
			//collectionViewResultadosCompeticoes.IsVisible = true;
			/*			gridGeral.IsVisible = true;
						gridIdentificacao.IsVisible = false;
						gridMorada.IsVisible = false;
						gridEncEducacao.IsVisible = false;*/

		}


		async void OnCollectionViewProximasCompeticoesSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewProximasCompeticoesSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{

				Competition competition = (sender as CollectionView).SelectedItem as Competition;

				Competition_Participation competition_participation = null;

				foreach (Competition_Participation competition_participation_i in futureCompetitionParticipations)
				{
					if (competition.id == competition_participation_i.competicao_id)
					{

						competition_participation = competition_participation_i;
					}

				}

				Debug.WriteLine("OnCollectionViewProximasCompeticoesSelectionChanged competition.name " + competition.name);

				await Navigation.PushAsync(new DetailCompetitionPageCS(competition));

			}
		}

		async void OnCollectionViewPastCompeticoesSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewResultadosCompeticoesSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{

				Competition_Participation competition_participation = (sender as CollectionView).SelectedItem as Competition_Participation;

				await Navigation.PushAsync(new DetailCompetitionResultPageCS(competition_participation));

			}
		}
	}
}
