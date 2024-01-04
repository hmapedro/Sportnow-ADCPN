using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
//Ausing Acr.UserDialogs;

namespace SportNow.Views
{
	public class CompetitionMBWayPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
		}



		private Competition competition_v;

		private Payment payment;

		RegisterButton payButton;

		FormValueEdit phoneValueEdit;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";

		}


		public async void initSpecificLayout()
		{

			payment = await GetCompetitionParticipationPayment(this.competition_v);

			createLayoutPhoneNumber();
			/*
			if ((payments == null) | (payments.Count == 0))
			{
				createRegistrationConfirmed();
			}
			else {
				createMBPaymentLayout();
			}*/
		}

		public async void createLayoutPhoneNumber()
		{

			Label eventParticipationNameLabel = new Label
			{
                Text = "Para confirmar a sua presença na competição\n " + competition_v.name + "\n efetue o pagamento de " + competition_v.value + "€.",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.topColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

			relativeLayout.Children.Add(eventParticipationNameLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (10 * App.screenHeightAdapter)); // center of image (which is 40 wide)
							}),
				heightConstraint: Constraint.Constant(160 * App.screenHeightAdapter)
			);

			Image MBWayLogoImage = new Image
			{
				Source = "logombway.png",
				WidthRequest = 184 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter

			};

			relativeLayout.Children.Add(MBWayLogoImage,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width / 2) - ((184 * App.screenHeightAdapter) / 2)); // center of image (which is 40 wide)
				}),
				yConstraint: Constraint.Constant(180 * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(184 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(120 * App.screenHeightAdapter)
			);

			Label phoneNumberLabel = new Label
			{
				Text = "Confirme o seu número de telefone",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 50 * App.screenHeightAdapter,
				FontSize = App.bigTitleFontSize
			};

			relativeLayout.Children.Add(phoneNumberLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(290 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (10 * App.screenHeightAdapter)); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter)
			);

			phoneValueEdit = new FormValueEdit(App.member.phone);
			phoneValueEdit.entry.HorizontalTextAlignment = TextAlignment.Center;
			phoneValueEdit.entry.FontSize = App.titleFontSize;


			relativeLayout.Children.Add(phoneValueEdit,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(340 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 10); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
			);

			payButton = new RegisterButton("PAGAR", 100, 50);
			payButton.button.Clicked += OnPayButtonClicked;


			relativeLayout.Children.Add(payButton,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height - (60 * App.screenHeightAdapter));
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter)
			);
		}


		public CompetitionMBWayPageCS(Competition competition_v)
		{
			this.competition_v = competition_v;
			//App.event_participation = event_participation;

			this.initLayout();
			this.initSpecificLayout();

		}

		async void OnPayButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			payButton.IsEnabled = false;

			await CreateMbWayPayment(payment);

            hideActivityIndicator();
            payButton.IsEnabled = true;
		}

		async Task<Payment> GetCompetitionParticipationPayment(Competition competition)
		{
			Debug.WriteLine("GetCompetitonParticipationPayment");
			CompetitionManager competitionManager = new CompetitionManager();

			Payment payment = await competitionManager.GetCompetitionParticipation_Payment(competition);
			if (payment == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return payment;
		}

		async Task<string> CreateMbWayPayment(Payment payment)
		{
			Debug.WriteLine("CreateMbWayPayment");
			showActivityIndicator();

			PaymentManager paymentManager = new PaymentManager();

			string value_string = Convert.ToString(payment.value);
			string result = await paymentManager.CreateMbWayPayment(App.original_member.id, payment.id, payment.orderid, phoneValueEdit.entry.Text, value_string, App.member.email);
			if ((result == "-2") | (result == "-3"))
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
                hideActivityIndicator();
                return null;
			}
            hideActivityIndicator();
            await DisplayAlert("VALIDAÇÃO DE PAGAMENTO", "Valide o pagamento na App MBWay ou no seu Home Banking. Logo que o faça pode voltar a consultar o estado da sua inscrição e verificar se já se encontra inscrito.", "OK");

/*			App.isToPop = true;
			await Navigation.PopAsync();*/

			App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
			{
				BarBackgroundColor = Color.FromRgb(15, 15, 15),
				BarTextColor = Color.White//FromRgb(75, 75, 75)
			};



			/*			Application.Current.MainPage = new NavigationPage(new DetailEventPageCS(event_v))
						{
							BarBackgroundColor = Color.FromRgb(15, 15, 15),
							BarTextColor = Color.White
						};*/

			return result;
		}

	}
}

