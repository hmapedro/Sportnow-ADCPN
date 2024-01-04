using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
//Ausing Acr.UserDialogs;

namespace SportNow.Views.CompleteRegistration
{
	public class CompleteRegistration_PaymentMBWay_PageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
		}

		private Payment payment;

		RegisterButton payButton;

		FormValueEdit phoneValueEdit;

		string paymentID;

		bool paymentDetected;


        public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{

			payment = await GetPayment(this.paymentID);

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

			Label paymentNameLabel = new Label
			{
				Text = payment.name,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.WordWrap,
				FontSize = App.bigTitleFontSize
			};

			relativeLayout.Children.Add(paymentNameLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (10 * App.screenHeightAdapter)); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(100 * App.screenHeightAdapter)
			);


			Label valorLabel = new Label
			{
				Text = "Valor: "+payment.value.ToString("0.00") + "€",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.WordWrap,
				FontSize = App.bigTitleFontSize
			};

			relativeLayout.Children.Add(valorLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(110 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (10 * App.screenHeightAdapter)); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter)
			);

			Image MBWayLogoImage = new Image
			{
				Source = "logombway.png",
				WidthRequest = 184 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
			};

			relativeLayout.Children.Add(MBWayLogoImage,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width / 2) - ((184 * App.screenHeightAdapter) / 2)); // center of image (which is 40 wide)
				}),
				yConstraint: Constraint.Constant(170 * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(184 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(120 * App.screenHeightAdapter)
			);

			Label phoneNumberLabel = new Label
			{
				Text = "Confirme o seu número de telefone",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 50 * App.screenHeightAdapter,
				FontSize = App.bigTitleFontSize
			};

			relativeLayout.Children.Add(phoneNumberLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(280 * App.screenHeightAdapter),
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
				yConstraint: Constraint.Constant(330 * App.screenHeightAdapter),
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


		public CompleteRegistration_PaymentMBWay_PageCS(string paymentID)
		{
			//App.event_participation = event_participation;
			this.paymentID = paymentID;
			this.initLayout();
			this.initSpecificLayout();

			paymentDetected = false;

            int sleepTime = 5;
            Device.StartTimer(TimeSpan.FromSeconds(sleepTime), () =>
            {
                if ((paymentID != null) & (paymentID != ""))
                {
                    this.checkPaymentStatus(paymentID);
                    if (paymentDetected == false)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            });
        }

        async void checkPaymentStatus(string paymentID)
        {
            Debug.Print("checkPaymentStatus");
            this.payment = await GetPayment(paymentID);
            if ((payment.status == "confirmado") | (payment.status == "fechado"))
            {
                App.member.estado = "activo";
                App.original_member.estado = "activo";

                if (paymentDetected == false)
                {
                    paymentDetected = true;

                    await DisplayAlert("Pagamento Confirmado", "O seu pagamento foi recebido com sucesso. Já pode aceder à nossa App!", "Ok");
                    App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                    {
                        BarBackgroundColor = Color.White,
                        BackgroundColor = Color.White,
                        BarTextColor = Color.Black
                    };

                }
            }
        }

        async void OnPayButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			payButton.IsEnabled = false;

			await CreateMbWayPayment(payment);

			hideActivityIndicator();
			payButton.IsEnabled = true;
		}

		async Task<Payment> GetPayment(string paymentID)
		{
			Debug.WriteLine("GetPayment");
			PaymentManager paymentManager = new PaymentManager();

			Payment payment = await paymentManager.GetPayment(this.paymentID);

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
            //await UserDialogs.Instance.AlertAsync(new AlertConfig() { Title = "VALIDAÇÃO DE PAGAMENTO", Message = "Valide o pagamento na App MBWay ou no seu Home Banking. Logo que o faça pode voltar a consultar o estado da sua inscrição e verificar se já se encontra inscrito.", OkText = "Ok" });

/*			App.isToPop = true;
			await Navigation.PopAsync();*/

			App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
			{
				BarBackgroundColor = Color.White,
				BarTextColor = Color.Black//FromRgb(75, 75, 75)
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

