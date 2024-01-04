using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using System.Diagnostics;
using SportNow.CustomViews;
using System;

namespace SportNow.Views.CompleteRegistration
{
	public class CompleteRegistration_Begin_PageCS : DefaultPage
	{
		bool dialogShowing;

		protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{
		}

		//Image estadoQuotaImage;
		private CollectionView collectionViewMembers;
		List<Member> members_To_Approve;
		Label titleLabel;

		public void initLayout()
		{
			Debug.Print("CompleteRegistration_Begin_PageCS - initLayout");
			Title = "INSCRIÇÃO";

            //NavigationPage.SetHasNavigationBar(this, false);

            App.AdaptScreen();
            Image logo_aksl = new Image
			{
				Source = "logo_login.png",
				HorizontalOptions = LayoutOptions.Center,
				Opacity = 0.8,
			};
			relativeLayout.Children.Add(logo_aksl,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Height / 2) - (((parent.Width) - (40 * App.screenHeightAdapter)) / 2));
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (40 * App.screenHeightAdapter));
				})
			);

			var toolbarItem = new ToolbarItem
			{
				Text = "Logout",
				
			};
			toolbarItem.Clicked += OnLogoutButtonClicked;
			ToolbarItems.Add(toolbarItem);

			NavigationPage.SetHasBackButton(this, false);
		}


		public void initSpecificLayout()
		{
			Debug.Print("CompleteRegistration_Begin_PageCS - initSpecificLayout");

			Label welcomeLabel = new Label
			{
				Text = "BEM-VINDO À ADCPN",
				TextColor = App.topColor,
				FontSize = App.bigTitleFontSize,
				HorizontalOptions = LayoutOptions.Center
			};
			relativeLayout.Children.Add(welcomeLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
			);


			Label welcomeLabel1 = new Label
			{
				Text = "Para terminar o seu processo de inscrição, clique em 'Continuar'",
				TextColor = App.topColor,
				FontSize = App.titleFontSize,
				HorizontalOptions = LayoutOptions.Center
			};
			relativeLayout.Children.Add(welcomeLabel1,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(70 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
			);

			RoundButton confirmButton = new RoundButton("CONTINUAR", 100, 50);
			confirmButton.button.Clicked += OnConfirmButtonClicked;

			relativeLayout.Children.Add(confirmButton,
				xConstraint: Constraint.Constant(10),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 60; // 
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 20 * App.screenHeightAdapter);
				}),
				heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter));

		}

		public CompleteRegistration_Begin_PageCS()
		{
			Debug.Print("CompleteRegistration_Begin_PageCS()");
			this.initLayout();
			this.initSpecificLayout();
		}

		async void OnConfirmButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new CompleteRegistration_Consent_PageCS());
			//await Navigation.PushAsync(new CompleteRegistration_Payment_PageCS());
		}

		async void OnLogoutButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnLogoutButtonClicked");

			Application.Current.Properties.Remove("EMAIL");
			Application.Current.Properties.Remove("PASSWORD");
			Application.Current.Properties.Remove("SELECTEDUSER");

			App.member = null;
			App.members = null;

			Application.Current.SavePropertiesAsync();


			Application.Current.MainPage = new NavigationPage(new LoginPageCS(""))
			{
				BarBackgroundColor = Color.FromRgb(15, 15, 15),
				BarTextColor = Color.White//FromRgb(75, 75, 75)
			};
		}
	}

}