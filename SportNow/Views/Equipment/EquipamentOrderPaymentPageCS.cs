﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;


namespace SportNow.Views
{
	public class EquipamentOrderPaymentPageCS : DefaultPage
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

		private EquipmentOrder equipmentOrder;

		private Grid gridPaymentOptions;

		public void initLayout()
		{
			Title = "ENCOMENDA EQUIPAMENTO";

			relativeLayout = new RelativeLayout
			{
				Margin = new Thickness(0)
			};
			Content = relativeLayout;

			NavigationPage.SetBackButtonTitle(this, "");
		}


		public async void initSpecificLayout()
		{

            createPaymentOptions();

		}

		public void createPaymentOptions() {


            Label eventoLabel = new Label
            {
                Text = equipmentOrder.name,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,
                //LineBreakMode = LineBreakMode.NoWrap,
                FontSize = App.bigTitleFontSize
            };

            relativeLayout.Children.Add(eventoLabel,
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
				TextColor = Color.Black,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

			relativeLayout.Children.Add(selectPaymentModeLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
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
				yConstraint: Constraint.Constant(200 * App.screenHeightAdapter),
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
				yConstraint: Constraint.Constant(330 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (20 * App.screenHeightAdapter)); // center of image (which is 40 wide)
							}),
				heightConstraint: Constraint.Constant(115 * App.screenHeightAdapter)
			);
		}

		public EquipamentOrderPaymentPageCS(EquipmentOrder equipmentOrder)
		{

			this.equipmentOrder = equipmentOrder;
			Debug.Print("this.equipmentOrder.name = " + this.equipmentOrder.name);

			this.initLayout();
			this.initSpecificLayout();

		}


		async void OnMBButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new EquipamentOrderMBPageCS(equipmentOrder));
		}


		async void OnMBWayButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new EquipamentOrderMBWayPageCS(equipmentOrder));
		}

		async Task<List<Payment>> GetEventParticipationPayment(Event_Participation event_participation)
		{
			Debug.WriteLine("GetCompetitionParticipationPayment");
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

	}
}

