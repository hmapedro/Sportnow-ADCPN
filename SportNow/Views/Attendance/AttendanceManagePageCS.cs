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
using System.Globalization;
using SportNow.ViewModel;
//Ausing Acr.UserDialogs;
using System.Linq;

namespace SportNow.Views
{
	public class AttendanceManagePageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			
			//initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			//this.CleanScreen();
		}

		private RelativeLayout presencasRelativeLayout;


		private StackLayout stackButtons, stackWeekSelector;

		private CollectionView classAttendanceCollectionView;

		private List<Class_Schedule> allClass_Schedules;
		private static List<Class_Schedule> dummyClass_Schedules = new List<Class_Schedule>();

		DateTime firstDayWeek_datetime;
		Label currentWeek;

		public void initLayout()
		{
			Title = "ESCOLHER AULAS";
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (classAttendanceCollectionView != null)
			{
				classAttendanceCollectionView = null;
				relativeLayout.Children.Remove(classAttendanceCollectionView);

			}

		}

		public async void initSpecificLayout()
		{
			App.AdaptScreen();

            showActivityIndicator();

            DateTime currentTime = DateTime.Now.Date;

			firstDayWeek_datetime = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);

			string firstDayWeek = firstDayWeek_datetime.ToString("yyyy-MM-dd");
			string lastdayLastWeek = firstDayWeek_datetime.AddDays(6).ToString("yyyy-MM-dd");
			allClass_Schedules = await GetAllClass_Schedules(firstDayWeek, lastdayLastWeek);
			CompleteClass_Schedules();

			//CreateStackButtons();

			string firstDayWeek_formatted = firstDayWeek_datetime.ToString("dd ") + Constants.months[firstDayWeek_datetime.Month];
			string lastdayLastWeek_formatted = firstDayWeek_datetime.AddDays(6).ToString("dd ") + Constants.months[firstDayWeek_datetime.Month];

            await CreateWeekSelector(firstDayWeek_formatted, lastdayLastWeek_formatted);

            CreateClassesColletion();

            //OnWeek0ButtonClicked(null, null);

            hideActivityIndicator();
        }


		public async Task<string> CreateWeekSelector(string firstDayWeek, string lastdayLastWeek)
		{
            var width = Constants.ScreenWidth;
			var buttonWidth = (width - 50) / 3;

			DateTime currentTime = DateTime.Now;

			Button previousWeekButton = new Button();
			Button nextWeekButton = new Button();

			if (Device.RuntimePlatform == Device.iOS)
			{
				previousWeekButton = new Button()
				{
					Text = "<",
					FontSize = App.titleFontSize,
					TextColor = App.topColor,
					VerticalOptions = LayoutOptions.Center
				};
			}
			else if (Device.RuntimePlatform == Device.Android)
			{
				previousWeekButton = new Button()
				{
					Text = "<",
					FontSize = App.titleFontSize,
					TextColor = App.topColor,
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.Center
				};
			}

			previousWeekButton.Clicked += OnPreviousButtonClicked;

			currentWeek = new Label()
			{
				Text = firstDayWeek + " - " + lastdayLastWeek,
				FontSize = App.titleFontSize,
				TextColor = App.topColor,
                WidthRequest = 150,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};

			if (Device.RuntimePlatform == Device.iOS)
			{
				nextWeekButton = new Button()
				{
					Text = ">",
					FontSize = App.titleFontSize,
					TextColor = App.topColor,
					VerticalOptions = LayoutOptions.Center
				};
			}
			else if (Device.RuntimePlatform == Device.Android)
			{
				nextWeekButton = new Button()
				{
					Text = ">",
					FontSize = App.titleFontSize,
					TextColor = App.topColor,
					BackgroundColor = Color.White,
					VerticalOptions = LayoutOptions.Center
				};
			}

			nextWeekButton.Clicked += OnNextButtonClicked;

            stackWeekSelector = new StackLayout
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
					previousWeekButton,
					currentWeek,
					nextWeekButton
				}
			};


            relativeLayout.Children.Add(stackWeekSelector,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter));

			return "";
        }

		public void CompleteClass_Schedules()
		{
			foreach (Class_Schedule class_schedule in allClass_Schedules)
			{
				DateTime class_schedule_date = DateTime.Parse(class_schedule.date).Date;

				class_schedule.datestring = Constants.daysofWeekPT[class_schedule_date.DayOfWeek.ToString()] + " - "
					+ class_schedule_date.Day + " "
					+ Constants.months[class_schedule_date.Month] + "\n"
					+ class_schedule.begintime + " às " + class_schedule.endtime;

				if (class_schedule.imagesource == null)
				{
					class_schedule.imagesourceObject = "logo_login" +".png";
				}
				else
				{
					class_schedule.imagesourceObject = new UriImageSource
					{
						Uri = new Uri(Constants.images_URL + class_schedule.classid + "_imagem_c"),
						CachingEnabled = true,
						CacheValidity = new TimeSpan(5, 0, 0, 0)
					};
				}
			}
		}

		
		public void CreateClassesColletion()
		{
			//COLLECTION GRADUACOES
			classAttendanceCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = allClass_Schedules,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem Aulas agendadas nesta semana.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

			classAttendanceCollectionView.SelectionChanged += OnClassAttendanceCollectionViewSelectionChanged;

			GradientBrush gradient = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 1),
			};

			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 143, 86), Convert.ToSingle(0)));
			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(246, 220, 178), Convert.ToSingle(0.5)));

			classAttendanceCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					HeightRequest = App.ItemHeight,
					WidthRequest = App.ItemWidth
				};

				Frame itemFrame = new Frame
				{
					CornerRadius = 5 * (float)App.screenWidthAdapter,
					IsClippedToBounds = true,
					BorderColor = Color.Transparent,
					BackgroundColor = Color.Black,
					Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = App.ItemHeight,
					VerticalOptions = LayoutOptions.Center,
				};

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.4 }; //, HeightRequest = 60, WidthRequest = 60
				eventoImage.SetBinding(Image.SourceProperty, "imagesourceObject");

				itemFrame.Content = eventoImage;

				/*var itemFrame_tap = new TapGestureRecognizer();
				itemFrame_tap.Tapped += (s, e) =>
				{
					Navigation.PushAsync(new EquipamentsPageCS("protecoescintos"));
				};
				itemFrame.GestureRecognizers.Add(itemFrame_tap);*/

				itemRelativeLayout.Children.Add(itemFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - 5);
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height);
					}));

				Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 15 * App.screenWidthAdapter, TextColor = Color.White };
				dateLabel.SetBinding(Label.TextProperty, "datestring");

				itemRelativeLayout.Children.Add(dateLabel,
					xConstraint: Constraint.Constant(25 * App.screenWidthAdapter),
					yConstraint: Constraint.Constant(App.ItemHeight - (45 * App.screenHeightAdapter)),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (50 * App.screenWidthAdapter)); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant((40 * App.screenHeightAdapter)));


				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 20 * App.screenWidthAdapter, TextColor = Color.White };
				nameLabel.SetBinding(Label.TextProperty, "name");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(3 * App.screenWidthAdapter),
					yConstraint: Constraint.Constant(25 * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (6 * App.screenWidthAdapter));
					}),
					heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter));

				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.SetBinding(Image.SourceProperty, "participationimage");

				itemRelativeLayout.Children.Add(participationImagem,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (25 * App.screenHeightAdapter));
					}),
					yConstraint: Constraint.Constant(3),
					widthConstraint: Constraint.Constant((20 * App.screenHeightAdapter)),
					heightConstraint: Constraint.Constant((20 * App.screenHeightAdapter)));

				return itemRelativeLayout;
			});
			relativeLayout.Children.Add(classAttendanceCollectionView,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(50 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 50 * App.screenHeightAdapter; // 
				}));


		}


		public AttendanceManagePageCS()
		{
			this.initLayout();
			initSpecificLayout();
		}



		async void OnWeek0ButtonClicked(object sender, EventArgs e)
		{
			/*week0Button.activate();
			week1Button.deactivate();
			week2Button.deactivate();
			week3Button.deactivate();*/

			//weekClassesCollectionView.ItemsSource = week0Class_Detail;
			//_collection.Items = week0Class_Detail;
		}

		async void OnPreviousButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			firstDayWeek_datetime = firstDayWeek_datetime.AddDays(-7);
			string firstDayWeek_formatted = firstDayWeek_datetime.ToString("dd ") + Constants.months[firstDayWeek_datetime.Month];
			string lastdayLastWeek_formatted = firstDayWeek_datetime.AddDays(6).ToString("dd ") + Constants.months[firstDayWeek_datetime.AddDays(6).Month];

			currentWeek.Text = firstDayWeek_formatted + " - " + lastdayLastWeek_formatted;

			string firstDayWeek = firstDayWeek_datetime.ToString("yyyy-MM-dd");
			string lastdayLastWeek = firstDayWeek_datetime.AddDays(6).ToString("yyyy-MM-dd");

			allClass_Schedules = await GetAllClass_Schedules(firstDayWeek, lastdayLastWeek);
			CompleteClass_Schedules();
			classAttendanceCollectionView.ItemsSource = dummyClass_Schedules;
			classAttendanceCollectionView.ItemsSource = allClass_Schedules;
            hideActivityIndicator();
        }

		async void OnNextButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			firstDayWeek_datetime = firstDayWeek_datetime.AddDays(7);
			
			string firstDayWeek_formatted = firstDayWeek_datetime.ToString("dd ") + Constants.months[firstDayWeek_datetime.Month];
			string lastdayLastWeek_formatted = firstDayWeek_datetime.AddDays(6).ToString("dd ") + Constants.months[firstDayWeek_datetime.AddDays(6).Month];

			currentWeek.Text = firstDayWeek_formatted + " - " + lastdayLastWeek_formatted;

			string firstDayWeek = firstDayWeek_datetime.ToString("yyyy-MM-dd");
			string lastdayLastWeek = firstDayWeek_datetime.AddDays(6).ToString("yyyy-MM-dd");

			allClass_Schedules = await GetAllClass_Schedules(firstDayWeek, lastdayLastWeek);

			CompleteClass_Schedules();
			classAttendanceCollectionView.ItemsSource = dummyClass_Schedules;
			classAttendanceCollectionView.ItemsSource = allClass_Schedules;
            hideActivityIndicator();
        }

		async Task<List<Class_Schedule>> GetAllClass_Schedules(string begindate, string enddate)
		{
			ClassManager classManager = new ClassManager();
			List<Class_Schedule> class_schedules_i = await classManager.GetAllClass_Schedules(App.member.id, begindate, enddate);
			if (class_schedules_i == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return class_schedules_i;
		}


		async void OnClassAttendanceCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("AttendanceManagePageCS OnClassAttendanceCollectionViewSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{
				Class_Schedule class_schedule = (sender as CollectionView).SelectedItem as Class_Schedule;
				classAttendanceCollectionView.SelectedItem = null;
				await Navigation.PushAsync(new AttendanceClassPageCS(class_schedule));
				
			}
		}
	}

}
