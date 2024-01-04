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

using Xamarin.Essentials;

namespace SportNow.Views
{
	public class AttendanceMonthPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private RelativeLayout presencasRelativeLayout;



		private CollectionView class_attendanceCollectionView;

		private Class_Schedule class_schedule;
		private ObservableCollection<Class_Attendance> class_attendances;
		private List<Class_Attendance> class_attendances_dummy = new List<Class_Attendance>();

		private AttendanceCollection attendanceCollection;

		private int alunosAusentes, alunosMarcados, alunosConfirmados;
		Label ausentesLabel, marcadosLabel, confirmadosLabel;

		public void initLayout()
		{
			Title = "PRESENÇA AULA";

			relativeLayout = new RelativeLayout
			{	
				Margin = new Thickness(20)
			};
			Content = relativeLayout;

		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
		}

		public async void initSpecificLayout()
		{
			showActivityIndicator();

			class_attendances = await GetClass_Attendances();
			if (class_attendances == null)
			{
				Debug.Print("class_attendances_i é null");
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return;
			}
			CreateTitle();
			CompleteClass_Attendances();

			attendanceCollection = new AttendanceCollection();
			attendanceCollection.Items = class_attendances;

			CreateClassesColletion();
			CreateConfirmButton();

            hideActivityIndicator();
        }


		public void CreateTitle()
		{

			Label className = new Label()
			{
				Text = this.class_schedule.name + "\n" + class_schedule.dojo + "\n" + class_schedule.date,
				FontSize = 20,
				TextColor = Color.FromRgb(246, 220, 178),
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center
			};

			relativeLayout.Children.Add(className,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(80));
		}

		public void CompleteClass_Attendances()
		{
			foreach (Class_Attendance class_attendance in class_attendances)
			{
				Debug.Print("class_attendance.membernickname=" + class_attendance.membernickname + " " + class_attendance.status);
				if (class_attendance.status == "confirmada")
				{
					alunosMarcados++;
					class_attendance.imagesource = "iconinativo.png";
					class_attendance.color = Color.Blue;
				}
				else if (class_attendance.status == "fechada")
				{
					alunosConfirmados++;
					class_attendance.imagesource = "iconcheck.png";
					class_attendance.color = Color.Green;
				}
				else if (class_attendance.status == "anulada")
				{
					alunosAusentes++;
					class_attendance.imagesource = "";
					class_attendance.color = Color.Yellow;
				}
				else 
				{
					alunosAusentes++;
					class_attendance.color = Color.Transparent;
				}
			}
		}

