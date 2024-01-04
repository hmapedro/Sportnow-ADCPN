using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SportNow.Views
{
	public class IntroPageCS : ContentPage
	{



		public List<MainMenuItem> MainMenuItems { get; set; }

		private Member member;

		Label msg;
		Button btn;


		protected override void OnAppearing()
		{
			Constants.ScreenWidth = Application.Current.MainPage.Width;//DeviceDisplay.MainDisplayInfo.Width;
			Constants.ScreenHeight = Application.Current.MainPage.Height; //DeviceDisplay.MainDisplayInfo.Height;
			//Debug.Print("ScreenWidth = "+ Constants.ScreenWidth + " ScreenHeight = " + Constants.ScreenHeight);
		}

		public void initLayout()
		{
			Title = "Aguarde";
			BackgroundColor = Color.White;



		}



		public IntroPageCS ()
		{
			this.initLayout();
			
		}
	}
}
