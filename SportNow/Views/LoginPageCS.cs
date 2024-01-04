using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Collections.Generic;
//using Android.Preferences;
//using Android.Content;
using SportNow.CustomViews;
//Ausing Acr.UserDialogs;
using Xamarin.Essentials;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using SportNow.Views.Profile;

namespace SportNow.Views
{
	public class LoginPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);

			var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
			Constants.ScreenWidth = mainDisplayInfo.Width;
			Constants.ScreenHeight = mainDisplayInfo.Height;
			Debug.Print("AQUI Login - ScreenWidth = " + Constants.ScreenWidth + " ScreenHeight = " + Constants.ScreenHeight + "mainDisplayInfo.Density = " + mainDisplayInfo.Density);

			App.AdaptScreen();
			this.initSpecificLayout();
		}


		Label welcomeLabel;
		Button loginButton;
		FormEntry usernameEntry;
		FormEntryPassword passwordEntry;
		Label messageLabel;

		string password = "";
		string username = "";
		string message = "";
	

		public void initBaseLayout() {
			this.BackgroundColor = Color.White;

			NavigationPage.SetHasNavigationBar(this, false);
		}

		public void initSpecificLayout()
		{
			welcomeLabel = new Label
			{
				Text = "BEM-VINDO",
				TextColor = App.topColor,
				FontSize = 30 * App.screenHeightAdapter,
				HorizontalOptions = LayoutOptions.Center
			};
			relativeLayout.Children.Add(welcomeLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(50 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
			);


			Image logo_adcpn = new Image
			{
				Source = "logo_login.png",
				HorizontalOptions = LayoutOptions.Center,
			};
			relativeLayout.Children.Add(logo_adcpn,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width / 2) - (70 * App.screenHeightAdapter));
				}),
				yConstraint: Constraint.Constant(90 * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(140 * App.screenHeightAdapter)
			);


			if (App.Current.Properties.ContainsKey("EMAIL"))
			{
				username = App.Current.Properties["EMAIL"] as string;
			}


			//USERNAME ENTRY
			usernameEntry = new FormEntry(username, "EMAIL", 300, Keyboard.Email);
			usernameEntry.entry.IsTextPredictionEnabled = true;

			relativeLayout.Children.Add(usernameEntry,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(300 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(45 * App.screenHeightAdapter)
			);

			if (App.Current.Properties.ContainsKey("PASSWORD"))
			{
				password = App.Current.Properties["PASSWORD"] as string;
			}

			//PASSWORD ENTRY
			passwordEntry = new FormEntryPassword(password, "PALAVRA-PASSE");
			relativeLayout.Children.Add(passwordEntry,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(350 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(45 * App.screenHeightAdapter)
			);

			//LOGIN BUTTON
			GradientBrush gradient = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 1),
			};

			gradient.GradientStops.Add(new GradientStop(App.topColor, Convert.ToSingle(0)));
			gradient.GradientStops.Add(new GradientStop(App.bottomColor, Convert.ToSingle(0.5)));

			loginButton = new Button
			{
				Text = "LOGIN",
				Background = gradient,
                TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = 200 * App.screenWidthAdapter,
				HeightRequest = 45 * App.screenHeightAdapter
			};
			loginButton.Clicked += OnLoginButtonClicked;

			

			Frame frame_loginButton = new Frame {
				BackgroundColor = Color.FromRgb(25, 25, 25),
				BorderColor = Color.LightGray,
				CornerRadius = 10 * (float) App.screenWidthAdapter,
				IsClippedToBounds = true,
				Padding = 0,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				WidthRequest = 200 * App.screenWidthAdapter,
				HeightRequest = 45 * App.screenWidthAdapter
			};
			
			frame_loginButton.Content = loginButton;

			relativeLayout.Children.Add(loginButton,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(410 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(45 * App.screenHeightAdapter)
			);


			messageLabel = new Label {
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.End,
				TextColor = Color.Red,
				FontSize = App.itemTitleFontSize
			};

			messageLabel.Text = this.message;

			relativeLayout.Children.Add(messageLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(250 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
			);

			//RECOVER PASSWORD LABEL
			Label recoverPasswordLabel = new Label
			{
				Text = "Recuperar palavra-passe",
				TextColor = Color.Black,
				FontSize = 20,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center
			};
			var recoverPasswordLabel_tap = new TapGestureRecognizer();
			recoverPasswordLabel_tap.Tapped += (s, e) =>
			{
				/*Navigation.InsertPageBefore(new RecoverPasswordPageCS(), this);
				Navigation.PopAsync();*/

				 Navigation.PushAsync(new RecoverPasswordPageCS());

			};
			recoverPasswordLabel.GestureRecognizers.Add(recoverPasswordLabel_tap);

			relativeLayout.Children.Add(recoverPasswordLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(450 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter)
			);


            //RECOVER PASSWORD LABEL
            Label newMemberLabel = new Label
            {
                Text = "Novo Sócio",
                TextColor = Color.Black,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };
            var newMemberLabel_tap = new TapGestureRecognizer();
            newMemberLabel_tap.Tapped += (s, e) =>
            {
                /*Navigation.InsertPageBefore(new RecoverPasswordPageCS(), this);
				Navigation.PopAsync();*/

                Navigation.PushAsync(new NewMemberPageCS());

            };
            newMemberLabel.GestureRecognizers.Add(newMemberLabel_tap);

            relativeLayout.Children.Add(newMemberLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(540 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter)
			);

        }


		public LoginPageCS (string message)
		{
			if (message != "")
			{
				this.message = message;
				//UserDialogs.Instance.Alert(new AlertConfig() { Title = "Erro", Message = message, OkText = "Ok" });
			}
			
			this.initBaseLayout();
			

		}

		async void OnSignUpButtonClicked (object sender, EventArgs e)
		{
			//await Navigation.PushAsync (new SignUpPageCS ());
		}

		async void OnLoginButtonClicked (object sender, EventArgs e)
		{
			Debug.WriteLine("OnLoginButtonClicked");

			loginButton.IsEnabled = false;

			var user = new User {
				Username = usernameEntry.entry.Text,
				Password = passwordEntry.entry.Text
			};


			MemberManager memberManager = new MemberManager();

            ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true };
			showActivityIndicator();

            var loginResult = await memberManager.Login(user);

			if (loginResult == "1")
			{
				Debug.WriteLine("login ok");

				App.members = await GetMembers(user);

				this.saveUserPassword(user.Username, user.Password);

				if (App.members.Count == 1)
                {
					App.original_member = App.members[0];
					App.member = App.original_member;

					App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
					{
						BarBackgroundColor = Color.White,
						BackgroundColor = Color.White,
						BarTextColor = Color.Black
					};
				}
				else if (App.members.Count > 1)
				{
					Navigation.InsertPageBefore(new SelectMemberPageCS(), this);
					await Navigation.PopAsync();
				}
			}
			else {
				Debug.WriteLine("login nok");
				passwordEntry.entry.Text = string.Empty;

				if (loginResult == "0")
				{
					Debug.WriteLine("Login falhou. O Utilizador não existe");
					messageLabel.Text = "Login falhou. O Utilizador não existe.";
				}
				else if (loginResult == "-1")
				{
					Debug.WriteLine("Login falhou. A Palavra-Passe está incorreta");
					messageLabel.Text = "Login falhou. A Palavra-Passe está incorreta.";
				}
				else if (loginResult == "-2")
				{
					Debug.WriteLine("Ocorreu um erro 1. Volte a tentar mais tarde.");
					messageLabel.Text = "Ocorreu um erro 1. Volte a tentar mais tarde.";
				}
				else
				{
					Debug.WriteLine("Ocorreu um erro. Volte a tentar mais tarde ." + loginResult);
					messageLabel.Text = "Ocorreu um erro. Volte a tentar mais tarde.";// + loginResult;
                    await DisplayAlert("ERRO", loginResult, "OK");
//                    UserDialogs.Instance.Alert(new AlertConfig() { Title = "Erro", Message = loginResult, OkText = "Ok" });
				}

				this.saveUserPassword(user.Username, user.Password);
			}
			loginButton.IsEnabled = true;
            hideActivityIndicator();
        }


		async Task<List<Member>> GetMembers(User user)
		{
			Debug.WriteLine("GetMembers");
			MemberManager memberManager = new MemberManager();

			List<Member> members;

			members = await memberManager.GetMembers(user);

			return members;
			
		}

		protected void saveUserPassword(string email, string password)
		{
			Application.Current.Properties.Remove("EMAIL");
			Application.Current.Properties.Remove("PASSWORD");

			Application.Current.Properties.Add("EMAIL", email);
			Application.Current.Properties.Add("PASSWORD", password);
			Application.Current.SavePropertiesAsync();

			username = App.Current.Properties["EMAIL"] as string;
		}
	}
}


