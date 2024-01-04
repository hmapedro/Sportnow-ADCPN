using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;


namespace SportNow.Views
{
	public class EquipamentOrderMBPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
		}



		private EquipmentOrder equipmentOrder;

		private Grid gridMBPayment;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{
			createMBPaymentLayout();
		}

		public void createMBPaymentLayout() {
			gridMBPayment= new Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 140 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label competitionParticipationNameLabel = new Label
			{
				Text = "Para confirmar a sua \n " + equipmentOrder.name + "\n efetue o pagamento no MB com os dados apresentados em baixo.",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.Black,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
				WidthRequest = 184 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter

			};

			Label referenciaMBLabel = new Label
			{
				Text = "Pagamento por\n Multibanco",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.Black,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 115 * App.screenHeightAdapter,
				FontSize = App.bigTitleFontSize
			};

			/*gridMBDataPayment.Children.Add(referenceLabel, 0, 1);
			gridMBDataPayment.Children.Add(valueLabel, 0, 2);
			*/
			gridMBPayment.Children.Add(competitionParticipationNameLabel, 0, 0);
			Grid.SetColumnSpan(competitionParticipationNameLabel, 2);

			gridMBPayment.Children.Add(MBLogoImage, 0, 2);
			gridMBPayment.Children.Add(referenciaMBLabel, 1, 2);

			createMBGrid();

			relativeLayout.Children.Add(gridMBPayment,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(10),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 10; // center of image (which is 40 wide)
				})
			);
		}

		public void createMBGrid()
		{
			Grid gridMBDataPayment = new Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBDataPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

			Label entityLabel = new Label
			{
				Text = "Entidade:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = Color.Black,
				FontSize = App.titleFontSize
			};
			Label referenceLabel = new Label
			{
				Text = "Referência:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = Color.Black,
				FontSize = App.titleFontSize
			};
			Label valueLabel = new Label
			{
				Text = "Valor:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = Color.Black,
				FontSize = App.titleFontSize
			};

			Label entityValue = new Label
			{
				Text = equipmentOrder.entidade,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = Color.Black,
				FontSize = App.titleFontSize
			};
			Label referenceValue = new Label
			{
				Text = equipmentOrder.referencia_mb,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = Color.Black,
				FontSize = App.titleFontSize
			};
			Label valueValue = new Label
			{
				Text = String.Format("{0:0.00}", equipmentOrder.valor) + "€",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = Color.Black,
				FontSize = App.titleFontSize
			};

			Frame MBDataFrame = new Frame { HasShadow = false, BackgroundColor = Color.White, BorderColor = App.topColor, CornerRadius = 10, IsClippedToBounds = true, Padding = 0 };
			MBDataFrame.Content = gridMBDataPayment;

			gridMBDataPayment.Children.Add(entityLabel, 0, 0);
			gridMBDataPayment.Children.Add(entityValue, 1, 0);
			gridMBDataPayment.Children.Add(referenceLabel, 0, 1);
			gridMBDataPayment.Children.Add(referenceValue, 1, 1);
			gridMBDataPayment.Children.Add(valueLabel, 0, 2);
			gridMBDataPayment.Children.Add(valueValue, 1, 2);

			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			gridMBPayment.Children.Add(MBDataFrame, 0, 4);
			Grid.SetColumnSpan(MBDataFrame, 2);
		}


		public EquipamentOrderMBPageCS(EquipmentOrder equipmentOrder)
		{
			this.equipmentOrder = equipmentOrder;
			this.initLayout();
			this.initSpecificLayout();
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

