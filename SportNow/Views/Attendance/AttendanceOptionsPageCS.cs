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
	public class AttendanceOptionsPageCS : DefaultPage
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


		private StackLayout stackButtons;

		private OptionButton marcarAulaButton, estatisticasButton, presencasButton, mensalidadesButton, mensalidadesStudentButton;
	

		public void initLayout()
		{
			Title = "AULAS";

			relativeLayout = new RelativeLayout
			{
				Margin = new Thickness(0)
			};
			Content = relativeLayout;

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
			if (presencasRelativeLayout != null)
			{
				relativeLayout.Children.Remove(presencasRelativeLayout);

				presencasRelativeLayout = null;
			}

		}

		public async void initSpecificLayout()
		{
			presencasRelativeLayout = new RelativeLayout
			{
				Margin = new Thickness(5)
			};

			CreatePresencasOptionButtonsAsync();

			relativeLayout.Children.Add(presencasRelativeLayout,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(20),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 20;
				}));
		}


		public async Task CreatePresencasOptionButtonsAsync()
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width) / 2;


			marcarAulaButton = new OptionButton("MARCAR AULAS", "confirmclasses.png", buttonWidth, 70);
			//minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
			var marcarAulaButton_tap = new TapGestureRecognizer();
			marcarAulaButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new AttendancePageCS());
			};
			marcarAulaButton.GestureRecognizers.Add(marcarAulaButton_tap);

			estatisticasButton = new OptionButton("ESTATÍSTICAS", "estatisticas.png", buttonWidth, 70);
			var estatisticasButton_tap = new TapGestureRecognizer();
			estatisticasButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new AttendanceStatsPageCS());
			};
			estatisticasButton.GestureRecognizers.Add(estatisticasButton_tap);

			presencasButton = new OptionButton("PRESENÇAS", "presencasadcpn.png", buttonWidth, 70);
			//minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
			var presencasButton_tap = new TapGestureRecognizer();
			presencasButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new AttendanceManagePageCS());
			};
			presencasButton.GestureRecognizers.Add(presencasButton_tap);

			mensalidadesButton = new OptionButton("MENSALIDADES TREINADOR", "mensalidades_alunos.png", buttonWidth, 70);
			var mensalidadesButton_tap = new TapGestureRecognizer();
			mensalidadesButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new MonthFeeListPageCS());
			};
			mensalidadesButton.GestureRecognizers.Add(mensalidadesButton_tap);

			mensalidadesStudentButton = new OptionButton("MENSALIDADES", "monthfees.png", buttonWidth, 70);
			var mensalidadesStudentButton_tap = new TapGestureRecognizer();
			mensalidadesStudentButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new MonthFeeStudentListPageCS());
			};
			mensalidadesStudentButton.GestureRecognizers.Add(mensalidadesStudentButton_tap);


			string monthFeeStudentCount = await Get_has_StudentMonthFees();


			StackLayout stackPresencasButtons;
			if (App.member.students_count > 0)
			{
				if (monthFeeStudentCount != "0")
				{

					stackPresencasButtons = new StackLayout
					{
						//WidthRequest = 370,
						Margin = new Thickness(0),
						Spacing = 10 * App.screenHeightAdapter,
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						HeightRequest = 550 * App.screenHeightAdapter,
						Children =
							{
								presencasButton,
								marcarAulaButton,
								estatisticasButton,
								mensalidadesButton,
								mensalidadesStudentButton,
							}
					};
				}
				else
				{

					stackPresencasButtons = new StackLayout
					{
						//WidthRequest = 370,
						Margin = new Thickness(0),
						Spacing = 20 * App.screenHeightAdapter,
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						HeightRequest = 550 * App.screenHeightAdapter,
						Children =
							{
								presencasButton,
								marcarAulaButton,
								estatisticasButton,
								mensalidadesButton
							}
					};
				}

			}
			else
			{

				if (monthFeeStudentCount != "0")
				{
					stackPresencasButtons = new StackLayout
					{
						//WidthRequest = 370,
						Margin = new Thickness(0),
						Spacing = 50 * App.screenHeightAdapter,
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						HeightRequest = 400 * App.screenHeightAdapter,
						Children =
						{
							marcarAulaButton,
							estatisticasButton,
							mensalidadesStudentButton
						}
					};
				}
				else
				{
					stackPresencasButtons = new StackLayout
					{
						//WidthRequest = 370,
						Margin = new Thickness(0),
						Spacing = 50 * App.screenHeightAdapter,
						Orientation = StackOrientation.Vertical,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.FillAndExpand,
						HeightRequest = 400 * App.screenHeightAdapter,
						Children =
						{
							marcarAulaButton,
							estatisticasButton,
						}
					};
				}
			}


			

			presencasRelativeLayout.Children.Add(stackPresencasButtons,
			xConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width / 4);
			}),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width/2);
			}),
			heightConstraint: Constraint.Constant(550 * App.screenHeightAdapter));
		}

		public AttendanceOptionsPageCS()
		{
			this.initLayout();
		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfilePageCS());
		}

		async Task<string> Get_has_StudentMonthFees()
		{
			Debug.WriteLine("GetStudentClass_Schedules");
			MonthFeeManager monthFeeManager = new MonthFeeManager();
			string count = await monthFeeManager.Has_MonthFeesStudent(App.member.id);
			if (count == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return count;
		}

	}
}
