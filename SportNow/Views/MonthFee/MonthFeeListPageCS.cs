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
	public class MonthFeeListPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
            Debug.Print("MonthFeeListPageCS - OnAppearing");
            initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}
		private RelativeLayout quotasRelativeLayout;

		private CollectionView monthFeesCollectionView;

		public Label currentMonth;

		public StackLayout stackMonthSelector;

		Image estadoQuotaImage;

		DateTime selectedTime;

		ObservableCollection<MonthFee> monthFees;

		Picker classPicker;

        Class_Schedule class_Schedule;

        RoundButton approveSelectedButton, approveAllButton;

		public void initLayout()
		{
			Title = "MENSALIDADES";
		}


		public void CleanScreen()
		{
			Debug.Print("MonthFeeListPageCS CleanScreen");

            relativeLayout.Children.Remove(classPicker);
            relativeLayout.Children.Remove(monthFeesCollectionView);
			monthFeesCollectionView = null;
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

			CreateMonthSelector();
			//_ = await CreateClassPicker();
            _ = await CreateClassPicker();
            monthFees = await GetMonthFeesbyClass();
			CreateMonthFeesColletion();

            hideActivityIndicator();
        }


        public async Task<int> CreateClassPicker()
        {
			Debug.Print("CreateClassPicker - begin");
            List<string> classList = new List<string>();
            classList.Add("Todas as Classes");
            List<Class_Detail> classes = await GetAllClasses();
            Debug.Print("CreateClassPicker - after GetAllClasses");
            int selectedIndex = 0;
            int selectedIndex_temp = 0;

            foreach (Class_Detail class_detail in classes)
            {
				Debug.Print("CreateClassPicker class_detail.name = " + class_detail.name);
                classList.Add(class_detail.name);
               /* if (class_detail.name == class_Schedule.name)
                {
                    selectedIndex = selectedIndex_temp;
                }
                selectedIndex_temp++;*/
            }


            Debug.Print("selectedIndex = " + selectedIndex);

            classPicker = new Picker
            {
                Title = "",
                TitleColor = App.topColor,
                BackgroundColor = Color.Transparent,
                TextColor = App.topColor,
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = App.titleFontSize

            };
            classPicker.ItemsSource = classList;
            classPicker.SelectedIndex = selectedIndex;

            classPicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
            {
                showActivityIndicator();

                monthFees = await GetMonthFeesbyClass();

                monthFeesCollectionView.ItemsSource = monthFees;

                hideActivityIndicator();
            };

            relativeLayout.Children.Add(classPicker,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width); // center of image (which is 40 wide)
                }),
                heightConstraint: Constraint.Constant(50));

			return 0;
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
                    BackgroundColor = Color.FromRgb(25, 25, 25),
					VerticalOptions = LayoutOptions.Center
				};
			}


			previousMonthButton.Clicked += OnPreviousButtonClicked;

			currentMonth = new Label()
			{
				Text = selectedTime.Year + " - " + selectedTime.Month,
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
                    BackgroundColor = Color.FromRgb(25, 25, 25),
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
			yConstraint: Constraint.Constant(40 * App.screenHeightAdapter),
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
								new Label { Text = "Não existem Mensalidades desta Classe para este mês.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
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

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.gridTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "membernickname");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(5),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (225 * App.screenWidthAdapter)); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));



                Label classLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.gridTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                classLabel.SetBinding(Label.TextProperty, "classname");

                itemRelativeLayout.Children.Add(classLabel,
                    xConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return (parent.Width - (250 * App.screenWidthAdapter)); // center of image (which is 40 wide)
                    }),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.Constant(100 * App.screenWidthAdapter),
                    heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));


                Label statusLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.gridTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
                statusLabel.SetBinding(Label.TextProperty, "status");
                statusLabel.SetBinding(Label.TextColorProperty, "selectedColor");

                itemRelativeLayout.Children.Add(statusLabel,
                    xConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return (parent.Width - (140 * App.screenWidthAdapter)); // center of image (which is 40 wide)
                    }),
                    yConstraint: Constraint.Constant(0),
                    widthConstraint: Constraint.Constant(70 * App.screenWidthAdapter),
                    heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));


                Label valueLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.End, FontSize = App.gridTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				valueLabel.SetBinding(Label.TextProperty, "value");

				itemRelativeLayout.Children.Add(valueLabel,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (65 * App.screenWidthAdapter)); // center of image (which is 40 wide)
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(60 * App.screenWidthAdapter),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

                return itemFrame;
			});

			relativeLayout.Children.Add(monthFeesCollectionView,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (170 * App.screenHeightAdapter); // 
				}));


		}

		public void createApproveButtons()
		{
            /*approveSelectedButton = new Button
			{
				Text = "APROVAR SELECCIONADOS",
				BackgroundColor = Color.FromRgb(96, 182, 89),
				TextColor = Color.White,
				FontSize = App.itemTitleFontSize,
				WidthRequest = 100,
				HeightRequest = 50
			};

			Frame frame_approveSelectedButton = new Frame
			{
				BorderColor = Color.FromRgb(96, 182, 89),
				WidthRequest = 100,
				HeightRequest = 50,
				CornerRadius = 10,
				IsClippedToBounds = true,
				Padding = 0
			};

			frame_approveSelectedButton.Content = approveSelectedButton;
			approveSelectedButton.Clicked += approveSelectedButtonClicked;

			relativeLayout.Children.Add(frame_approveSelectedButton,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 60; // 
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width/2)-5; // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(50)
			);*/

            approveAllButton = new RoundButton("APROVAR TODOS", 100, 40);

			approveAllButton.button.Clicked += approveAllButtonClicked;

			relativeLayout.Children.Add(approveAllButton,
				xConstraint: Constraint.Constant(5),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 60; // 
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width) - 10; // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(50)
			);

		}

		public MonthFeeListPageCS()
		{
			Debug.Print("MonthFeeListPageCS - Begin");
			this.initLayout();
			//this.initSpecificLayout();

		}

        async Task<List<Class_Detail>> GetAllClasses()
        {
            ClassManager classManager = new ClassManager();
            List<Class_Detail> classes = await classManager.GetAllClasses("1");

            return classes;
        }


        async void OnPreviousButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			selectedTime = selectedTime.AddMonths(-1);
			currentMonth.Text = selectedTime.Year + " - " + selectedTime.Month;

			monthFees = await GetMonthFeesbyClass();

			monthFeesCollectionView.ItemsSource = monthFees;

            hideActivityIndicator();
        }

		async void OnNextButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			Debug.Print("selectedTime antes = " + selectedTime.ToShortDateString());
			selectedTime = selectedTime.AddMonths(1);
			Debug.Print("selectedTime = " + selectedTime.ToShortDateString());
			currentMonth.Text = selectedTime.Year + " - " + selectedTime.Month;
			monthFees = await GetMonthFeesbyClass();

			monthFeesCollectionView.ItemsSource = monthFees;


            hideActivityIndicator();


        }

		async Task<ObservableCollection<MonthFee>> GetMonthFeesbyClass()
		{
			/*if (classPicker == null)
			{
				Debug.Print("classPicker == null");
				return null;
			}*/
            //Debug.WriteLine("GetMonthFeesbyClass " + classPicker.SelectedItem.ToString());
            MonthFeeManager monthFeeManager = new MonthFeeManager();
			ObservableCollection<MonthFee> monthFees =	await monthFeeManager.GetMonthFeesbyClass(classPicker.SelectedItem.ToString(), selectedTime.Year.ToString(), selectedTime.Month.ToString());
			if (monthFees == null)
			{
				Debug.Print("MonthFeeListPageCS - GetMonthFeesbyClass - monthFees == null");

                Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
            Debug.Print("MonthFeeListPageCS - GetMonthFeesbyClass - monthFees != null");
            monthFees = correctMonthFees(monthFees);

			return monthFees;
		}


		public ObservableCollection<MonthFee> correctMonthFees(ObservableCollection<MonthFee> monthFees)
		{
			bool hasMonthFeesToApprove = false;

			Debug.Print("correctMonthFees");

			foreach (MonthFee monthFee in monthFees)
			{
				monthFee.selected = false;
				monthFee.selectedColor = Color.Black;
				Debug.Print("monthFee.status = " + monthFee.status);

				if (monthFee.status == "por_aprovar")
				{
					monthFee.selectedColor = Color.Orange;
					monthFee.status = "Por Aprovar";
					hasMonthFeesToApprove = true;
				}
                if (monthFee.status == "emitida_a_pagamento")
                {
                    monthFee.selectedColor = Color.Blue;
                    monthFee.status = "Emitida";
                }
                if (monthFee.status == "emitida")
                {
                    monthFee.selectedColor = Color.Blue;
                    monthFee.status = "Emitida";
                }
                else if (monthFee.status == "emitida_pagamento_em_atraso")
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

			if (hasMonthFeesToApprove == true)
			{
				createApproveButtons();
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
				if (selectedMonthFee.status == "Por Aprovar")
				{
					changeMonthFeeValuePrompt(selectedMonthFee);
				}
				else if ((selectedMonthFee.status == "Emitida") | (selectedMonthFee.status == "Em pagamento") | (selectedMonthFee.status == "Pagamento em Atraso"))
				{
					changeMonthFeeStatusPrompt(selectedMonthFee);
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


		async void changeMonthFeeValuePrompt(MonthFee monthFee)
		{
			showActivityIndicator();
			/*var promptConfig = new PromptConfig();
			promptConfig.InputType = InputType.Default;
			promptConfig.Text = monthFee.value;
			promptConfig.IsCancellable = true;
			promptConfig.Message = "Indica o valor da mensalidade a aplicar a este aluno";
			promptConfig.Title = "Mensalidade";
			promptConfig.OkText = "Ok";
			promptConfig.CancelText = "Cancelar";
			var input = await UserDialogs.Instance.PromptAsync(promptConfig);*/

            string input = await DisplayPromptAsync("Mensalidade", "Indique o valor da mensalidade a aplicar a este aluno");

            if (input != null)
			{
				string new_value = input;
				var charsToRemove = new string[] { "$", "€"};
				foreach (var c in charsToRemove)
				{
					new_value = new_value.Replace(c, string.Empty);
				}

                new_value = new_value.Replace(",", ".");
                Debug.Print("O Valor da Mensalidade é " + new_value);


				MonthFeeManager monthFeeManager = new MonthFeeManager();
				int i = await monthFeeManager.Update_MonthFee_Value_byID(monthFee.id, new_value);
				//monthFee.value = input;

                showActivityIndicator();
                monthFees = await GetMonthFeesbyClass();
                monthFeesCollectionView.ItemsSource = monthFees;
                hideActivityIndicator();

                /*relativeLayout.Children.Remove(monthFeesCollectionView);
				monthFeesCollectionView = null;
				CreateMonthFeesColletion();*/
				/*string global_evaluation = await UpdateExamination_Result(examination_Result.id, input.Text);
				examination_Result.description = input.Text;*/
			}

            hideActivityIndicator();
        }

		async void changeMonthFeeStatusPrompt(MonthFee monthFee)
		{
            			showActivityIndicator();

            bool result = await DisplayAlert("Confirmar Pagamento", "Confirma que pretende colocar esta mensalidade como paga (a FATURA será emitida)?", "Sim", "Não");
            //var result = await UserDialogs.Instance.ConfirmAsync("Confirmas que pretendes colocar esta mensalidade como paga?", "Confirmar Pagamento", "Sim", "Não");
			if (result)
			{
                Payment payment = await GetMonthFeePayment(monthFee);
                MonthFeeManager monthFeeManager = new MonthFeeManager();
				int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "paga", payment.id);
				monthFee.selectedColor = Color.LightGreen;
				monthFee.status = "Paga";
				relativeLayout.Children.Remove(monthFeesCollectionView);
				monthFeesCollectionView = null;
				CreateMonthFeesColletion();
			}
			else
			{
			}

            hideActivityIndicator();
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

		async void approveSelectedButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("approveSelectedButtonClicked");
			showActivityIndicator();
			
			//approveSelectedButton.IsEnabled = false;
			//approveAllButton.IsEnabled = false;


			int selectedMonthFeeCount = 0;

			foreach (MonthFee monthFee in monthFees)
			{
				if (monthFee.selected == true)
				{
					selectedMonthFeeCount++;
					MonthFeeManager monthFeeManager = new MonthFeeManager();
					int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "emitida", null);
				}
			}

			if (selectedMonthFeeCount == 0)
			{
                await DisplayAlert("ESCOLHA VAZIA", "Tens de escolher pelo menos uma mensalidade para aprovar.", "OK");
                //UserDialogs.Instance.Alert(new AlertConfig() { Title = "ESCOLHA VAZIA", Message = "Tens de escolher pelo menos uma mensalidade para aprovar.", OkText = "Ok" });
			}

            //approveSelectedButton.IsEnabled = true;
            //approveAllButton.IsEnabled = true;

            hideActivityIndicator();
        }

		async void approveAllButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("approveAllButtonClicked");
			showActivityIndicator();


			foreach (MonthFee monthFee in monthFees)
			{
				Debug.Print("monthFee "+ monthFee.name + " status = "+ monthFee.status);
				if (monthFee.status == "Por Aprovar")
				{
					MonthFeeManager monthFeeManager = new MonthFeeManager();

					int currentMonth = Convert.ToInt32(DateTime.Now.Month.ToString());
					int currentYear = Convert.ToInt32(DateTime.Now.Year.ToString());

					if (((Convert.ToInt32(monthFee.year) == currentYear) & (Convert.ToInt32(monthFee.month) < currentMonth)) | (Convert.ToInt32(monthFee.year) < currentYear))
					{
						int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "emitida", null);
						monthFee.status = "Emitida";
						monthFee.selectedColor = Color.IndianRed;
					}
					else
					{
						int i = await monthFeeManager.Update_MonthFee_Status_byID(monthFee.id, "emitida", null);
						monthFee.status = "Emitida";
						monthFee.selectedColor = Color.LightBlue;
					}
				}
			}
			relativeLayout.Children.Remove(monthFeesCollectionView);
			monthFeesCollectionView = null;
			CreateMonthFeesColletion();


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
