using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Collections.ObjectModel;
using Syncfusion.SfChart.XForms;
using SportNow.Model.Charts;
//Ausing Acr.UserDialogs;
using SportNow.Views.Profile;

namespace SportNow.Views
{
	public class DoPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		List<Competition_Result> competition_results;

		MenuButton premiosButton, palmaresButton, participacoesEventosButton, quotasButton;
		private RelativeLayout premiosRelativeLayout, palmaresRelativeLayout, participacoesEventosRelativeLayout, quotasRelativeLayout;


		private StackLayout stackButtons;

		private CollectionView collectionViewPremios, collectionViewParticipacoesCompeticoes, collectionViewParticipacoesEventos, collectionViewPastQuotas;
		private SfChart chart;
        Image estadoQuotaImage;

        private List<Award> awards;
		private List<Competition_Participation>  pastCompetitionParticipations;
		private List<Event_Participation> pastEventParticipations;

		public void initLayout()
		{
			Title = "HISTÓRICO";

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
				relativeLayout.Children.Remove(premiosRelativeLayout);
				relativeLayout.Children.Remove(collectionViewParticipacoesCompeticoes);
				relativeLayout.Children.Remove(palmaresRelativeLayout);
				relativeLayout.Children.Remove(collectionViewParticipacoesEventos);
                relativeLayout.Children.Remove(quotasRelativeLayout);
				if (collectionViewPastQuotas!=null)
				{
                    relativeLayout.Children.Remove(collectionViewPastQuotas);
                }
				

                stackButtons = null;
				collectionViewPremios = null;
				collectionViewParticipacoesCompeticoes = null;
				palmaresRelativeLayout = null;
				collectionViewParticipacoesEventos = null;
                quotasRelativeLayout = null;
                collectionViewPastQuotas = null;
            }
			if (chart != null)
			{
				relativeLayout.Children.Remove(chart);
				chart = null;
			}

		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

			pastEventParticipations = await GetPastEventParticipations();
			pastCompetitionParticipations = await GetPastCompetitionParticipations();

			CreateStackButtons();
			CreatePremios();
			CreateParticipacoesCompeticoesColletion();
			CreateParticipacoesEventosColletion();

            quotasRelativeLayout = new RelativeLayout
            {
                Margin = new Thickness(5)
            };

            CreateCurrentQuota();
			CreatePastQuotas();


            activateLastSelectedTab();

