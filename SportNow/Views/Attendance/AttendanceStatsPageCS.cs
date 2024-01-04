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
	public class AttendanceStatsPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private RelativeLayout graphRelativeLayout;


		private StackLayout stackButtons;

		private List<Class_Schedule> pastclass_schedules, pastclass_schedules_dummy;
		private CollectionView classesCollectionView;

		SfChart chart;
		string centerViewText;
		Label centerView_Label;
		DoughnutSeries series;
		Attendance_Stats pastClass_Attendances;

		public void initLayout()
		{
			Title = "PRESENÇAS";


		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover

		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();
			DateTime currentTime = DateTime.Now.Date;
            string firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]).ToString("yyyy-MM-dd");
			string currentTime_string = currentTime.ToString("yyyy-MM-dd");
			Debug.Print("firstDayWeek = " + firstDayWeek+ " currentTime_string = "+ currentTime_string);
			pastclass_schedules = await GetStudentClass_Schedules(firstDayWeek, currentTime_string);
			pastclass_schedules_dummy = new List<Class_Schedule>();
			CompleteClass_Schedules();

			CreatePeriodSelection();
			createReport();
			CreateClassesColletion();

			hideActivityIndicator();

		}

		public void CompleteClass_Schedules()
		{
			foreach (Class_Schedule class_schedule in pastclass_schedules)
			{
				DateTime class_schedule_date = DateTime.Parse(class_schedule.date).Date;

				class_schedule.datestring = Constants.daysofWeekPT[class_schedule_date.DayOfWeek.ToString()] + " - "
					+ class_schedule_date.Day + " "
					+ Constants.months[class_schedule_date.Month] + "\n"
					+ class_schedule.begintime + " às " + class_schedule.endtime;
				//class_schedule.imagesource = "logo_aksl.png";

				if (class_schedule.imagesource == null)
				{
					class_schedule.imagesourceObject = "logo_login.png";
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

				if (class_schedule.classattendancestatus == "fechada")
                {
					class_schedule.participationimage = "iconcheck.png";
				}
				else
                {
					class_schedule.participationimage = "iconinativo.png";
				}
					
			}
		}


		public void CreatePeriodSelection()
		{

			Label periodLabel = new Label
			{
				BackgroundColor = Color.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.bigTitleFontSize,
				TextColor = App.normalTextColor,
				Text = "Escolha o Periodo"
			};

			var periodList = new List<string>();
			periodList.Add("ESTA SEMANA");
			periodList.Add("ESTE MÊS");
			periodList.Add("ÚLTIMA SEMANA");
			periodList.Add("ÚLTIMO MÊS");
			//periodList.Add("DESDE ÚLTIMO EXAME");

			var periodPicker = new Picker
			{
				Title = "",
				TitleColor = App.topColor,
				BackgroundColor = Color.Transparent,
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.itemTitleFontSize

			};
			periodPicker.ItemsSource = periodList;
			periodPicker.SelectedIndex = 0;

			periodPicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{
				Debug.Print("periodPicker.SelectedItem.ToString() = " + periodPicker.SelectedItem.ToString());
				string begindate = "";
				string enddate = "";
				if (periodPicker.SelectedItem.ToString() == "ESTA SEMANA")
				{
					DateTime currentTime = DateTime.Now.Date;
					begindate = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]).ToString("yyyy-MM-dd");
					enddate = currentTime.ToString("yyyy-MM-dd");
				}
				else if (periodPicker.SelectedItem.ToString() == "ESTE MÊS")
				{
					DateTime currentTime = DateTime.Now.Date;
					begindate = currentTime.ToString("yyyy-MM-") + "01";
					enddate = currentTime.ToString("yyyy-MM-dd");
					//searchDate = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]).ToString("yyyy-mm-dd");
				}
				else if (periodPicker.SelectedItem.ToString() == "ÚLTIMA SEMANA")
				{
					DateTime lastWeek = DateTime.Now.AddDays(-7);
					DateTime firstdayLastWeek = lastWeek.AddDays(-Constants.daysofWeekInt[lastWeek.DayOfWeek.ToString()]);
					DateTime lastdayLastWeek = firstdayLastWeek.AddDays(7);

					begindate = firstdayLastWeek.ToString("yyyy-MM-dd");
					enddate = lastdayLastWeek.ToString("yyyy-MM-dd");
				}
				else if (periodPicker.SelectedItem.ToString() == "ÚLTIMO MÊS")
				{
					DateTime lastMonth = DateTime.Now.AddMonths(-1).Date;
					DateTime fisrtDayMonth = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-") + "01").Date;

					begindate = lastMonth.ToString("yyyy-MM-") + "01";
					enddate = fisrtDayMonth.AddDays(-1).ToString("yyyy-MM-dd");

					//searchDate = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]).ToString("yyyy-mm-dd");
				}
				Debug.Print("begindate = " + begindate + " enddate = " + enddate);
				showActivityIndicator();

				pastclass_schedules = await GetStudentClass_Schedules(begindate, enddate);
				CompleteClass_Schedules();
				classesCollectionView.ItemsSource = pastclass_schedules_dummy;
				classesCollectionView.ItemsSource = pastclass_schedules;


				chart.BindingContext = new Attendance_Stats(pastclass_schedules_dummy);
				pastClass_Attendances = new Attendance_Stats(pastclass_schedules);
				chart.BindingContext = pastClass_Attendances;

				centerViewText = pastClass_Attendances.Data[0].class_count_presente.ToString() + "\n TOTAL";
				centerView_Label = new Label() { Text = centerViewText, FontSize = 25, TextColor = App.normalTextColor, HorizontalTextAlignment = TextAlignment.Center };
				series.CenterView = centerView_Label;

                //CreatePastClassesColletion();
                //createReport();

                hideActivityIndicator();

            };
			
			relativeLayout.Children.Add(periodLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter));

			relativeLayout.Children.Add(periodPicker,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(40 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter));
		}

		public void createReport()
		{
			graphRelativeLayout = new RelativeLayout
			{
				Margin = new Thickness(5)
			};

			createChart();

			graphRelativeLayout.Children.Add(chart,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 10;
				}));

			BoxView separator = new BoxView()
			{
				HeightRequest = 1,
				BackgroundColor = App.topColor
			};

			graphRelativeLayout.Children.Add(separator,
				xConstraint: Constraint.Constant(30),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 10;
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 60); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(1));

			relativeLayout.Children.Add(graphRelativeLayout,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(80 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (((parent.Height - 80 * App.screenHeightAdapter) * 3 / 5));
				}));
		}

		public SfChart createChart()
		{
			chart = new SfChart();

			if (Device.RuntimePlatform == Device.Android)
			{
				chart.BackgroundColor = Color.White;
			}

			//Initializing Primary Axis
			CategoryAxis primaryAxis = new CategoryAxis();
			primaryAxis.Title.Text = "Aulas";
			chart.PrimaryAxis = primaryAxis;

			NumericalAxis secondaryAxis = new NumericalAxis();
			secondaryAxis.Title.Text = "#";
			chart.SecondaryAxis = secondaryAxis;

			Attendance_Stats pastClass_Attendances = new Attendance_Stats(pastclass_schedules);
			this.BindingContext = pastClass_Attendances;

			//Initializing column series

			series = new DoughnutSeries()
			{
				ColorModel = new ChartColorModel()
				{
					Palette = ChartColorPalette.Custom,
					CustomBrushes = new ChartColorCollection()
								{
									Color.FromRgb(246, 220, 178),
									Color.Green,
									Color.GreenYellow,
									Color.DarkSeaGreen,
									Color.LimeGreen
								}
				},
				StartAngle = -90,
				EndAngle = 270
			};

			centerViewText = pastClass_Attendances.Data[0].class_count_presente.ToString() + "\n TOTAL";
			centerView_Label = new Label() { Text = centerViewText, FontSize = 25, TextColor = App.normalTextColor, HorizontalTextAlignment = TextAlignment.Center };
			series.CenterView = centerView_Label;


			series.IsStackedDoughnut = true;
			series.CapStyle = DoughnutCapStyle.BothCurve;
			series.Spacing = 0.5;
			series.MaximumValue = 100;
			series.TrackBorderColor = Color.FromRgb(200, 200, 200);
			series.TrackColor = Color.FromRgb(200, 200, 200);
			series.TrackBorderWidth = 0;
			series.DoughnutCoefficient = 0.60;
			

			//series.DoughnutCoefficient = 0.2;

			series.SetBinding(ChartSeries.ItemsSourceProperty, "Data");
			series.XBindingPath = "name";
			series.YBindingPath = "attendance_percentage";
			series.DataMarker = new ChartDataMarker();
			chart.EnableSeriesSelection = true;
			chart.SeriesSelectionColor = Color.Red;

			chart.Series.Add(series);

			ChartLegend legend = new ChartLegend()
			{
				OverflowMode = ChartLegendOverflowMode.Wrap,
				DockPosition = LegendPlacement.Bottom,
			};
			//chart.Legend.ToggleSeriesVisibility = true;
			legend.Title.Text = "Aula";
			legend.Title.TextColor = App.normalTextColor;
			legend.ToggleSeriesVisibility = true;

			/*chart.LegendItemClicked += async (object sender, EventArgs e) =>
			{

			}*/

			chart.Legend = legend;


			return chart;
		}

		public void CreateClassesColletion()
		{
			//COLLECTION GRADUACOES
			classesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Multiple,
				ItemsSource = pastclass_schedules,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existiram aulas neste periodo.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = 20 },
							}
					}
				}
			};

			classesCollectionView.SelectionChanged += OnClassesCollectionViewSelectionChanged;

			classesCollectionView.ItemTemplate = new DataTemplate(() =>
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

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.4, BackgroundColor = Color.White }; //, HeightRequest = 60, WidthRequest = 60
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
					yConstraint: Constraint.Constant(3 * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant((20 * App.screenHeightAdapter)),
					heightConstraint: Constraint.Constant((20 * App.screenHeightAdapter)));

				return itemRelativeLayout;
			});
			relativeLayout.Children.Add(classesCollectionView,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (((parent.Height - 80 * App.screenHeightAdapter) * 3 / 5) + (80 * App.screenHeightAdapter));
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height - (80 * App.screenHeightAdapter)) * 2 / 5; // 
				}));


		}

		public AttendanceStatsPageCS()
		{
			this.initLayout();
		}

		async Task<List<Class_Schedule>> GetStudentClass_Schedules(string begindate, string enddate)
		{
			ClassManager classManager = new ClassManager();
			List<Class_Schedule> class_schedules_i = await classManager.GetStudentClass_Schedules(App.member.id, begindate, enddate);
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


		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfilePageCS());
		}


		async void OnClassesCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

	}
}
