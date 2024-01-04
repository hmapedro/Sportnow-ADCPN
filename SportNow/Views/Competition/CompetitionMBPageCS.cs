using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;


namespace SportNow.Views
{
	public class CompetitionMBPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
			//App.competition_participation = competition_participation;

			//registerButton.IsEnabled = true;
			//estadoValue.entry.Text = App.competition_participation.estado;
		}



		private Competition competition;
		//private List<Competition> competitions;

		private Payment payment;

		private Grid gridMBPayment;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";
		}


		public async void initSpecificLayout()
		{

			payment = await GetCompetitionParticipationPayment(this.competition);

			if (payment == null)
			{
				createRegistrationConfirmed();
			}
			else if (payment.value == 0)
			{
				createRegistrationConfirmed();
			}
			else
			{
				createMBPaymentLayout();
			}
		}

		public async void createRegistrationConfirmed()
		{
			Label inscricaoOKLabel = new Label
			{
				Text = "A sua Inscrição na Competição " + competition.name + " está Confirmada. \n Boa sorte e nunca se esqueças de se divertir!",
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

			CompetitionManager competitionManager = new CompetitionManager();

			await competitionManager.Update_Competition_Participation_Status(competition.participationid, "confirmado");
			competition.participationconfirmed = "confirmado";

		}

		public void createMBPaymentLayout() {
			gridMBPayment= new Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 100 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 10 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 10 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label competitionParticipationNameLabel = new Label
			{
				Text = "Para confirmar a sua presença na competição\n " + competition.name + "\n efetue o pagamento MB com os dados apresentados em baixo.",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.topColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = 20
			};

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
				WidthRequest = 100,
				HeightRequest = 100
			};

			Label referenciaMBLabel = new Label
			{
				Text = "Pagamento por\n Multibanco",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.topColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 100,
				FontSize = 30
			};

			gridMBPayment.Children.Add(competitionParticipationNameLabel, 0, 0);
			Grid.SetColumnSpan(competitionParticipationNameLabel, 2);

			gridMBPayment.Children.Add(MBLogoImage, 0, 2);
			gridMBPayment.Children.Add(referenciaMBLabel, 1, 2);

			createMBGrid(payment, competition.participationcategory);

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

		public void createMBGrid(Payment payment, string category)
		{
			Grid gridMBDataPayment = new Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBDataPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBDataPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto

			Label categoryLabel = new Label
			{
				Text = "Escalão:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.topColor,
				FontSize = 18
			};
			Label categoryValue = new Label
			{
				Text = category,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = 15
			};

			Label entityLabel = new Label
			{
				Text = "Entidade:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.topColor,
				FontSize = 18
			};
			Label referenceLabel = new Label
			{
				Text = "Referência:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.topColor,
				FontSize = 18
			};

            Label referenceValue = new Label
            {
                Text = payment.reference,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = App.normalTextColor,
                FontSize = 18
            };


            Label valueLabel = new Label
			{
				Text = "Valor:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.topColor,
				FontSize = 18
			};

			Label entityValue = new Label
			{
				Text = payment.entity,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = 18
			};

			Label valueValue = new Label
			{
				Text = String.Format("{0:0.00}", payment.value) + "€",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
				FontSize = 18
			};

			Frame MBDataFrame = new Frame { BackgroundColor = Color.Transparent, BorderColor = App.bottomColor, CornerRadius = 10, IsClippedToBounds = true, Padding = 0, HasShadow = false };
			MBDataFrame.Content = gridMBDataPayment;

			gridMBDataPayment.Children.Add(categoryLabel, 0, 0);
			gridMBDataPayment.Children.Add(categoryValue, 1, 0);
			gridMBDataPayment.Children.Add(entityLabel, 0, 1);
			gridMBDataPayment.Children.Add(entityValue, 1, 1);
			gridMBDataPayment.Children.Add(referenceLabel, 0, 2);
			gridMBDataPayment.Children.Add(referenceValue, 1, 2);
			gridMBDataPayment.Children.Add(valueLabel, 0, 3);
			gridMBDataPayment.Children.Add(valueValue, 1, 3);

			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			gridMBPayment.Children.Add(MBDataFrame, 0, 4 + 2);
			Grid.SetColumnSpan(MBDataFrame, 2);
		}

		public CompetitionMBPageCS(Competition competition)
		{

			this.competition = competition;
			//App.competition_participation = competition_participation;

			this.initLayout();
			this.initSpecificLayout();

		}

		async Task<Payment> GetCompetitionParticipationPayment(Competition competition)
		{
			Debug.WriteLine("GetCompetitionParticipationPayment");
			CompetitionManager competitionManager = new CompetitionManager();

			Payment payment = await competitionManager.GetCompetitionParticipation_Payment(competition);
			if (payment == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.FromRgb(15, 15, 15), BarTextColor = Color.White
				};
				return null;
			}
			return payment;
		}

	}
}

