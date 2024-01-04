using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Collections.ObjectModel;
//Ausing Acr.UserDialogs;
using Xamarin.Essentials;
using SportNow.Views.Profile;

namespace SportNow.Views
{
	public class AllEventsPageCS : DefaultPage
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
		MenuButton proximasCompeticoesButton;


		private StackLayout stackButtons;

		private CollectionView proximosEventosCollectionView;
		private CollectionView proximasCompeticoesCollectionView;

		private List<Event> proximosEventosAll, proximosEventos;
		private List<Competition> proximasCompeticoes;


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
				stackButtons = null;
			}
			CleanProximosEventosCollectionView();
			CleanProximasCompeticoesCollectionView();
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();


            int result = await getEventData();
			CreateStackButtons();
			CreateProximosEventosColletion();
			activateLastSelectedTab();
			//CreateCalendarioLink();
			//OnProximosEstagiosButtonClicked(null, null);

			hideActivityIndicator();

        }


		public void activateLastSelectedTab()
		{
			Debug.Print("activateLastSelectedTab");
			if (App.EVENTOS_activetab == "competicoes")
			{
				OnProximasCompeticoesButtonClicked(null, null);
			}
			else if (App.EVENTOS_activetab == "eventos")
			{
				OnProximosEventosButtonClicked(null, null);
			}
			else
			{
				OnProximosEventosButtonClicked(null, null);
			}
		}

		public async Task<int> getEventData()
		{
			proximosEventosAll = await GetFutureEventsAll();
			proximosEventos = new List<Event>();
			if (proximosEventosAll != null)
			{

				foreach (Event event_i in proximosEventosAll)
				{
					if ((event_i.imagemNome == "") | (event_i.imagemNome is null))
					{
						event_i.imagemSource = "logo_login.png";
					}
					else
					{
						event_i.imagemSource = Constants.images_URL + event_i.id + "_imagem_c";

					}

					if (event_i.participationconfirmed == "inscrito")
					{
						event_i.participationimage = "iconcheck.png";
					}

					proximosEventos.Add(event_i);
				}
			}

			List<Competition> proximasCompeticoesAll = await GetFutureCompetitions();
			proximasCompeticoes = new List<Competition>(); //await GetFutureCompetitions();

			if (proximasCompeticoesAll != null)
			{
				foreach (Competition competition in proximasCompeticoesAll)
				{
					if ((competition.imagemNome == "") | (competition.imagemNome is null))
					{
						competition.imagemSource = "logo_login.png";
					}
					else
					{
						competition.imagemSource = Constants.images_URL + competition.id + "_imagem_c";
						Debug.Print("ANTES competition ImageSource = " + competition.imagemSource);
					}

					if (competition.participationconfirmed == "confirmado")
					{
						competition.participationimage = "iconcheck.png";
					}

					proximasCompeticoes.Add(competition);
				}
			}

			return 1;
		}


		public void CreateStackButtons()
		{

			Debug.Print("AllEventsPageCS - CreateStackButtons");
			var width = Constants.ScreenWidth;
			var buttonWidth = (width-50) / 2;


			proximosEventosButton = new MenuButton("EVENTOS", buttonWidth, 40);
			proximosEventosButton.button.Clicked += OnProximosEventosButtonClicked;

			proximasCompeticoesButton = new MenuButton("COMPETIÇÕES", buttonWidth, 40);
			proximasCompeticoesButton.button.Clicked += OnProximasCompeticoesButtonClicked;

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
					proximasCompeticoesButton,
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


			//participacoesEventosButton.deactivate();
			//proximosEventosButton.activate();

		}


		public void CreateProximosEventosColletion()
		{
			//COLLECTION GRADUACOES
			proximosEventosCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				//ItemsSource = proximosEventosSelected,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter,  },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem Eventos deste tipo agendados", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White, FontSize = 20 },
							}
					}
				}
			};

			proximosEventosCollectionView.SelectionChanged += OnProximosEventosCollectionViewSelectionChanged;

			proximosEventosCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					HeightRequest=100
				};

				Frame itemFrame = new Frame
				{
					CornerRadius = 5 * (float) App.screenHeightAdapter,
					IsClippedToBounds = true,
					BorderColor = Color.Transparent,
					BackgroundColor = Color.Black,
					Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = App.ItemHeight,
					VerticalOptions = LayoutOptions.Center,
				};

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.4 }; //, HeightRequest = 60, WidthRequest = 60
				eventoImage.SetBinding(Image.SourceProperty, "imagemSource");

				itemFrame.Content = eventoImage;

				itemRelativeLayout.Children.Add(itemFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (5 * App.screenWidthAdapter));
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height);
					}));

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 15 * App.screenWidthAdapter, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "name");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(3 * App.screenWidthAdapter),
					yConstraint: Constraint.Constant(15 * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (10 * App.screenWidthAdapter));
					}),
					heightConstraint: Constraint.Constant(((App.ItemHeight - (15 * App.screenHeightAdapter)) / 2) ));


				Label categoryLabel = new Label { VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = 12 * App.screenWidthAdapter, TextColor = Color.White, LineBreakMode = LineBreakMode.NoWrap };
				categoryLabel.SetBinding(Label.TextProperty, "participationcategory");

				itemRelativeLayout.Children.Add(categoryLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(((App.ItemHeight - (15 * App.screenHeightAdapter)) / 2)),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant(((App.ItemHeight - (15 * App.screenHeightAdapter)) / 4)));

				Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = 12 * App.screenWidthAdapter, TextColor = Color.White, LineBreakMode = LineBreakMode.NoWrap };
				dateLabel.SetBinding(Label.TextProperty, "detailed_date");

				itemRelativeLayout.Children.Add(dateLabel,
					xConstraint: Constraint.Constant(5 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant((App.ItemHeight - 15) - ((App.ItemHeight - 15) / 4)),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (10 * App.screenHeightAdapter)); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant(((App.ItemHeight - (15 * App.screenHeightAdapter)) / 4)));


				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.SetBinding(Image.SourceProperty, "participationimage");

				itemRelativeLayout.Children.Add(participationImagem,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (25 * App.screenHeightAdapter));
					}),
					yConstraint: Constraint.Constant(3 * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(20 * App.screenHeightAdapter));
				
				return itemRelativeLayout;
			});
			relativeLayout.Children.Add(proximosEventosCollectionView,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(80),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
							}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 120; // 
			}));


		}

		public async void CreateCalendarioLink()
		{
			Label dateLabel = new Label { Text = "Faz aqui download do CALENDÁRIO completo", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = 13 * App.screenWidthAdapter, TextColor = App.topColor, LineBreakMode = LineBreakMode.NoWrap };

			var dateLabel_tap = new TapGestureRecognizer();
			dateLabel_tap.Tapped += async (s, e) =>
			{
				try
				{
					await Browser.OpenAsync("https://www.ippon.pt/images/calendario/calendario-ippon.jpg", BrowserLaunchMode.SystemPreferred);
				}
				catch (Exception ex)
				{

					// An unexpected error occured. No browser may be installed on the device.
				}
				//
				 //  Do your work here.
				 //
			};
			dateLabel.GestureRecognizers.Add(dateLabel_tap);


			relativeLayout.Children.Add(dateLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height)-20 * App.screenHeightAdapter; // 
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(20 * App.screenHeightAdapter));

		}

		public AllEventsPageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}

		public void CleanProximosEventosCollectionView()
		{
			Debug.Print("CleanProximosEstagiosCollectionView");
			//valida se os objetos já foram criados antes de os remover
			if (proximosEventosCollectionView != null)
			{
				relativeLayout.Children.Remove(proximosEventosCollectionView);
				proximosEventosCollectionView = null;
			}

		}

		public void CleanProximasCompeticoesCollectionView()
		{
			Debug.Print("CleanProximasCompeticoesCollectionView");
			//valida se os objetos já foram criados antes de os remover
			if (proximasCompeticoesCollectionView != null)
			{
				relativeLayout.Children.Remove(proximasCompeticoesCollectionView);
				proximasCompeticoesCollectionView = null;
			}

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

		async Task<List<Competition>> GetFutureCompetitions()
		{
			Debug.WriteLine("GetFutureCompetitions");
			CompetitionManager competitionManager = new CompetitionManager();
			List<Competition> futureCompetitions = await competitionManager.GetFutureCompetitions(App.member.id);
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

		async void OnProximosEventosButtonClicked(object sender, EventArgs e)
		{
			App.EVENTOS_activetab = "estagios";
			proximosEventosButton.activate();
			proximasCompeticoesButton.deactivate();
			proximosEventosCollectionView.ItemsSource = proximosEventos;
		}

		async void OnProximasCompeticoesButtonClicked(object sender, EventArgs e)
		{
			App.EVENTOS_activetab = "competicoes";
			proximosEventosButton.deactivate();
			proximasCompeticoesButton.activate();

			proximosEventosCollectionView.ItemsSource = proximasCompeticoes;
		}


		async void OnProximosEventosCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewProximosEstagiosSelectionChanged "+ (sender as CollectionView).SelectedItem.GetType().ToString());

			if ((sender as CollectionView).SelectedItem != null)
			{

				if ((sender as CollectionView).SelectedItem.GetType().ToString() == "SportNow.Model.Event")
                {
					Event event_v = (sender as CollectionView).SelectedItem as Event;
					await Navigation.PushAsync(new DetailEventPageCS(event_v));
				}
				else if ((sender as CollectionView).SelectedItem.GetType().ToString() == "SportNow.Model.Competition")
				{
					Competition competition = (sender as CollectionView).SelectedItem as Competition;
					await Navigation.PushAsync(new DetailCompetitionPageCS(competition));
				}

			}
		}

	}
}
