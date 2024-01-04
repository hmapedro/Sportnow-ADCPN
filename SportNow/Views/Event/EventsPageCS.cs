using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Collections.ObjectModel;
using SportNow.Views.Profile;

namespace SportNow.Views
{
	public class EventsPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		MenuButton proximosEventosButton;
		MenuButton participacoesEventosButton;


		private StackLayout stackButtons;

		private CollectionView collectionViewProximosEventos;
		private CollectionView collectionViewParticipacoesEventos;

		private List<Event> futureEvents;
		private List<Event_Participation> futureEventParticipations;
		private List<Event_Participation> pastEventParticipations;


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


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (stackButtons != null)
            {

				relativeLayout.Children.Remove(stackButtons);
				relativeLayout.Children.Remove(collectionViewProximosEventos);
				relativeLayout.Children.Remove(collectionViewParticipacoesEventos);

				stackButtons = null;
				collectionViewProximosEventos = null;
				collectionViewParticipacoesEventos = null;

			}

		}

		public async void initSpecificLayout()
		{
			futureEvents = await GetFutureEventsAll();
			futureEventParticipations = await GetFutureEventParticipations();
			pastEventParticipations = await GetPastEventParticipations();

			CreateStackButtons();
			CreateProximosEventosColletion();
			CreateParticipacoesEventosColletion();


			//collectionViewProximasCompeticoes.IsVisible = true;
			//collectionViewResultadosCompeticoes.IsVisible = false;

			relativeLayout.Children.Remove(collectionViewParticipacoesEventos);
		}

		public void CreateStackButtons()
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width) / 4;

			
			proximosEventosButton = new MenuButton("EVENTOS", buttonWidth, 60);
			proximosEventosButton.button.Clicked += OnProximosEventosButtonClicked;

			participacoesEventosButton = new MenuButton("PARTICIPAÇÕES", buttonWidth, 60);
			participacoesEventosButton.button.Clicked += OnParticipacoesEventosButtonClicked;

			stackButtons = new StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 5,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40,
				Children =
				{
					proximosEventosButton,
					participacoesEventosButton,
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


			participacoesEventosButton.deactivate();
			proximosEventosButton.activate();

		}


		public void CreateProximosEventosColletion()
		{

			//COLLECTION GRADUACOES
			collectionViewProximosEventos = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = futureEvents,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5,  },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem Eventos agendados.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White, FontSize = 20 },
							}
					}
				}
			};

			//collectionViewProximosEventos.SelectionChanged += OnCollectionViewProximosEventosSelectionChanged;


			foreach (Event event_i in futureEvents)
			{
				if ((event_i.imagemNome == "") | (event_i.imagemNome is null))
				{
					event_i.imagemSource = "logo_aksl.png";
				}
				else {
					event_i.imagemSource = "https://20.234.59.92nkslcrm/upload/" + event_i.id + "_imagem_c";
					
				}
				
				event_i.participationconfirmed = "nao_inscrito";
				//event_i.participationimage = "iconinfowhite.png";
				event_i.participationimage = "";
				foreach (Event_Participation event_participation in futureEventParticipations)
				{
					if ((event_i.id == event_participation.evento_id) & (event_participation.estado == "inscrito"))
					{
						event_i.participationconfirmed = "inscrito";
                        event_i.participationimage = "iconcheck.png";
					}
/*					else if ((event_i.id == event_participation.evento_id))// & (event_participation.estado == "convocado"))
					{
						event_i.participationconfirmed = 0;
						event_i.participationimage = "iconinfowhite.png";
					}*/
				}
			}

			GradientBrush gradient = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 1),
			};

			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 143, 86), Convert.ToSingle(0)));
			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(246, 220, 178), Convert.ToSingle(0.5)));


			collectionViewProximosEventos.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					HeightRequest=100
				};

				Frame itemFrame = new Frame
				{
					CornerRadius = 5,
					IsClippedToBounds = true,
					BorderColor = Color.FromRgb(182, 145, 89),
					BackgroundColor = Color.Transparent,
					Padding = new Thickness(2, 2, 2, 2),
					HeightRequest = 100,
					VerticalOptions = LayoutOptions.Center,
				};

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.25 }; //, HeightRequest = 60, WidthRequest = 60
				eventoImage.SetBinding(Image.SourceProperty, "imagemSource");

				itemFrame.Content = eventoImage;

				itemRelativeLayout.Children.Add(itemFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width-5);
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height);
					}));

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 18 * App.screenWidthAdapter, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "name");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(30),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width); 
					}),
					heightConstraint: Constraint.Constant(40));

				Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 12 * App.screenWidthAdapter, TextColor = Color.White, LineBreakMode = LineBreakMode.NoWrap };
				dateLabel.SetBinding(Label.TextProperty, "detailed_date");

				itemRelativeLayout.Children.Add(dateLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(80),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant(20));


				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.SetBinding(Image.SourceProperty, "participationimage");

				itemRelativeLayout.Children.Add(participationImagem,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - 25);
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(20),
					heightConstraint: Constraint.Constant(20));
				
				return itemRelativeLayout;
			});



			relativeLayout.Children.Add(collectionViewProximosEventos,
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


		public void CreateParticipacoesEventosColletion()
		{

			//COLLECTION RESULTADOS COMPETICOES
			collectionViewParticipacoesEventos = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = pastEventParticipations,
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

			collectionViewParticipacoesEventos.SelectionChanged += OnCollectionViewPastEventosSelectionChanged;

			collectionViewParticipacoesEventos.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemParticipacaoRelativeLayout = new RelativeLayout
				{
					Margin = new Thickness(10)
				};

				Label competitionnameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 20 * App.screenWidthAdapter, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				competitionnameLabel.SetBinding(Label.TextProperty, "evento_name");

				Frame competitionnameFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Color.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(40, 0, 30, 0)
				};
				competitionnameFrame.Content = competitionnameLabel;

				itemParticipacaoRelativeLayout.Children.Add(competitionnameFrame,
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

				Label dateParticipationLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 15 * App.screenWidthAdapter, TextColor = Color.Black, LineBreakMode = LineBreakMode.NoWrap };
				dateParticipationLabel.SetBinding(Label.TextProperty, "evento_data");

				Frame dateFrame = new Frame
				{
					
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = 5
				};
				dateFrame.Content = dateParticipationLabel;

				itemParticipacaoRelativeLayout.Children.Add(dateFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 5); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 60;
					}));


				Image participationImagem = new Image { Aspect = Aspect.AspectFill, Source = "iconcheck.png" };

				itemParticipacaoRelativeLayout.Children.Add(participationImagem,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - 40);
					}),
					yConstraint: Constraint.Constant(20),
					widthConstraint: Constraint.Constant(20),
					heightConstraint: Constraint.Constant(20));

				return itemParticipacaoRelativeLayout;
			});

			relativeLayout.Children.Add(collectionViewParticipacoesEventos,
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

		public EventsPageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfilePageCS());
		}


		async Task<List<Event>> GetFutureEventsAll()
		{
			Debug.WriteLine("GetFutureEventsAll");
			EventManager eventManager = new EventManager();

			List<Event> futureEvents = await eventManager.GetFutureEventsAll(App.member.id);
			if (futureEvents == null)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return futureEvents;
		}

		async Task<List<Event_Participation>> GetFutureEventParticipations()
		{
			Debug.WriteLine("GetFutureCompetitionParticipation");
			EventManager eventManager = new EventManager();

			List<Event_Participation> futureEventParticipations = await eventManager.GetFutureEventParticipations(App.member.id);
			if (futureEventParticipations == null)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return futureEventParticipations;
		}

		async Task<List<Event_Participation>> GetPastEventParticipations()
		{
			Debug.WriteLine("GetPastCompetitionParticipations");
			EventManager eventManager = new EventManager();

			List<Event_Participation> pastEventParticipations = await eventManager.GetPastEventParticipations(App.member.id);
			if (pastEventParticipations == null)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return pastEventParticipations;
		}

		async void OnProximosEventosButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnProximosEventosButtonClicked");

			participacoesEventosButton.deactivate();
			proximosEventosButton.activate();


			relativeLayout.Children.Add(collectionViewProximosEventos,
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

			relativeLayout.Children.Remove(collectionViewParticipacoesEventos);

			//collectionViewProximasCompeticoes.IsVisible = true;
			//collectionViewResultadosCompeticoes.IsVisible = false;


			/*			gridGeral.IsVisible = true;
						gridIdentificacao.IsVisible = false;
						gridMorada.IsVisible = false;
						gridEncEducacao.IsVisible = false;*/

		}

		async void OnParticipacoesEventosButtonClicked(object sender, EventArgs e)
		{

			participacoesEventosButton.activate();
			proximosEventosButton.deactivate();

			relativeLayout.Children.Add(collectionViewParticipacoesEventos,
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


			relativeLayout.Children.Remove(collectionViewProximosEventos);

			//collectionViewProximasCompeticoes.IsVisible = false;
			//collectionViewResultadosCompeticoes.IsVisible = true;
			/*			gridGeral.IsVisible = true;
						gridIdentificacao.IsVisible = false;
						gridMorada.IsVisible = false;
						gridEncEducacao.IsVisible = false;*/

		}


		/*async void OnCollectionViewProximosEventosSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewProximosEventosSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{

				Event event_v = (sender as CollectionView).SelectedItem as Event;

				Event_Participation event_participation = null;

				foreach (Event_Participation event_participation_i in futureEventParticipations)
				{
					if (event_v.id == event_participation_i.evento_id)
					{

						event_participation = event_participation_i;
					}

				}

				Debug.WriteLine("OnCollectionViewProximosEventosSelectionChanged Event.name " + event_v.name);

				await Navigation.PushAsync(new DetailEventPageCS(event_v, event_participation));

			}
		}*/

		async void OnCollectionViewPastEventosSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewResultadosCompeticoesSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{

				Event_Participation event_participation = (sender as CollectionView).SelectedItem as Event_Participation;

				await Navigation.PushAsync(new DetailEventParticipationPageCS(event_participation));

			}
		}
	}
}
