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

namespace SportNow.Views
{
	public class MonthFeeStudentListPageCS : DefaultPage
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

		private CollectionView monthFeesCollectionView;

		public Label currentMonth;

		public StackLayout stackMonthSelector;

		Image estadoQuotaImage;

		DateTime selectedTime;

		ObservableCollection<MonthFee> monthFees;

		Button approveSelectedButton, approveAllButton;

		public void initLayout()
		{
			Title = "MENSALIDADES";
		}


		public void CleanScreen()
		{
			Debug.Print("MonthFeeStudentListPageCS - CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (quotasRelativeLayout != null)
			{
				relativeLayout.Children.Remove(quotasRelativeLayout);
				monthFeesCollectionView = null;
			}
			if (stackMonthSelector != null)
			{
				relativeLayout.Children.Remove(stackMonthSelector);
				stackMonthSelector = null;
			}
            if (monthFeesCollectionView != null)
            {
                relativeLayout.Children.Remove(monthFeesCollectionView);
                monthFeesCollectionView = null;
            }
        }

		public async void initSpecificLayout()
		{
			showActivityIndicator();

            Label nameMemberLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
            nameMemberLabel.Text = App.member.nickname;

            relativeLayout.Children.Add(nameMemberLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(10),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter));

            CreateMonthSelector();
			monthFees = await GetMonthFeesbyStudent();
			CreateMonthFeesColletion();

            hideActivityIndicator();
        }

