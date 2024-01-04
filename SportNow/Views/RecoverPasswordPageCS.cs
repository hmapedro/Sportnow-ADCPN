
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Collections.Generic;
using System.Net.Mail;
using SportNow.CustomViews;

namespace SportNow.Views
{
	public class RecoverPasswordPageCS : DefaultPage
	{

		Button recoverPasswordButton;
		FormEntry usernameEntry;
		Label messageLabel;



		public void initBaseLayout() {
			Title = "RECUPERAR PALAVRA-PASSE";			




			/*var toolbarItem = new ToolbarItem {
				Text = "Sign Up",
			};
			toolbarItem.Clicked += OnSignUpButtonClicked;
			ToolbarItems.Add (toolbarItem);

			messageLabel = new Label ();*/

		}

		public void initSpecificLayout()
		{

			Grid gridLogin = new Grid { Padding = 10 };
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridLogin.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridLogin.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star}); //GridLength.Auto 


			Label recoverPasswordLabel = new Label
			{
				Text = "Introduza o email para o qual pretende recuperar a Palavra-Passe.",
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				FontSize = App.itemTitleFontSize
			};

			string username = "";

			if (App.Current.Properties.ContainsKey("EMAIL"))
			{
				username = App.Current.Properties["EMAIL"] as string;
			}

			//USERNAME ENTRY
			usernameEntry = new FormEntry(username, "EMAIL", 300, Keyboard.Email);

			

			//LOGIN BUTTON
			GradientBrush gradient = new LinearGradientBrush
			{
				StartPoint = new Point(0, 0),
				EndPoint = new Point(1, 1),
			};

			gradient.GradientStops.Add(new GradientStop(App.topColor, Convert.ToSingle(0)));
			gradient.GradientStops.Add(new GradientStop(App.bottomColor, Convert.ToSingle(0.5)));

			recoverPasswordButton = new Button
			{
				Text = "ENVIAR EMAIL",
				Background = gradient,
                TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				FontSize = App.itemTitleFontSize,
				WidthRequest = 200 * App.screenWidthAdapter
			};
			recoverPasswordButton.Clicked += OnrecoverPasswordButtonClicked;


			Frame frame_recoverPasswordButton = new Frame {
				BackgroundColor = Color.FromRgb(25, 25, 25),
				BorderColor = Color.LightGray,
				CornerRadius = 10 * (float) App.screenHeightAdapter,
				IsClippedToBounds = true,
				Padding = 0,
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = 200 * App.screenWidthAdapter,
				HasShadow = false
			};

			frame_recoverPasswordButton.Content = recoverPasswordButton;

			messageLabel = new Label {
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.Green,
				FontSize = App.itemTitleFontSize
			};

			//RECOVER PASSWORD LABEL

			gridLogin.Children.Add(recoverPasswordLabel, 0, 0);
			gridLogin.Children.Add(usernameEntry, 0, 1);
			gridLogin.Children.Add(frame_recoverPasswordButton, 0, 2);
			gridLogin.Children.Add(messageLabel, 0, 3);

			relativeLayout.Children.Add(gridLogin,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) ; // center of image (which is 40 wide)
				})
			);

		}


		public RecoverPasswordPageCS()
		{

			this.initBaseLayout();
			this.initSpecificLayout();

		}


		async void OnrecoverPasswordButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnrecoverPasswordButtonClicked");

			recoverPasswordButton.IsEnabled = false;
			messageLabel.Text = "";
			messageLabel.TextColor = Color.Green;

			MemberManager memberManager = new MemberManager();


			if (IsValidEmail(usernameEntry.entry.Text))
			{
				int result = await memberManager.RecoverPassword(usernameEntry.entry.Text);

				if (result == 1)
				{
					messageLabel.TextColor = Color.Green;
					messageLabel.Text = "Enviámos um email para o endereço indicado em cima com os dados para recuperar a sua password.";
				}
				else if (result == -1)
				{
					messageLabel.TextColor = Color.Red;
					messageLabel.Text = "Houve um erro. O email introduzido não é válido.";
				}
				else 
				{
					messageLabel.TextColor = Color.Red;
					messageLabel.Text = "Houve um erro. Verifique a sua ligação à Internet ou tente novamente mais tarde.";
				}
			}
			else
			{
				messageLabel.TextColor = Color.Red;
				messageLabel.Text = "Houve um erro. O email introduzido não é válido.";
			}
			recoverPasswordButton.IsEnabled = true;
		}

		async Task <string> AreCredentialsCorrect (User user)
		{
			Debug.WriteLine("AreCredentialsCorrect");
			MemberManager memberManager = new MemberManager();

			string loginOk = await memberManager.Login(user);

			return loginOk;
		}

		async Task<List<Member>> GetMembers(User user)
		{
			Debug.WriteLine("AreCredentialsCorrect");
			UserManager userManager = new UserManager();

			List<Member> members;

			members = await userManager.GetMembers(user);

			return members;
			
		}

		public bool IsValidEmail(string emailaddress)
		{
			try
			{
				MailAddress m = new MailAddress(emailaddress);

				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

	}
}


