using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using Xamarin.Essentials;

namespace SportNow.Views.Profile
{
    public class ChangePasswordPageCS : DefaultPage
	{



		private Member member;


		private Grid grid;
		FormEntryPassword currentPasswordEntry;
		FormEntryPassword newPasswordEntry;
		FormEntryPassword newPasswordConfirmEntry;
		RoundButton changePasswordButton;
		Label messageLabel;

		CancelButton deleteMemberButton;

        public void initLayout()
		{
			Title = "SEGURANÇA";



			

			/*var toolbarItem = new ToolbarItem
			{
				Text = "Logout"
			};
			toolbarItem.Clicked += OnLogoutButtonClicked;
			ToolbarItems.Add(toolbarItem);*/

		}


		public void initSpecificLayout()
		{

			//member = App.members[0];


			grid = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //gridGeral.RowDefinitions.Add(new RowDefinition { Height = 1 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			FormLabel currentPasswordLabel = new FormLabel { Text = "PALAVRA-PASSE ATUAL" };
			currentPasswordEntry = new FormEntryPassword("", "");

			FormLabel newPasswordLabel = new FormLabel { Text = "NOVA PALAVRA-PASSE" };
			newPasswordEntry = new FormEntryPassword("", "");

			FormLabel newPasswordConfirmLabel = new FormLabel { Text = "NOVA PALAVRA-PASSE CONFIRMAÇÃO" };
			newPasswordConfirmEntry = new FormEntryPassword("", "");

			changePasswordButton = new RoundButton("Alterar Palavra-Passe", 100, 50);
			changePasswordButton.button.Clicked += OnChangePasswordButtonClicked;

			messageLabel = new Label
			{
				HorizontalOptions = LayoutOptions.Center,
				TextColor = Color.Red,
				FontSize = App.itemTitleFontSize
			};

			grid.Children.Add(currentPasswordLabel, 0, 0);
			grid.Children.Add(currentPasswordEntry, 1, 0);

			grid.Children.Add(newPasswordLabel, 0, 1);
			grid.Children.Add(newPasswordEntry, 1, 1);

			grid.Children.Add(newPasswordConfirmLabel, 0, 2);
			grid.Children.Add(newPasswordConfirmEntry, 1, 2);

			grid.Children.Add(changePasswordButton, 0, 3);
			Grid.SetColumnSpan(changePasswordButton, 2);

			grid.Children.Add(messageLabel, 0, 4);
			Grid.SetColumnSpan(messageLabel, 2);

            deleteMemberButton = new CancelButton("Apagar Sócio", 100, 50);
            deleteMemberButton.VerticalOptions = LayoutOptions.End;
            deleteMemberButton.button.Clicked += OndeleteMemberButtonClicked;

            grid.Children.Add(deleteMemberButton, 0, 6);
            Grid.SetColumnSpan(deleteMemberButton, 2);


            relativeLayout.Children.Add(grid,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(20),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 10); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 20; // center of image (which is 40 wide)
				})
			);


		}

		public ChangePasswordPageCS(Member member)
		{
			this.member = member;
			this.initLayout();
			this.initSpecificLayout();

		}

		async void OnChangePasswordButtonClicked(object sender, EventArgs e)
		{
			changePasswordButton.IsEnabled = false;
			messageLabel.TextColor = Color.Red;
			if (newPasswordEntry.entry.Text.Length < 6) {
				messageLabel.Text = "A nova Palavra-Passe tem de ter pelo menos 6 caracteres.";
			}
			else if (newPasswordEntry.entry.Text != newPasswordConfirmEntry.entry.Text)
			{
				messageLabel.Text = "A nova Palavra-Passe não coincide com a Palavra-Passe de confirmação.";
			}
			else 
			{
				MemberManager memberManager = new MemberManager();
				var user = new User
				{
					Username = member.email,
					Password = currentPasswordEntry.entry.Text
				};

				var loginResult = await memberManager.Login(user);

				if (loginResult == "1")
				{
					Debug.WriteLine("password ok");
					int changePasswordResult = await ChangePassword(member.email, newPasswordEntry.entry.Text);
					if (changePasswordResult == 1)
					{
						messageLabel.TextColor = Color.Green;
						messageLabel.Text = "A Palavra-Passe foi alterada com sucesso.";
						
					}
					else
                    {
						messageLabel.Text = "A alteração de Palavra-Passe falhou. Tente novamente mais tarde ou contacte o Apoio ao Cliente.";
					}
				}
				else
				{
					Debug.WriteLine("password nok");
					changePasswordButton.IsEnabled = true;
					currentPasswordEntry.entry.Text = string.Empty;
					newPasswordEntry.entry.Text = string.Empty;
					newPasswordConfirmEntry.entry.Text = string.Empty;

					if (loginResult == "0")
					{
						messageLabel.Text = "A alteração de Palavra-Passe falhou. O Utilizador não existe."; //isto não pode acontecer...
					}
					else if (loginResult == "-1")
					{
						messageLabel.Text = "A alteração de Palavra-Passe falhou. A Password atual está incorreta.";
					}
					else
					{
						messageLabel.Text = "Ocorreu um erro. Volte a tentar mais tarde.";
					}
				}
			}
			changePasswordButton.IsEnabled = true;

			/*Application.Current.MainPage = new NavigationPage(new LoginPageCS())
			{
				BarBackgroundColor = Color.FromRgb(15, 15, 15),
				BarTextColor = Color.White//FromRgb(75, 75, 75)
			};
			//_ = Navigation.PushModalAsync(new LoginPageCS());*/

		}

		async Task<int> ChangePassword(string email, string newpassword)
		{
			Debug.WriteLine("ChangePassword");
			MemberManager memberManager = new MemberManager();

			int result = await memberManager.ChangePassword(email, newpassword);

			return result;
			
		}

        async void OndeleteMemberButtonClicked(object sender, EventArgs e)
        {
            bool res = await DisplayAlert("Apagar Sócio?", "Tem a certeza que pretende apagar a sua conta e todos os dados associados? \nATENÇÃO: Esta acçao é irreversível!", "Sim", "Não");

            if (res == true)
            {
                MemberManager memberManager = new MemberManager();
                string result = await memberManager.Update_Member_Approved_Status(App.member.id, "", "", "apagado", "");
                App.Current.MainPage = new NavigationPage(new LoginPageCS(""))
                {
                    BarBackgroundColor = Color.FromRgb(15, 15, 15),
                    BarTextColor = Color.White
                };
            }
        }

    }
}