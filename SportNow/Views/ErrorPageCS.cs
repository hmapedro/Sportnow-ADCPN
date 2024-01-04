using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;

namespace SportNow.Views
{
	public class ErrorPageCS : DefaultPage
	{



		public List<MainMenuItem> MainMenuItems { get; set; }

		private Member member;

		Label msg;
		Button btn;


		protected override void OnAppearing()
		{
			Constants.ScreenWidth = Application.Current.MainPage.Width;//DeviceDisplay.MainDisplayInfo.Width;
			Constants.ScreenHeight = Application.Current.MainPage.Height; //DeviceDisplay.MainDisplayInfo.Height;
			Debug.Print("ScreenWidth = "+ Constants.ScreenWidth + " ScreenHeight = " + Constants.ScreenHeight);
		}

		public void initLayout()
		{
			Title = "Home";


			Label errorLabel = new Label
			{
				Text = "Verifique a sua ligação à Internet. No caso do problema persistir entre em contacto connosco através de info@nksl.org",
				TextColor = Color.White,
				FontSize = 25,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				HorizontalTextAlignment = TextAlignment.Center
			};

			relativeLayout.Children.Add(errorLabel,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height/2 - 150); }),
			widthConstraint: Constraint.RelativeToParent((parent) => { return parent.Width; }),
			heightConstraint: Constraint.Constant(300));

			RoundButton retryButton = new RoundButton("Tentar novamente", 100, 40);
			retryButton.button.Clicked += OnRetryButtonClicked;


			relativeLayout.Children.Add(retryButton,
			xConstraint: Constraint.RelativeToParent((parent) => { return (parent.Width / 2 - 20); }),
			yConstraint: Constraint.RelativeToParent((parent) => { return (parent.Height / 2 + 150); }),
			widthConstraint: Constraint.Constant(100),
			heightConstraint: Constraint.Constant(40));



		}

		async void OnRetryButtonClicked(object sender, EventArgs e)
		{
			Navigation.InsertPageBefore(new MainTabbedPageCS("",""), this);
			await Navigation.PopAsync();
		}


		public ErrorPageCS()
		{
			this.initLayout();
			
		}
	}
}
