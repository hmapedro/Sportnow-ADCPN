using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;


namespace SportNow.Views
{
	public class MonthFeePaymentPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			if (App.isToPop == true)
			{
				App.isToPop = false;
				Navigation.PopAsync();
			}
			
		}

		
		protected override void OnDisappearing()
		{
		}



		private MonthFee monthFee;

		private Grid gridPaymentOptions;

		public void initLayout()
		{
			Title = "PAGAMENTO MENSALIDADE";



		}


		public async void initSpecificLayout()
		{

			createPaymentOptions();
		}


		public void createPaymentOptions() {


            Label monthFeeNameLabel = new Label
            {
                Text = monthFee.name,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = App.normalTextColor,
                //LineBreakMode = LineBreakMode.NoWrap,
                FontSize = App.bigTitleFontSize
            };

            relativeLayout.Children.Add(monthFeeNameLabel,
				xConstraint: Constraint.Constant(10),
				yConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (20 * App.screenHeightAdapter)); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(80 * App.screenHeightAdapter)
			);

            Label selectPaymentModeLabel = new Label
			{
				Text = "Escolha o modo de pagamento pretendido:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

			relativeLayout.Children.Add(selectPaymentModeLabel,
				xConstraint: Constraint.Constant(10),
				yConstraint: Constraint.Constant(90 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (20 * App.screenHeightAdapter)); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(80 * App.screenHeightAdapter)
			);

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
				MinimumHeightRequest = 115 * App.screenHeightAdapter,
				//WidthRequest = 100 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter,
				//BackgroundColor = Color.Red,
			};

			var tapGestureRecognizerMB = new TapGestureRecognizer();
			tapGestureRecognizerMB.Tapped += OnMBButtonClicked;
			MBLogoImage.GestureRecognizers.Add(tapGestureRecognizerMB);

			relativeLayout.Children.Add(MBLogoImage,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(170 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (20 * App.screenHeightAdapter)); // center of image (which is 40 wide)
							}),
				heightConstraint: Constraint.Constant(115 * App.screenHeightAdapter)
			);


			Image MBWayLogoImage = new Image
			{
				Source = "logombway.png",
				//BackgroundColor = Color.Green,
				//WidthRequest = 184 * App.screenHeightAdapter,
				MinimumHeightRequest = 115 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter
			};

			var tapGestureRecognizerMBWay = new TapGestureRecognizer();
			tapGestureRecognizerMBWay.Tapped += OnMBWayButtonClicked;
			MBWayLogoImage.GestureRecognizers.Add(tapGestureRecognizerMBWay);

			relativeLayout.Children.Add(MBWayLogoImage,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(310 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (20 * App.screenHeightAdapter)); // center of image (which is 40 wide)
							}),
				heightConstraint: Constraint.Constant(115 * App.screenHeightAdapter)
			);
		}

		public MonthFeePaymentPageCS(MonthFee monthFee)
		{

			this.monthFee = monthFee;

			//App.event_participation = event_participation;

			this.initLayout();
			this.initSpecificLayout();

		}


		async void OnMBButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new monthFeeMBPageCS(monthFee));
		}


		async void OnMBWayButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new MonthFeeMBWayPageCS(monthFee));
		}

	}
}