		public void CreateMonthSelector()
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width - 50) / 3;

			//DateTime currentTime = DateTime.Now;
			selectedTime = DateTime.Now;



			Button previousMonthButton = new Button();
			Button nextMonthButton = new Button();

			if (Device.RuntimePlatform == Device.iOS)
			{
				previousMonthButton = new Button()
				{
					Text = "<",
					FontSize = App.titleFontSize,
					TextColor = App.topColor,
					VerticalOptions = LayoutOptions.Center
				};
			}
			else if (Device.RuntimePlatform == Device.Android)
			{
				previousMonthButton = new Button()
				{
					Text = "<",
					FontSize = App.titleFontSize,
					TextColor = App.topColor,
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.Center
				};
			}


			previousMonthButton.Clicked += OnPreviousButtonClicked;

			currentMonth = new Label()
			{
				Text = selectedTime.Year.ToString(),
				FontSize = App.titleFontSize,
				TextColor = App.topColor,
				WidthRequest = 150,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};

			if (Device.RuntimePlatform == Device.iOS)
			{
				nextMonthButton = new Button()
				{
					Text = ">",
					FontSize = App.titleFontSize,
					TextColor = App.topColor,
					VerticalOptions = LayoutOptions.Center
				};
			}
			else if (Device.RuntimePlatform == Device.Android)
			{
				nextMonthButton = new Button()
				{
					Text = ">",
					FontSize = App.titleFontSize,
					TextColor = App.topColor,
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.Center
				};
			}

			nextMonthButton.Clicked += OnNextButtonClicked;

			stackMonthSelector = new StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 5 * App.screenHeightAdapter,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40 * App.screenHeightAdapter,
				Children =
				{
					previousMonthButton,
					currentMonth,
					nextMonthButton
				}
			};

			relativeLayout.Children.Add(stackMonthSelector,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(50 * App.screenHeightAdapter),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter));

		}

		public void CreateMonthFeesColletion()
		{
			//COLLECTION GRADUACOES
			monthFeesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = monthFees,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem Mensalidades relativas a este ano.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

			monthFeesCollectionView.SelectionChanged += OncollectionViewMonthFeesSelectionChangedAsync;

			monthFeesCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					HeightRequest = 100
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

				/*Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.gridTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "membernickname");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(5),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (230 * App.screenWidthAdapter)); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));*/


                Label classLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.gridTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                classLabel.SetBinding(Label.TextProperty, "classname");

                itemRelativeLayout.Children.Add(classLabel,
					xConstraint: Constraint.Constant(5),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (220 * App.screenWidthAdapter)); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));


                /*itemRelativeLayout.Children.Add(classLabel,
                    xConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return (parent.Width - (250 * App.screenWidthAdapter)); // center of image (which is 40 wide)
                    }),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.Constant(100 * App.screenWidthAdapter),
                    heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));*/

                Label monthLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.gridTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				monthLabel.SetBinding(Label.TextProperty, "month");

				itemRelativeLayout.Children.Add(monthLabel,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (180 * App.screenWidthAdapter)); // center of image (which is 40 wide)
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(30 * App.screenWidthAdapter),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

                Label divisionLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.gridTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                divisionLabel.SetBinding(Label.TextProperty, "division");

                itemRelativeLayout.Children.Add(divisionLabel,
                    xConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return (parent.Width - (150 * App.screenWidthAdapter)); // center of image (which is 40 wide)
                    }),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.Constant(30 * App.screenWidthAdapter),
                    heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));


                Label statusLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.gridTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                statusLabel.SetBinding(Label.TextProperty, "status");
                statusLabel.SetBinding(Label.TextColorProperty, "selectedColor");

                itemRelativeLayout.Children.Add(statusLabel,
                    xConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return (parent.Width - (120 * App.screenWidthAdapter)); // center of image (which is 40 wide)
                    }),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.Constant(50 * App.screenWidthAdapter),
                    heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

                Label valueLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.End, FontSize = App.gridTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				valueLabel.SetBinding(Label.TextProperty, "value");

				itemRelativeLayout.Children.Add(valueLabel,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (70 * App.screenWidthAdapter)); // center of image (which is 40 wide)
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(65 * App.screenWidthAdapter),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

                return itemFrame;
			});

			relativeLayout.Children.Add(monthFeesCollectionView,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(110 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (130 * App.screenHeightAdapter); // 
				}));


		}

		public MonthFeeStudentListPageCS()
		{
			Debug.Print("MonthFeeStudentListPageCS - Begin");
			this.initLayout();
			//this.initSpecificLayout();

		}


		async void OnPreviousButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
            selectedTime = selectedTime.AddYears(-1);
			currentMonth.Text = selectedTime.Year.ToString();

			monthFees = await GetMonthFeesbyStudent();

			monthFeesCollectionView.ItemsSource = monthFees;

            hideActivityIndicator();
        }

		async void OnNextButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			Debug.Print("selectedTime antes = " + selectedTime.ToShortDateString());
			selectedTime = selectedTime.AddYears(1);
			Debug.Print("selectedTime = " + selectedTime.ToShortDateString());
			currentMonth.Text = selectedTime.Year.ToString();
			monthFees = await GetMonthFeesbyStudent();

			monthFeesCollectionView.ItemsSource = monthFees;


            hideActivityIndicator();


        }

		async Task<ObservableCollection<MonthFee>> GetMonthFeesbyStudent()
		{
			MonthFeeManager monthFeeManager = new MonthFeeManager();
			ObservableCollection<MonthFee> monthFees = await monthFeeManager.GetMonthFeesbyStudent(App.member.id, selectedTime.Year.ToString());
			if (monthFees == null)
			{
				Debug.Print("GetMonthFeesbyStudent monthFees is null");
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}

            Debug.Print("GetMonthFeesbyStudent monthFees is not null");
            monthFees = correctMonthFees(monthFees);

			return monthFees;
		}


		public ObservableCollection<MonthFee> correctMonthFees(ObservableCollection<MonthFee> monthFees)
		{

			foreach (MonthFee monthFee in monthFees)
			{
				monthFee.selected = false;
				monthFee.selectedColor = Color.Black;

				if (monthFee.status == "emitida_a_pagamento")
				{
					monthFee.selectedColor = Color.Blue;
					monthFee.status = "Emitida";
                }
                else if (monthFee.status == "emitida_pagamento_em_atraso")
                {
                    monthFee.selectedColor = Color.Blue;
                    monthFee.status = "Emitida";
                }
                else if (monthFee.status == "emitida")
				{
					monthFee.selectedColor = Color.IndianRed;
					monthFee.status = "Emitida";
				}
            else if (monthFee.status == "paga")
				{
					monthFee.selectedColor = Color.Green;
					monthFee.status = "Paga";
				}
			}

			return monthFees;
		}

		void OncollectionViewMonthFeesSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
		{
			Debug.Print("OncollectionViewMonthFeesSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				MonthFee selectedMonthFee = (sender as CollectionView).SelectedItem as MonthFee;

				Debug.WriteLine("OncollectionViewExaminationSessionCallSelectionChanged selected item = " + selectedMonthFee.membernickname);
				
				if ((selectedMonthFee.status == "Emitida") | (selectedMonthFee.status == "Em pagamento") | (selectedMonthFee.status == "Pagamento em Atraso"))
				{
					payMonthFee(selectedMonthFee);
				}
				else if (selectedMonthFee.status == "Paga")
				{
					InvoiceDocument(selectedMonthFee);
				}
				(sender as CollectionView).SelectedItem = null;
			}
			else
			{
				Debug.WriteLine("OncollectionViewMonthFeesSelectionChanged selected item = nulll");
			}
		}

		public async void InvoiceDocument(MonthFee monthFee)
		{
			Debug.Print("InvoiceDocument");
			Payment payment = await GetMonthFeePayment(monthFee);
			if (payment.invoiceid != null)
			{
				Debug.Print("InvoiceDocument != null");
				if (payment.value != 0)
                {
					Debug.Print("InvoiceDocument != 0");
					await Navigation.PushAsync(new InvoiceDocumentPageCS(payment));
				}
					
			}
		}

		public async Task<bool> checkPreviusUnpaidMonthFeeAsync(MonthFee selectedMonthFee)
		{
			Debug.Print("checkPreviusUnPaidMonthFee");
			foreach (MonthFee monthFee in monthFees)
			{
				Debug.Print("selectedMonthFee = " + selectedMonthFee.name + " " + selectedMonthFee.status + " " + selectedMonthFee.year + " " + selectedMonthFee.month);
				Debug.Print("MonthFee = " + monthFee.name + " " + monthFee.status + " " + monthFee.year + " " + monthFee.month);
				if (((monthFee.status == "Em pagamento") | (monthFee.status == "Pagamento em Atraso"))
					& ((Convert.ToInt32(selectedMonthFee.year) == Convert.ToInt32(monthFee.year)) & (Convert.ToInt32(selectedMonthFee.month) > Convert.ToInt32(monthFee.month)))
						| (Convert.ToInt32(selectedMonthFee.year) > Convert.ToInt32(monthFee.year)))
                {
					Debug.Print("Tem mensalidades anteriores sem pagamento!");

                    bool result = await DisplayAlert("MENSALIDADES EM ATRASO", "Tem mensalidades anteriores em atraso. Confirma que queres pagar esta mensalidade?", "Sim", "Não");
                    //var result = await UserDialogs.Instance.ConfirmAsync("Tens mensalidades anteriores em atraso. Confirmas que queres pagar esta mensalidade?", "MENSALIDADES EM ATRASO", "Sim", "Não");
					return result;
					/*if (result)
					{
						return true;
					}
					else
					{
						return false;
					}*/
				}
			}
			return true;
		}
		

		async void payMonthFee(MonthFee monthFee)
		{
			Debug.Print("payMonthFee");

			bool unpaid = await checkPreviusUnpaidMonthFeeAsync(monthFee);
			if (unpaid == true)
			{
				showActivityIndicator();
				await Navigation.PushAsync(new MonthFeePaymentPageCS(monthFee));
                hideActivityIndicator();
            }
		}

		async void changeMonthFeeStatusPrompt(MonthFee monthFee)
		{
			showActivityIndicator();
            bool result = await DisplayAlert("Confirmar Pagamento", "Confirma que pretende colocar esta mensalidade como paga(a FATURA será emitida)?", "Sim", "Não");
            //var result = await UserDialogs.Instance.ConfirmAsync("Confirmas que pretendes colocar esta mensalidade como paga?", "Confirmar Pagamento", "Sim", "Não");
			if (result)
			{
                Payment payment = await GetMonthFeePayment(monthFee);
                MonthFeeManager monthFeeManager = new MonthFeeManager();
                int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "paga", payment.id);
                monthFee.status = "Paga";
				monthFeesCollectionView.ItemsSource = null;
				monthFeesCollectionView.ItemsSource = monthFees;
			}
			else
			{
			}

            hideActivityIndicator();
        }

		public async Task<Payment> GetMonthFeePayment(MonthFee monthFee)
		{
			Debug.WriteLine("GetMonthFeePayment");
			MonthFeeManager monthFeeManager = new MonthFeeManager();

			List<Payment> result = await monthFeeManager.GetMonthFee_Payment(monthFee.id);
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
