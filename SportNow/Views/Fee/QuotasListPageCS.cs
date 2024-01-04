using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
//Ausing Acr.UserDialogs;
using SportNow.Views.Profile;
//using static System.Net.Mime.MediaTypeNames;

namespace SportNow.Views
{
	public class QuotasListPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}
		private RelativeLayout quotasRelativeLayout;


		private StackLayout stackButtons;

		private CollectionView collectionViewPastQuotas;

		private List<Fee> pastQuotas;

		Image estadoQuotaImage;

		public void initLayout()
		{
			Title = "QUOTAS";

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
				relativeLayout.Children.Remove(quotasRelativeLayout);

				stackButtons = null;
				collectionViewPastQuotas = null;
			}
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

			CreateQuotas();

			hideActivityIndicator();
		}


		public void CreateQuotas() {
			quotasRelativeLayout = new RelativeLayout
			{
				Margin = new Thickness(5)
			};

			CreateCurrentQuota();
			CreatePastQuotas();

			relativeLayout.Children.Add(quotasRelativeLayout,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (80 * App.screenHeightAdapter);
				}));
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
				BorderColor = Color.FromRgb(182, 145, 89),
				BackgroundColor = Color.Transparent,
				Padding = new Thickness(2, 2, 2, 2),
				HeightRequest = 120*App.screenHeightAdapter,
				VerticalOptions = LayoutOptions.Center,
			};

			var tapGestureRecognizer_quotasFrame = new TapGestureRecognizer();
			tapGestureRecognizer_quotasFrame.Tapped += async (s, e) => {
				await Navigation.PushAsync(new QuotasPageCS());
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
				logoFeeFileName = "fnkpikp.png";
				estadoImageFileName = "iconcheck.png";
			}
			else if (hasQuotaPayed == false)
			{
				logoFeeFileName = "fnkpikp.png";
				estadoImageFileName = "iconinativo.png";
			}

            Xamarin.Forms.Image LogoFee = new Xamarin.Forms.Image
            {
				Source = logoFeeFileName,
				//WidthRequest = 100,
				HorizontalOptions = LayoutOptions.Center
			};
			currentQuotasRelativeLayout.Children.Add(LogoFee,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 2) - 78 * App.screenHeightAdapter;
				}),
				yConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter));

			estadoQuotaImage = new Xamarin.Forms.Image
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

			/*
            Label fgpNumberLabel = new Label
            {
                
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.	normalTextColor,
                LineBreakMode = LineBreakMode.NoWrap,
                FontSize = App.formValueFontSize
            };


			if (App.member.number_fnkp == "")
			{
				fgpNumberLabel.Text = "";
			}
			else
			{
                fgpNumberLabel.Text = "Nº " + App.member.number_fnkp;
            }

            currentQuotasRelativeLayout.Children.Add(fgpNumberLabel,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height) - 30 * App.screenHeightAdapter; // center of image (which is 40 wide)
                }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width); // center of image (which is 40 wide)
                }),
                heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));
			*/

            Label feeLabel = new Label
			{
				Text = "QUOTAS",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.itemTitleFontSize
			};
			currentQuotasRelativeLayout.Children.Add(feeLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 30 * App.screenHeightAdapter; // center of image (which is 40 wide)
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

			quotasRelativeLayout.Children.Add(quotasFrame,
				xConstraint: Constraint.Constant(50 * App.screenWidthAdapter),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width) - 100 * App.screenWidthAdapter; // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(120 * App.screenHeightAdapter));
		}

		public async void CreatePastQuotas()
		{			
			var result = await GetPastFees(App.member);

			Label historicoQuotasLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = Color.FromRgb(182, 145, 89), LineBreakMode = LineBreakMode.WordWrap };
			historicoQuotasLabel.Text = "HISTÓRICO QUOTAS";

			quotasRelativeLayout.Children.Add(historicoQuotasLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(130 * App.screenHeightAdapter),
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
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5,  },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem quotas anteriores.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
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
					HeightRequest=100
				};

				Frame itemFrame = new Frame
				{
					CornerRadius = 5,
					IsClippedToBounds = true,
					BorderColor = Color.FromRgb(182, 145, 89),
					BackgroundColor = Color.Transparent,
					Padding = new Thickness(2, 2, 2, 2),
					HeightRequest = 30 * App.screenHeightAdapter,
					VerticalOptions = LayoutOptions.Center,
				};

				itemFrame.Content = itemRelativeLayout;

				Label periodLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				periodLabel.SetBinding(Label.TextProperty, "periodo");

				itemRelativeLayout.Children.Add(periodLabel,
					xConstraint: Constraint.Constant(5),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(30 * App.screenWidthAdapter),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "tipo_desc");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(35 * App.screenWidthAdapter),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (40 * App.screenWidthAdapter)); 
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
			yConstraint: Constraint.Constant(170 * App.screenHeightAdapter),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width); // center of image (which is 40 wide)
			}),
			heightConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height) - (170 * App.screenHeightAdapter);
			}));

		}


		public QuotasListPageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfilePageCS());
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
				return result[0];
			}
			return result[0];
		}

	}
}
