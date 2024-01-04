using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;
//Ausing Acr.UserDialogs;

namespace SportNow.Views.Profile
{
    public class NewMemberSuccessPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
			initLayout();
			initSpecificLayout();
		}


		protected async override void OnDisappearing()
		{
			if (relativeLayout != null)
			{
				relativeLayout = null;
				this.Content = null;
			}

		}

		//Image estadoQuotaImage;

		Label titleLabel;

		public void initLayout()
		{
			Title = "BEM-VINDO";
            NavigationPage.SetBackButtonTitle(this, "");

        }


		public async void initSpecificLayout()
		{
			if (relativeLayout == null)
			{
				initBaseLayout();
            }


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
                yConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
                widthConstraint: Constraint.Constant(140 * App.screenHeightAdapter)
            );

            Label labelSucesso = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.bigTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
            labelSucesso.Text = "OBRIGADO "+ App.member.name.Split(' ')[0].ToUpper() +"!\n\n Os responsáveis da ADPCN serão avisados que efetuou o seu pedido de inscrição.\n Logo que a inscrição seja aprovada poderá começar a utilizar a nossa App.";
			relativeLayout.Children.Add(labelSucesso,
				xConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(150 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (60 * App.screenHeightAdapter));
				}),
                heightConstraint: Constraint.Constant(300 * App.screenHeightAdapter)
            );



            Image logo_ippon = new Image
            {
                Source = "logo_aksl_round.png",
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 224 * App.screenHeightAdapter
            };
            relativeLayout.Children.Add(logo_ippon,
				xConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(350 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (60 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(224 * App.screenHeightAdapter)

            );

            RoundButton confirmButton = new RoundButton("VOLTAR AO LOGIN", 100, 50);
			confirmButton.button.Clicked += confirmConsentButtonClicked;

			relativeLayout.Children.Add(confirmButton,
				xConstraint: Constraint.Constant(10),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (60 * App.screenHeightAdapter); // 
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 20 * App.screenHeightAdapter);
				}),
				heightConstraint: Constraint.Constant((50 * App.screenHeightAdapter))
			);

            confirmButton.button.Clicked += confirmConsentButtonClicked;

            relativeLayout.Children.Add(confirmButton,
                xConstraint: Constraint.Constant(10),
                yConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height) - (60 * App.screenHeightAdapter); // 
                }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width - 20 * App.screenHeightAdapter);
                }),
                heightConstraint: Constraint.Constant((50 * App.screenHeightAdapter))
            );


        }

		public NewMemberSuccessPageCS()
		{
            this.initLayout();
            this.initSpecificLayout();
        }

		async void confirmConsentButtonClicked(object sender, EventArgs e)
		{
            Application.Current.MainPage = new NavigationPage(new LoginPageCS("Por favor aguarde que o treinador aprove a sua inscrição."))
            {
                BarBackgroundColor = Color.FromRgb(15, 15, 15),
                BarTextColor = Color.White
            };
        }
	}

}