using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;


namespace SportNow.Views
{
	public class ExaminationSessionMBPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
			//App.competition_participation = competition_participation;

			//registerButton.IsEnabled = true;
			//estadoValue.entry.Text = App.competition_participation.estado;
		}



		private Examination_Session examination_session;

		private List<Payment> payments;

		private Grid gridMBPayment;

		public void initLayout()
		{
			Title = "INSCRIÇÃO";

		}


		public async void initSpecificLayout()
		{

			payments = await GetExaminationSession_Payment(examination_session);

			if ((payments == null) | (payments.Count == 0))
			{
				createRegistrationConfirmed();
			}
			else {
				createMBPaymentLayout();
			}
		}

		public async void createRegistrationConfirmed()
		{
			Label inscricaoOKLabel = new Label
			{
				Text = "A sua Inscrição na " + examination_session.name + " está Confirmada. \n Boa sorte e nunca se esqueça de se divertir!",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 400,
				FontSize = 30
			};

			relativeLayout.Children.Add(inscricaoOKLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(10),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
							}),
				heightConstraint: Constraint.Constant(80)
			);

			CompetitionManager competitionManager = new CompetitionManager();

			await competitionManager.Update_Competition_Participation_Status(examination_session.participationid, "confirmado");
			examination_session.participationconfirmed = "confirmado";

		}

		public void createMBPaymentLayout() {
			gridMBPayment= new Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 100 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = 20 });
			gridMBPayment.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridMBPayment.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label competitionParticipationNameLabel = new Label
			{
				Text = "Para confirmar a sua presença na\n " + examination_session.name + "\n efetue o pagamento no MB com os dados apresentados em baixo",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
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
				TextColor = Color.White,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 100,
				FontSize = 30
			};

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
				TextColor = Color.White,
				FontSize = 20
			};
			Label referenceLabel = new Label
			{
				Text = "Referência:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = Color.White,
				FontSize = 20
			};
			Label valueLabel = new Label
			{
				Text = "Valor:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = Color.White,
				FontSize = 20
			};

			Label entityValue = new Label
			{
				Text = payments[0].entity,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = Color.White,
				FontSize = 20
			};
			Label referenceValue = new Label
			{
				Text = payments[0].reference,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = Color.White,
				FontSize = 20
			};
			Label valueValue = new Label
			{
                Text = String.Format("{0:0.00}", payments[0].value) + "€",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = Color.White,
				FontSize = 20
			};

			Frame MBDataFrame= new Frame { BackgroundColor = Color.FromRgb(25, 25, 25), BorderColor = Color.Yellow, CornerRadius = 10, IsClippedToBounds = true, Padding = 0 };
			MBDataFrame.Content = gridMBDataPayment;

			gridMBDataPayment.Children.Add(entityLabel, 0, 0);
			gridMBDataPayment.Children.Add(entityValue, 1, 0);
			gridMBDataPayment.Children.Add(referenceLabel, 0, 1);
			gridMBDataPayment.Children.Add(referenceValue, 1, 1);
			gridMBDataPayment.Children.Add(valueLabel, 0, 2);
			gridMBDataPayment.Children.Add(valueValue, 1, 2);

			gridMBDataPayment.Children.Add(referenceLabel, 0, 1);
			gridMBDataPayment.Children.Add(valueLabel, 0, 2);

			gridMBPayment.Children.Add(competitionParticipationNameLabel, 0, 0);
			Grid.SetColumnSpan(competitionParticipationNameLabel, 2);

			gridMBPayment.Children.Add(MBLogoImage, 0, 2);
			gridMBPayment.Children.Add(referenciaMBLabel, 1, 2);

			gridMBPayment.Children.Add(MBDataFrame, 0, 4);
			Grid.SetColumnSpan(MBDataFrame, 2);


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

		public ExaminationSessionMBPageCS(Examination_Session examination_session)
		{

			this.examination_session = examination_session;
			//App.competition_participation = competition_participation;

			this.initLayout();
			this.initSpecificLayout();

		}

		async Task<List<Payment>> GetExaminationSession_Payment(Examination_Session examination_session)
		{
			Debug.WriteLine("GetExaminationSession_Payment");
			ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();

			Debug.Print("examination_session.participationid = " + examination_session.participationid);
			List<Payment> payments = await examination_sessionManager.GetExamination_Payment(examination_session.participationid);
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