            hideActivityIndicator();
        }

		public void activateLastSelectedTab() {

			if (App.DO_activetab == "palmares")
			{
				OnPalmaresButtonClicked(null, null);
			}
			else if (App.DO_activetab == "participacoesevento")
			{
				OnParticipacoesEventosButtonClicked(null, null);
			}
			else if (App.DO_activetab == "premios")
			{
				OnPremiosButtonClicked(null, null);
			}
            else if (App.DO_activetab == "quotas")
            {
                OnQuotasButtonClicked(null, null);
            }
            else {
				OnPalmaresButtonClicked(null, null);
			}
		}

		public void CreateStackButtons()
		{

			Debug.Print("CreateStackButtons Constants.ScreenWidth = "+ Constants.ScreenWidth);
			var width = Constants.ScreenWidth;
			var buttonWidth = (width - 50) / 4;

			palmaresButton = new MenuButton("PALMARÉS", buttonWidth, 60);
			palmaresButton.button.Clicked += OnPalmaresButtonClicked;

			participacoesEventosButton = new MenuButton("EVENTOS", buttonWidth, 60);
			participacoesEventosButton.button.Clicked += OnParticipacoesEventosButtonClicked;

			premiosButton = new MenuButton("PRÉMIOS", buttonWidth, 60);
			premiosButton.button.Clicked += OnPremiosButtonClicked;

            quotasButton = new MenuButton("QUOTAS", buttonWidth, 60);
            quotasButton.button.Clicked += OnQuotasButtonClicked;

            stackButtons = new StackLayout
			{
				Margin = new Thickness(0),
				Spacing = 5,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 60 * App.screenHeightAdapter,
				Children =
				{
					palmaresButton,
					participacoesEventosButton,
					premiosButton,
                    quotasButton
                }
			};

			//Content = stackButtons;
			relativeLayout.Children.Add(stackButtons,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter));

			palmaresButton.activate();
			participacoesEventosButton.deactivate();
			premiosButton.deactivate();
		}

		public void CreatePremios() {
			premiosRelativeLayout = new RelativeLayout
			{
				Margin = new Thickness(5)
			};

			CreatePremiosCollection();

			relativeLayout.Children.Add(premiosRelativeLayout,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (80 * App.screenHeightAdapter);
				}));
		}


		public async void CreatePremiosCollection()
		{			
			var result = await GetAwards_Student(App.member);

			/*Label premiosLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
			premiosLabel.Text = "PRÉMIOS";

			premiosRelativeLayout.Children.Add(premiosLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter));*/

			//COLLECTION PREMIOS
			collectionViewPremios = new CollectionView
			{
				SelectionMode = SelectionMode.None,
				ItemsSource = awards,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5,  },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem prémios associados.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.topColor, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			GradientBrush gradient = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 1),
			};

			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 143, 86), Convert.ToSingle(0)));
			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(246, 220, 178), Convert.ToSingle(0.5)));


			collectionViewPremios.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					//HeightRequest=220 * App.screenHeightAdapter,
                    //WidthRequest = 220 * App.screenHeightAdapter
                };


				Image premioImage= new Image {};
				premioImage.SetBinding(Image.SourceProperty, "imagem");

				itemRelativeLayout.Children.Add(premioImage,
					xConstraint: Constraint.Constant(10),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(150 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(90 * App.screenHeightAdapter));

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "name");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(10),
					yConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(150 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(90 * App.screenHeightAdapter));

				
				
				return itemRelativeLayout;
			});



			premiosRelativeLayout.Children.Add(collectionViewPremios,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (70 * App.screenHeightAdapter);
				}));

		}


		public void CreateParticipacoesEventosColletion()
		{

			//COLLECTION PARTICIPAÇÕES EVENTOS
			collectionViewParticipacoesEventos = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = pastEventParticipations,
                Margin = new Thickness(5),
                ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem participações em eventos.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.topColor, FontSize = App.itemTitleFontSize },
                            }
					}
				}
			};

			foreach (Event_Participation event_participation in pastEventParticipations)
			{
				if ((event_participation.imagemNome == "") | (event_participation.imagemNome is null))
				{
					event_participation.imagemSource = "logo_login.png";
				}
				else
				{
					event_participation.imagemSource = Constants.images_URL + event_participation.evento_id + "_imagem_c";

				}
			}

			collectionViewParticipacoesEventos.SelectionChanged += OnCollectionViewParticipacoesEventosSelectionChanged;

			collectionViewParticipacoesEventos.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					HeightRequest = App.ItemHeight,
					WidthRequest = App.ItemWidth
				};

				Frame itemFrame = new Frame
				{
					CornerRadius = 5 * (float)App.screenHeightAdapter,
					IsClippedToBounds = true,
					BorderColor = Color.Transparent,
					BackgroundColor = Color.Black,
					Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = App.ItemHeight,
					WidthRequest = App.ItemWidth,
					VerticalOptions = LayoutOptions.Center,
					HasShadow = false
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

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "evento_name");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(3 * App.screenWidthAdapter),
					yConstraint: Constraint.Constant(15 * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (10 * App.screenWidthAdapter));
					}),
					heightConstraint: Constraint.Constant(((App.ItemHeight - (15 * App.screenHeightAdapter)) / 2)));

				Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				dateLabel.SetBinding(Label.TextProperty, "evento_detailed_date");

				itemRelativeLayout.Children.Add(dateLabel,
					xConstraint: Constraint.Constant(5 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant((App.ItemHeight - (15 * App.screenHeightAdapter)) - ((App.ItemHeight - (15 * App.screenHeightAdapter)) / 3)),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width-(10 * App.screenHeightAdapter)); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant(((App.ItemHeight - (15 * App.screenHeightAdapter)) / 3)));


				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.Source = "iconcheck.png";

				itemRelativeLayout.Children.Add(participationImagem,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (25 * App.screenHeightAdapter));
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(20 * App.screenHeightAdapter));

				return itemRelativeLayout;

			});

		}


		public void CreateParticipacoesCompeticoesColletion()
		{

			palmaresRelativeLayout = new RelativeLayout
			{
				Margin = new Thickness(5)
			};


			//COLLECTION PARTICIPAÇÕES EVENTOS
			collectionViewParticipacoesCompeticoes = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = pastCompetitionParticipations,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 5 * App.screenWidthAdapter, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem resultados de competições.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.topColor, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			foreach (Competition_Participation competition_participation in pastCompetitionParticipations)
			{
				if ((competition_participation.imagemNome == "") | (competition_participation.imagemNome is null))
				{
					competition_participation.imagemSource = "logo_login.png";
				}
				else
				{
					competition_participation.imagemSource = Constants.images_URL + competition_participation.competicao_id + "_imagem_c";
				}

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
					competition_participation.classificacaoColor = Color.FromRgb(88, 191, 237);
					if ((competition_participation.classificacao == "") | (competition_participation.classificacao is null))
					{
						competition_participation.classificacao = "P";

					}
					else
					{
						competition_participation.classificacao = competition_participation.classificacao.ToUpper();
					}
				}
			}

			collectionViewParticipacoesCompeticoes.SelectionChanged += OnCollectionViewParticipacoesCompeticoesSelectionChanged;

			collectionViewParticipacoesCompeticoes.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					HeightRequest = App.ItemHeight,
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
					HasShadow = false
				};

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.4 }; //, HeightRequest = 60, WidthRequest = 60
				eventoImage.SetBinding(Image.SourceProperty, "imagemSource");

				itemFrame.Content = eventoImage;

				itemRelativeLayout.Children.Add(itemFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (1 * App.screenWidthAdapter));
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height);
					}));

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "competicao_name");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(3 * App.screenWidthAdapter),
					yConstraint: Constraint.Constant(15 * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (10 * App.screenWidthAdapter));
					}),
					heightConstraint: Constraint.Constant(((App.ItemHeight - (15 * App.screenHeightAdapter)) / 2)));


				Label categoryLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				categoryLabel.SetBinding(Label.TextProperty, "categoria");

				itemRelativeLayout.Children.Add(categoryLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(((App.ItemHeight - (15 * App.screenHeightAdapter)) / 2)),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant(((App.ItemHeight - (15 * App.screenHeightAdapter)) / 4)));

				Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = Color.White, LineBreakMode = LineBreakMode.NoWrap };
				dateLabel.SetBinding(Label.TextProperty, "competicao_detailed_date");

				itemRelativeLayout.Children.Add(dateLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant((App.ItemHeight - 15) - ((App.ItemHeight - 15) / 4)),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant(((App.ItemHeight - (15 * App.screenHeightAdapter)) / 4)));

				Label classificacaoLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = Color.White, LineBreakMode = LineBreakMode.NoWrap };
				classificacaoLabel.SetBinding(Label.TextProperty, "classificacao");

				Frame classificacaoFrame = new Frame
				{
					CornerRadius = (float) (3 * App.screenHeightAdapter),
					IsClippedToBounds = true,
					Padding = 0
				};
				classificacaoFrame.SetBinding(Label.BackgroundColorProperty, "classificacaoColor");
				classificacaoFrame.Content = classificacaoLabel;

				/*
				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.Source = "iconcheck.png";
				*/
				itemRelativeLayout.Children.Add(classificacaoFrame,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (21 * App.screenHeightAdapter));
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(20 * App.screenHeightAdapter));

				return itemRelativeLayout;

			});

			palmaresRelativeLayout.Children.Add(collectionViewParticipacoesCompeticoes,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width); // center of image (which is 40 wide)
			}),
			heightConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height) - 200 * App.screenHeightAdapter; // 
			}));


			if (pastCompetitionParticipations.Count > 0) { 
				chart = createChart();

				palmaresRelativeLayout.Children.Add(chart,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 210 * App.screenHeightAdapter; // 
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(200 * App.screenHeightAdapter));
			}
		}

		public SfChart createChart() {
			SfChart chart = new SfChart();

			if (Device.RuntimePlatform == Device.Android)
			{
				chart.BackgroundColor = Color.Transparent;//.FromRgb(25, 25, 25);
			}

			//Initializing Primary Axis
			CategoryAxis primaryAxis = new CategoryAxis();
			primaryAxis.Title.Text = "Classificacao";
			chart.PrimaryAxis = primaryAxis;

			NumericalAxis secondaryAxis = new NumericalAxis();
			secondaryAxis.Title.Text = "#";
			chart.SecondaryAxis = secondaryAxis;


			//this.BindingContext = competition_results;
			
			this.BindingContext = new Competition_Results(pastCompetitionParticipations);

			//Initializing column series
			PieSeries series = new PieSeries()
			{
				ColorModel = new ChartColorModel()
				{
					Palette = ChartColorPalette.Custom,
					CustomBrushes = new ChartColorCollection()
					 {
						 Color.FromRgb(231, 188, 64),
						 Color.FromRgb(174, 174, 174),
						 Color.FromRgb(179, 144, 86),
						 Color.FromRgb(88, 191, 237)
					 }
				}
			};
			series.SetBinding(ChartSeries.ItemsSourceProperty, "Data");
			series.XBindingPath = "classificacao";
			series.YBindingPath = "count";

			series.EnableSmartLabels = true;
			series.DataMarkerPosition = CircularSeriesDataMarkerPosition.OutsideExtended;
			series.ConnectorLineType = ConnectorLineType.Bezier;
			series.StartAngle = 75;
			series.EndAngle = 435;

			series.DataMarker = new ChartDataMarker();

			chart.Series.Add(series);

			return chart;
		}

        public async void CreateCurrentQuota()
        {
            if (App.member.currentFee == null)
            {
                var result = await GetCurrentFees(App.member);
            }


            bool hasQuotaPayed = false;

            if (App.member.currentFee != null)
            {
                if ((App.member.currentFee.estado == "fechado") | (App.member.currentFee.estado == "recebido") | (App.member.currentFee.estado == "confirmado"))
                {
                    hasQuotaPayed = true;
                }
            }

            Frame quotasFrame = new Frame
            {
                CornerRadius = 5,
                IsClippedToBounds = true,
                BorderColor = App.topColor,
                BackgroundColor = Color.Transparent,
                Padding = new Thickness(2, 2, 2, 2),
                HeightRequest = 140 * App.screenHeightAdapter,
                VerticalOptions = LayoutOptions.Center,
				HasShadow = false,
            };

            var tapGestureRecognizer_quotasFrame = new TapGestureRecognizer();
            tapGestureRecognizer_quotasFrame.Tapped += async (s, e) => {
                //await Navigation.PushAsync(new QuotasPageCS());
            };
            quotasFrame.GestureRecognizers.Add(tapGestureRecognizer_quotasFrame);

            RelativeLayout currentQuotasRelativeLayout = new RelativeLayout
            {
                Margin = new Thickness(0)
            };
            quotasFrame.Content = currentQuotasRelativeLayout;

            string logoFeeFileName = "", estadoImageFileName = "";


            if (hasQuotaPayed == true)
            {
                logoFeeFileName = "logo_adcpn_fgp.png";
                estadoImageFileName = "iconcheck.png";
            }
            else if (hasQuotaPayed == false)
            {
                logoFeeFileName = "logo_adcpn_fgp.png";
                estadoImageFileName = "iconinativo.png";
            }

            Image LogoFeeFPG = new Image
            {
                Source = "logo_fgp.png",
                //WidthRequest = 100,
                HorizontalOptions = LayoutOptions.Center
            };
            currentQuotasRelativeLayout.Children.Add(LogoFeeFPG,
                xConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
                yConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
                widthConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(100 * App.screenHeightAdapter));

            Image LogoFeeADCPN = new Image
            {
                Source = "logo_login.png",
                //WidthRequest = 100,
                HorizontalOptions = LayoutOptions.Center
            };
            currentQuotasRelativeLayout.Children.Add(LogoFeeADCPN,
                xConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width) - 120 * App.screenHeightAdapter;
                }),
                yConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
                widthConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
                heightConstraint: Constraint.Constant(100 * App.screenHeightAdapter));

            estadoQuotaImage = new Image
            {
                Source = estadoImageFileName,
                WidthRequest = 20
            };

            currentQuotasRelativeLayout.Children.Add(estadoQuotaImage,
                xConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width) - 20 * App.screenHeightAdapter;
                }),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
                heightConstraint: Constraint.Constant(20 * App.screenHeightAdapter));

            Label fgpNumberLabel = new Label
            {

                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.topColor,
                LineBreakMode = LineBreakMode.NoWrap,
                FontSize = App.formValueFontSize
            };


            if ((App.member.number_fnkp == "") | (App.member.number_fnkp.ToLower() == "sim"))
            {
                fgpNumberLabel.Text = "";
            }
            else
            {
                fgpNumberLabel.Text = "Nº " + App.member.number_fnkp;
            }

            currentQuotasRelativeLayout.Children.Add(fgpNumberLabel,
                xConstraint: Constraint.Constant(10 * App.screenWidthAdapter),
                yConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height) - 30 * App.screenHeightAdapter; // center of image (which is 40 wide)
                }),
                widthConstraint: Constraint.Constant(100 * App.screenWidthAdapter),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));


            Label adcpnNumberLabel = new Label
            {
				
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.topColor,
                LineBreakMode = LineBreakMode.NoWrap,
                FontSize = App.formValueFontSize,
            };

           
            if (App.member.number_member == "")
            {
                adcpnNumberLabel.Text = "";
            }
            else
            {
                adcpnNumberLabel.Text = "Nº " + App.member.number_member;
            }

            currentQuotasRelativeLayout.Children.Add(adcpnNumberLabel,
                xConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width) - 120 * App.screenWidthAdapter; // center of image (which is 40 wide)
                }),
                yConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height) - 30 * App.screenHeightAdapter; // center of image (which is 40 wide)
                }),
                widthConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
                heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

            quotasRelativeLayout.Children.Add(quotasFrame,
                xConstraint: Constraint.Constant(50 * App.screenWidthAdapter),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width) - 100 * App.screenWidthAdapter; // center of image (which is 40 wide)
                }),
                heightConstraint: Constraint.Constant(140 * App.screenHeightAdapter));
        }

        public async void CreatePastQuotas()
        {
            var result = await GetPastFees(App.member);

            Label historicoQuotasLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
            historicoQuotasLabel.Text = "HISTÓRICO QUOTAS";

            quotasRelativeLayout.Children.Add(historicoQuotasLabel,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(150 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width);
                }),
                heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter));

            //COLLECTION GRADUACOES
            collectionViewPastQuotas = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = App.member.pastFees,
                ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
                EmptyView = new ContentView
                {
                    Content = new StackLayout
                    {
                        Children =
                            {
                                new Label { Text = "Não existem quotas anteriores.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.topColor, FontSize = App.itemTitleFontSize },
                            }
                    }
                }
            };

            GradientBrush gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1),
            };

            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 143, 86), Convert.ToSingle(0)));
            gradient.GradientStops.Add(new GradientStop(Color.FromRgb(246, 220, 178), Convert.ToSingle(0.5)));

            collectionViewPastQuotas.SelectionChanged += OncollectionViewFeeSelectionChangedAsync;

            collectionViewPastQuotas.ItemTemplate = new DataTemplate(() =>
            {
                RelativeLayout itemRelativeLayout = new RelativeLayout
                {
                    //HeightRequest = 100
                };

                Frame itemFrame = new Frame
                {
                    CornerRadius = 5,
                    IsClippedToBounds = true,
                    BorderColor = App.topColor,
                    BackgroundColor = Color.Transparent,
                    Padding = new Thickness(2, 2, 2, 2),
                    HeightRequest = 30 * App.screenHeightAdapter,
                    VerticalOptions = LayoutOptions.Center,
					HasShadow = false,
                };

                itemFrame.Content = itemRelativeLayout;

               /* Label periodLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                periodLabel.SetBinding(Label.TextProperty, "periodo");

                itemRelativeLayout.Children.Add(periodLabel,
                    xConstraint: Constraint.Constant(5),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.Constant(70 * App.screenWidthAdapter),
                    heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));*/

                Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                nameLabel.SetBinding(Label.TextProperty, "tipo_desc");

                itemRelativeLayout.Children.Add(nameLabel,
                    xConstraint: Constraint.Constant(5 * App.screenWidthAdapter),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return (parent.Width - (30 * App.screenWidthAdapter));
                    }),
                    heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

                Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
                participationImagem.Source = "iconcheck.png";

                itemRelativeLayout.Children.Add(participationImagem,
                    xConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return (parent.Width - (30 * App.screenWidthAdapter));
                    }),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
                    heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

                return itemFrame;
            });

            quotasRelativeLayout.Children.Add(collectionViewPastQuotas,
            xConstraint: Constraint.Constant(0),
            yConstraint: Constraint.Constant(200 * App.screenHeightAdapter),
            widthConstraint: Constraint.RelativeToParent((parent) =>
            {
                return (parent.Width);
            }),
            heightConstraint: Constraint.RelativeToParent((parent) =>
            {
                return (parent.Height) - (190 * App.screenHeightAdapter);
            }));

        }


        public DoPageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfilePageCS());
		}

		async Task<List<Competition_Participation>> GetPastCompetitionParticipations()
		{
			Debug.WriteLine("GetPastCompetitionParticipations");
			CompetitionManager competitionManager = new CompetitionManager();

			List<Competition_Participation> pastCompetitionParticipations = await competitionManager.GetPastCompetitionParticipations(App.member.id);


			Debug.WriteLine("GetPastCompetitionParticipations pastCompetitionParticipations.count "+ pastCompetitionParticipations.Count);
			return pastCompetitionParticipations;
		}

		async Task<List<Event_Participation>> GetPastEventParticipations()
		{
			Debug.WriteLine("GetPastCompetitionParticipations");
			EventManager eventManager = new EventManager();

			List<Event_Participation> pastEventParticipations = await eventManager.GetPastEventParticipations(App.member.id);

			return pastEventParticipations;
		}


		async void OnPremiosButtonClicked(object sender, EventArgs e)
		{
			App.DO_activetab = "premios";
			premiosButton.activate();
			palmaresButton.deactivate();
			participacoesEventosButton.deactivate();
            quotasButton.deactivate();

            relativeLayout.Children.Add(premiosRelativeLayout,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(80),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 80;
				}));

			relativeLayout.Children.Remove(palmaresRelativeLayout);
			relativeLayout.Children.Remove(collectionViewParticipacoesEventos);
            relativeLayout.Children.Remove(quotasRelativeLayout);
        }

		async void OnPalmaresButtonClicked(object sender, EventArgs e)
		{
			App.DO_activetab = "palmares";
			premiosButton.deactivate();
			palmaresButton.activate();
			participacoesEventosButton.deactivate();
            quotasButton.deactivate();

            relativeLayout.Children.Add(palmaresRelativeLayout,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(80),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height-80);
				}));

			relativeLayout.Children.Remove(premiosRelativeLayout);
			relativeLayout.Children.Remove(collectionViewParticipacoesEventos);
            relativeLayout.Children.Remove(quotasRelativeLayout);
        }

		async void OnParticipacoesEventosButtonClicked(object sender, EventArgs e)
		{
			App.DO_activetab = "participacoesevento";
			premiosButton.deactivate();
			palmaresButton.deactivate();
			participacoesEventosButton.activate();
            quotasButton.deactivate();

            relativeLayout.Children.Add(collectionViewParticipacoesEventos,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(80),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width);
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height) - 80;
					}));


			relativeLayout.Children.Remove(premiosRelativeLayout);
			relativeLayout.Children.Remove(palmaresRelativeLayout);
            relativeLayout.Children.Remove(quotasRelativeLayout);
        }

        async void OnQuotasButtonClicked(object sender, EventArgs e)
        {
            App.DO_activetab = "quotas";
            premiosButton.deactivate();
            palmaresButton.deactivate();
            participacoesEventosButton.deactivate();
            quotasButton.activate();

            relativeLayout.Children.Add(quotasRelativeLayout,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(80),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width);
                }),
                heightConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height) - 80;
                }));


            relativeLayout.Children.Remove(premiosRelativeLayout);
            relativeLayout.Children.Remove(palmaresRelativeLayout);
            relativeLayout.Children.Remove(collectionViewParticipacoesEventos);
        }

        async void OnCollectionViewParticipacoesEventosSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewParticipacoesEventosSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				Event_Participation event_participation = (sender as CollectionView).SelectedItem as Event_Participation;
				await Navigation.PushAsync(new DetailEventParticipationPageCS(event_participation));
			}
		}

		async void OnCollectionViewParticipacoesCompeticoesSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewParticipacoesCompeticoesSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				Competition_Participation competition_participation = (sender as CollectionView).SelectedItem as Competition_Participation;
				await Navigation.PushAsync(new DetailCompetitionResultPageCS(competition_participation));
			}
		}

		async Task<int> GetAwards_Student(Member member)
		{
			Debug.WriteLine("GetAwards_Student");
			AwardManager awardManager = new AwardManager();

			awards = await awardManager.GetAwards_Student(member.id);
			if (awards == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return -1;
			}

			foreach (Award award in awards)
			{
				if (award.tipo == "premio_diploma_de_merito_desportivo")
				{
					award.imagem = "premioadcpndourado.png";
				}
				else if (award.tipo == "premio_Diploma_de_merito_associativo")
                {
					award.imagem = "premioadcnpdouradosemginasta.png";
				}
                else if (award.tipo == "distincoes_socio_honorario")
                {
                    award.imagem = "premioadcnpverdesemginasta.png";
                }
                else if (award.tipo == "distincoes_emblema_comemorativo")
                {
                    award.imagem = "premioadcnazulsemginasta.png";
                }
            }

			return 1;
		}


        async Task<int> GetCurrentFees(Member member)
        {
            Debug.WriteLine("GetCurrentFees");
            MemberManager memberManager = new MemberManager();

            var result = await memberManager.GetCurrentFees(member);
            if (result == -1)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = Color.White,
                    BarTextColor = Color.Black
                };
                return -1;
            }
            return result;
        }

        async Task<int> GetPastFees(Member member)
        {
            Debug.WriteLine("GetPastFees");
            MemberManager memberManager = new MemberManager();

            var result = await memberManager.GetPastFees(member);
            if (result == -1)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = Color.White,
                    BarTextColor = Color.Black
                };
                return -1;
            }
            return result;
        }

        void OncollectionViewFeeSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            Debug.Print("OncollectionViewFeeSelectionChangedAsync");

            if ((sender as CollectionView).SelectedItem != null)
            {
                Fee selectedFee = (sender as CollectionView).SelectedItem as Fee;

                InvoiceDocument(selectedFee);

                (sender as CollectionView).SelectedItem = null;
            }
            else
            {
                Debug.WriteLine("OncollectionViewMonthFeesSelectionChanged selected item = nulll");
            }
        }

        public async void InvoiceDocument(Fee fee)
        {
            Payment payment = await GetFeePaymentAsync(fee);
			Debug.Print("payment.invoiceid = " + payment.invoiceid);
            if (payment.invoiceid != null)
            {
                await Navigation.PushAsync(new InvoiceDocumentPageCS(payment));
            }
        }


        public async Task<Payment> GetFeePaymentAsync(Fee fee)
        {
            Debug.WriteLine("GetFeePayment");
            MemberManager memberManager = new MemberManager();

            List<Payment> result = await memberManager.GetFeePayment(fee.id);
            if (result == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = Color.White,
                    BarTextColor = Color.Black
                };
				return null;
            }
            return result[0];
        }
    }
}
