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
	public class AttendancePageCS : DefaultPage
	{
		private string week = "current";

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private RelativeLayout presencasRelativeLayout;


		private StackLayout stackButtons;

		private MenuButton week0Button, week1Button, week2Button, week3Button;

		private CollectionView weekClassesCollectionView;

		private List<Class_Schedule> weekClass_Schedule, cleanClass_Schedule;

		public void initLayout()
		{
			Title = "MARCAR AULAS";

			relativeLayout = new RelativeLayout
			{	
				Margin = new Thickness(20)
			};
			Content = relativeLayout;
		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (stackButtons != null)
			{
				relativeLayout.Children.Remove(stackButtons);
				//relativeLayout.Children.Remove(presencasRelativeLayout);

				stackButtons = null;
				//presencasRelativeLayout = null;
			}

		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();
			DateTime currentTime = DateTime.Now.Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);
			int result = await getClass_DetailData(firstDayWeek);
			

			CreateStackButtons();
			CreateClassesColletion();


			if (week == "current")
			{
				OnWeek0ButtonClicked(null, null);
			}
			else if (week == "next")
			{
				OnWeek1ButtonClicked(null, null);
			}

            hideActivityIndicator();
        }

		public void CreateStackButtons()
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width - 50) / 4;

			DateTime currentTime = DateTime.Now;

			Debug.Print("current Time = "+currentTime.ToString());
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);

			var week0ButtonText = firstDayWeek.Day + " " + Constants.months[firstDayWeek.Month] + " - "+ firstDayWeek.AddDays(6).Day + " " + Constants.months[firstDayWeek.AddDays(6).Month];
			var week1ButtonText = firstDayWeek.AddDays(7).Day + " " + Constants.months[firstDayWeek.AddDays(7).Month] + " - " + firstDayWeek.AddDays(13).Day + " " + Constants.months[firstDayWeek.AddDays(13).Month];
			var week2ButtonText = firstDayWeek.AddDays(14).Day + " " + Constants.months[firstDayWeek.AddDays(14).Month] + " - " + firstDayWeek.AddDays(20).Day + " " + Constants.months[firstDayWeek.AddDays(20).Month];
			var week3ButtonText = firstDayWeek.AddDays(21).Day + " " + Constants.months[firstDayWeek.AddDays(21).Month] + " - " + firstDayWeek.AddDays(27).Day + " " + Constants.months[firstDayWeek.AddDays(27).Month];

			week0Button = new MenuButton(week0ButtonText, buttonWidth, 40 * App.screenHeightAdapter);
			week0Button.button.Clicked += OnWeek0ButtonClicked;

			week1Button = new MenuButton(week1ButtonText, buttonWidth, 40 * App.screenHeightAdapter);
			week1Button.button.Clicked += OnWeek1ButtonClicked;

			week2Button = new MenuButton(week2ButtonText, buttonWidth, 40 * App.screenHeightAdapter);
			week2Button.button.Clicked += OnWeek2ButtonClicked;

			week3Button = new MenuButton(week3ButtonText, buttonWidth, 40 * App.screenHeightAdapter);
			week3Button.button.Clicked += OnWeek3ButtonClicked;

			stackButtons = new StackLayout
			{
				Margin = new Thickness(0),
				Spacing = 5,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40 * App.screenHeightAdapter,
				Children =
				{
					week0Button,
					week1Button,
					week2Button,
					week3Button
				}
			};

			relativeLayout.Children.Add(stackButtons,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter));

		}


		public async Task<int> getClass_DetailData(DateTime startDate)
		{

			weekClass_Schedule = await GetStudentClass_Schedules(startDate.ToString("yyyy-MM-dd"), startDate.AddDays(6).ToString("yyyy-MM-dd"));//  new List<Class_Schedule>();
			cleanClass_Schedule = new List<Class_Schedule>();

			CompleteClass_Schedules();

			return 1;
		}


		public void CompleteClass_Schedules()
		{
			foreach (Class_Schedule class_schedule in weekClass_Schedule)
			{
				if (class_schedule.classattendancestatus == "confirmada")
				{
					class_schedule.participationimage = "iconcheck.png";
				}
				DateTime class_schedule_date = DateTime.Parse(class_schedule.date).Date;

				class_schedule.datestring = Constants.daysofWeekPT[class_schedule_date.DayOfWeek.ToString()] + " - "
					+ class_schedule_date.Day + " "
					+ Constants.months[class_schedule_date.Month] + "\n"
					+ class_schedule.begintime + " às " + class_schedule.endtime;
				if (class_schedule.imagesource == null)
				{
					class_schedule.imagesourceObject = "logo_aksl.png";
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


				if ((class_schedule.classattendancestatus == "confirmada") | (class_schedule.classattendancestatus == "fechada"))
				{
					class_schedule.participationimage = "iconcheck.png";
				}
				else
				{
					class_schedule.participationimage = "iconinativo.png";
				}

			}

		}



		public void CreateClassesColletion()
		{
			//COLLECTION GRADUACOES
			weekClassesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Multiple,
				ItemsSource = weekClass_Schedule,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem Aulas agendadas nesta semana.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White, FontSize = 20 },
							}
					}
				}
			};

			weekClassesCollectionView.SelectionChanged += OnClassScheduleCollectionViewSelectionChanged;

			GradientBrush gradient = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 1),
			};

			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(180, 143, 86), Convert.ToSingle(0)));
			gradient.GradientStops.Add(new GradientStop(Color.FromRgb(246, 220, 178), Convert.ToSingle(0.5)));


			weekClassesCollectionView.ItemTemplate = new DataTemplate(() =>
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
					yConstraint: Constraint.Constant(3 * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant((20 * App.screenHeightAdapter)),
					heightConstraint: Constraint.Constant((20 * App.screenHeightAdapter)));

				return itemRelativeLayout;
			});
			relativeLayout.Children.Add(weekClassesCollectionView,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(80 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (100 * App.screenHeightAdapter); // 
				}));


		}


		public AttendancePageCS()
		{
			this.initLayout();
		}


		public AttendancePageCS(string week)
		{
			this.week = week;
			this.initLayout();
		}



		async void OnWeek0ButtonClicked(object sender, EventArgs e)
		{
			week0Button.activate();
			week1Button.deactivate();
			week2Button.deactivate();
			week3Button.deactivate();

			DateTime currentTime = DateTime.Now.Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);
			int result = await getClass_DetailData(firstDayWeek);

			weekClassesCollectionView.ItemsSource = cleanClass_Schedule;
			weekClassesCollectionView.ItemsSource = weekClass_Schedule;
			//_collection.Items = week0Class_Detail;
		}

		async void OnWeek1ButtonClicked(object sender, EventArgs e)
		{
			week0Button.deactivate();
			week1Button.activate();
			week2Button.deactivate();
			week3Button.deactivate();

			DateTime currentTime = DateTime.Now.AddDays(7).Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);
			int result = await getClass_DetailData(firstDayWeek);

			weekClassesCollectionView.ItemsSource = cleanClass_Schedule;
			weekClassesCollectionView.ItemsSource = weekClass_Schedule;
		}

		async void OnWeek2ButtonClicked(object sender, EventArgs e)
		{
			week0Button.deactivate();
			week1Button.deactivate();
			week2Button.activate();
			week3Button.deactivate();

			DateTime currentTime = DateTime.Now.AddDays(14).Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);
			int result = await getClass_DetailData(firstDayWeek);

			weekClassesCollectionView.ItemsSource = cleanClass_Schedule;
			weekClassesCollectionView.ItemsSource = weekClass_Schedule;
		}

		async void OnWeek3ButtonClicked(object sender, EventArgs e)
		{
			week0Button.deactivate();
			week1Button.deactivate();
			week2Button.deactivate();
			week3Button.activate();

			DateTime currentTime = DateTime.Now.AddDays(21).Date;
			DateTime firstDayWeek = currentTime.AddDays(-Constants.daysofWeekInt[currentTime.DayOfWeek.ToString()]);


			int result = await getClass_DetailData(firstDayWeek);

			weekClassesCollectionView.ItemsSource = cleanClass_Schedule;
			weekClassesCollectionView.ItemsSource = weekClass_Schedule;
		}

		async Task<List<Class_Schedule>> GetStudentClass_Schedules(string begindate, string enddate)
		{
			Debug.WriteLine("GetStudentClass_Schedules");
			showActivityIndicator();
			ClassManager classManager = new ClassManager();
			List<Class_Schedule> class_schedules_i = await classManager.GetStudentClass_Schedules(App.member.id, begindate, enddate);
            hideActivityIndicator();
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


		async void OnClassScheduleCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			showActivityIndicator();
			Debug.WriteLine("MainPageCS.OnClassScheduleCollectionViewSelectionChanged");

			if ((sender as CollectionView).SelectedItems.Count != 0)
			{
				ClassManager classmanager = new ClassManager();

				Class_Schedule class_schedule = (sender as CollectionView).SelectedItems[0] as Class_Schedule;
				if (class_schedule.classattendanceid == null)
				{
					string class_attendance_id = await classmanager.CreateClass_Attendance(App.member.id, class_schedule.classid, "confirmada", class_schedule.date);
					class_schedule.classattendanceid = class_attendance_id;
					class_schedule.classattendancestatus = "confirmada";
					class_schedule.participationimage = "iconcheck.png";
				}
				else
				{
					if (class_schedule.classattendancestatus == "anulada")
					{
						class_schedule.classattendancestatus = "confirmada";
						class_schedule.participationimage = "iconcheck.png";
						int result = await classmanager.UpdateClass_Attendance(class_schedule.classattendanceid, class_schedule.classattendancestatus);
					}
					else if (class_schedule.classattendancestatus == "confirmada")
					{
						class_schedule.classattendancestatus = "anulada";
						class_schedule.participationimage = "iconinativo.png";
						int result = await classmanager.UpdateClass_Attendance(class_schedule.classattendanceid, class_schedule.classattendancestatus);
					}
					else if (class_schedule.classattendancestatus == "fechada")
					{
                        await DisplayAlert("PRESENÇA EM AULA", "A tua presença nesta aula já foi validada pelo treinador pelo que não é possível alterar o seu estado.", "OK");
					}
					
				}

				((CollectionView)sender).SelectedItems.Clear();
				weekClassesCollectionView.ItemsSource = cleanClass_Schedule;
				weekClassesCollectionView.ItemsSource = weekClass_Schedule;

                hideActivityIndicator();
            }
		}



	}
}