		public void CreateClassesColletion()
		{
			//COLLECTION GRADUACOES
			class_attendanceCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				//ItemsSource = class_attendances,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem Alunos inscritos nesta aula.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White, FontSize = 20 },
							}
					}
				}
			};

			//class_attendanceCollectionView.BindingContext = attendanceCollection;
			this.BindingContext = attendanceCollection;
			class_attendanceCollectionView.SetBinding(ItemsView.ItemsSourceProperty, "Items");

			class_attendanceCollectionView.SelectionChanged += OnClassAttendanceCollectionViewSelectionChanged;

			class_attendanceCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					HeightRequest = 30
				};

				FormValue nameLabel = new FormValue("");
				nameLabel.label.SetBinding(Label.TextProperty, "membernickname");


				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width-50);
					}),
					heightConstraint: Constraint.Constant(30));

				Frame attendanceStatus_Frame = new Frame()
				{
					CornerRadius = 5,
					IsClippedToBounds = true,
					BorderColor = Color.FromRgb(246, 220, 178),
					Padding = new Thickness(2, 2, 2, 2),
					HeightRequest = 30,
					VerticalOptions = LayoutOptions.Center,
				};

				attendanceStatus_Frame.SetBinding(Frame.BackgroundColorProperty, "color");


				itemRelativeLayout.Children.Add(attendanceStatus_Frame,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - 40);
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(30),
					heightConstraint: Constraint.Constant(30));

				return itemRelativeLayout;
			});
			relativeLayout.Children.Add(class_attendanceCollectionView,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(80),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 180; // 
				}));

			Grid gridCount = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridCount.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			gridCount.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
			gridCount.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
			gridCount.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

			ausentesLabel = new Label
			{
				Text = "Ausentes - " + alunosAusentes,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				FontSize = 18
			};

			marcadosLabel = new Label
			{
				Text = "Marcados - " + alunosMarcados,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				FontSize = 18
			};

			confirmadosLabel = new Label
			{
				Text = "Confirmados - " + alunosConfirmados,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				FontSize = 18
			};
			gridCount.Children.Add(ausentesLabel, 0, 0);
			gridCount.Children.Add(marcadosLabel, 1, 0);
			gridCount.Children.Add(confirmadosLabel, 2, 0);

			relativeLayout.Children.Add(gridCount,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 100;
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
							}),
				heightConstraint:  Constraint.Constant(50));

		}


		public void CreateConfirmButton()
		{

			RoundButton confirmButton = new RoundButton("CONFIRMAR PRESENÇAS", 100, 40);
			confirmButton.button.Clicked += OnConfirmButtonClicked;

			relativeLayout.Children.Add(confirmButton,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 40; // 
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(40));
		}

		public AttendanceMonthPageCS(Class_Schedule class_schedule)
		{
			this.class_schedule = class_schedule;
			this.initLayout();
		}

		async Task<ObservableCollection<Class_Attendance>> GetClass_Attendances()
		{
			ClassManager classManager = new ClassManager();
			ObservableCollection<Class_Attendance> class_attendances_i = await classManager.GetClass_Attendances_obs(this.class_schedule.classid, this.class_schedule.date);
			if (class_attendances_i == null)
			{
				Debug.Print("class_attendances_i é null");
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			Debug.Print("class_attendances_i não é null");
			return class_attendances_i;
		}

		async void OnConfirmButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();

			Debug.WriteLine("OnConfirmButtonClicked");
			ClassManager classmanager = new ClassManager();
			foreach (Class_Attendance class_attendance in class_attendances)
			{
				Debug.WriteLine("OnConfirmButtonClicked class_attendance.classattendanceid=" + class_attendance.classattendanceid + " class_attendance.status="+ class_attendance.status);
				if ((class_attendance.classattendanceid == null) & (class_attendance.status == "confirmada"))
				{ // SE NÃO EXISTIA E O INSTRUTOR METEU CONFIRMADA
					Debug.Print("CRIA");
					string class_attendance_id = await classmanager.CreateClass_Attendance(class_attendance.memberid, class_attendance.classid, "fechada", class_attendance.date);
					class_attendance.classattendanceid = class_attendance_id;
					class_attendance.status = "fechada";
					class_attendance.color = Color.Green;

					alunosMarcados--;
					marcadosLabel.Text = "Marcados - " + alunosMarcados;
					alunosConfirmados++;
					confirmadosLabel.Text = "Confirmados - " + alunosConfirmados;
				}
				else if ((class_attendance.classattendanceid != null) &
					((class_attendance.statuschanged == true) | ((class_attendance.statuschanged == false) & (class_attendance.status == "confirmada"))))
				{
					Debug.Print("FAZ UPDATE");
					if (class_attendance.status == "confirmada")
					{
						class_attendance.status = "fechada";
						class_attendance.color = Color.Green;

						alunosMarcados--;
						marcadosLabel.Text = "Marcados - " + alunosMarcados;
						alunosConfirmados++;
						confirmadosLabel.Text = "Confirmados - " + alunosConfirmados;

					}
					/*else if (class_attendance.status == "anulada")
					{
						alunosMarcados--;
						marcadosLabel.Text = "Marcados - " + alunosMarcados;
						alunosConfirmados++;
						confirmadosLabel.Text = "Confirmados - " + alunosConfirmados;
					}*/
					_ = await classmanager.UpdateClass_Attendance(class_attendance.classattendanceid, class_attendance.status);
					//ATUALIZA ESTADO
				}
			}

            hideActivityIndicator();

        }


		

		async void OnClassAttendanceCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged ");


			if ((sender as CollectionView).SelectedItem != null)
			{
				Class_Attendance class_attendance = (sender as CollectionView).SelectedItem as Class_Attendance;

				Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged selected item = "+ class_attendance.classattendanceid);
				class_attendance.statuschanged = true;

				if ((class_attendance.status == "confirmada") | (class_attendance.status == "fechada"))
				{
					if (class_attendance.status == "confirmada")
					{
						alunosMarcados--;
						marcadosLabel.Text = "Marcados - " + alunosMarcados;
					}
					else if (class_attendance.status == "fechada")
					{
						alunosConfirmados--;
						confirmadosLabel.Text = "Confirmados - " + alunosConfirmados;

					}
					alunosAusentes++;
					ausentesLabel.Text = "Ausentes - " + alunosAusentes;

					if (class_attendance.classattendanceid != null)
					{
						Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged status = anulada");
						class_attendance.status = "anulada";
						class_attendance.color = Color.Yellow;
					}
					else
					{
						Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged status = vazio");
						class_attendance.status = "";
						class_attendance.color = Color.Transparent;
					}


				}
				else
				{
					Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged status = confirmada");
					alunosAusentes--;
					ausentesLabel.Text = "Ausentes - " + alunosAusentes;
					alunosMarcados++;
					marcadosLabel.Text = "Marcados - " + alunosMarcados;

					class_attendance.status = "confirmada";
					class_attendance.color = Color.Blue;
					
				}

				class_attendanceCollectionView.SelectedItem = null;
			}
			else
            {
				Debug.WriteLine("OnClassAttendanceCollectionViewSelectionChanged selected item = nulll");
			}
		}
	}
}
