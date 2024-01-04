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
	public class EventMBWayPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
		}

		private Event_Participation event_participation;
		private Event event_v;

		private List<Payment> payments;

		RegisterButton payButton;

		FormValueEdit phoneValueEdit;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{

			payments = await GetEventParticipationPayment(event_participation);

			createLayoutPhoneNumber();

		}

		public async void createLayoutPhoneNumber()
		{

			Label eventParticipationNameLabel = new Label
			{
				Text = "Para confirmar a sua presença no(a) " + event_participation.evento_name + " efetue o pagamento de "+ event_participation.valor + "€.",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.Black,
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
				TextColor = Color.Black,
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
				heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter)
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


		public EventMBWayPageCS(Event event_v, Event_Participation event_participation)
		{

			this.event_participation = event_participation;
			this.event_v = event_v;
			//App.event_participation = event_participation;

			this.initLayout();
			this.initSpecificLayout();

		}

		async void OnPayButtonClicked(object sender, EventArgs e)
		{
            showActivityIndicator();
            payButton.IsEnabled = false;

			await CreateMbWayPayment(payments[0]);

            hideActivityIndicator();
            payButton.IsEnabled = true;
		}

		async Task<List<Payment>> GetEventParticipationPayment(Event_Participation event_participation)
		{
			Debug.WriteLine("GetEventParticipationPayment");
			EventManager eventManager = new EventManager();

			List<Payment> payments = await eventManager.GetEventParticipation_Payment(event_participation.id);
			if (payments == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return payments;
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
            //await UserDialogs.Instance.AlertAsync(new AlertConfig() { Title = "VALIDAÇÃO DE PAGAMENTO", Message = "Valide o pagamento na App MBWay ou no seu Home Banking. Logo que o faça pode voltar a consultar o estado da sua inscrição e verificar se já se encontra inscrito.", OkText = "Ok" });

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

