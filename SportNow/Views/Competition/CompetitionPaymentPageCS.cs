using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;


namespace SportNow.Views
{
	public class CompetitionPaymentPageCS : DefaultPage
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



		private Competition competition_v;

		private List<Payment> payments;

		private Grid gridPaymentOptions;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{


			if (competition_v.value == 0)
			{
				createRegistrationConfirmed();
			}
			else
			{
				createPaymentOptions();
			}
		}

		public async void createRegistrationConfirmed()
		{
			Label inscricaoOKLabel = new Label
			{
				Text = "A sua Inscrição na Competição " + competition_v.name + " está Confirmada. \n\n Boa sorte!",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.alternativeColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 200,
				FontSize = 30
			};

			relativeLayout.Children.Add(inscricaoOKLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(10),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(200)
			);

			Image competitionImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.05 };
			competitionImage.Source = competition_v.imagemSource;

			relativeLayout.Children.Add(competitionImage,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height); // center of image (which is 40 wide)
				})
			);

			CompetitionManager competitionManager = new CompetitionManager();

			await competitionManager.Update_Competition_Participation_Status(competition_v.participationid, "confirmado");
			competition_v.participationconfirmed = "confirmado";

		}

		public void createPaymentOptions() {

			Label selectPaymentModeLabel = new Label
			{
				Text = "Escolha o modo de pagamento pretendido:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.topColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

			relativeLayout.Children.Add(selectPaymentModeLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
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
				yConstraint: Constraint.Constant(130 * App.screenHeightAdapter),
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
				yConstraint: Constraint.Constant(280 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (20 * App.screenHeightAdapter)); // center of image (which is 40 wide)
							}),
				heightConstraint: Constraint.Constant(115 * App.screenHeightAdapter)
			);
		}

		public CompetitionPaymentPageCS(Competition competition_v)
		{

			this.competition_v = competition_v;

			//App.event_participation = event_participation;

			this.initLayout();
			this.initSpecificLayout();

		}


		async void OnMBButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new CompetitionMBPageCS(this.competition_v));
		}


		async void OnMBWayButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("OnMBWayButtonClicked");
			await Navigation.PushAsync(new CompetitionMBWayPageCS(this.competition_v));
		}

	}
}

