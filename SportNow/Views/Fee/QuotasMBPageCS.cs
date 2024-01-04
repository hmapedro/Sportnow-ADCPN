using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.Views.Profile;

namespace SportNow.Views
{
	public class QuotasMBPageCS : DefaultPage
	{



		private Member member;

		private Grid gridMBPayment;

		public void initLayout()
		{
			Title = "Quotas";
		}


		public async void initSpecificLayout()
		{

			member = App.member;

			var result = await GetFeePayment(member);

			
			createMBPaymentLayout();
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

			Label feeYearLabel = new Label
			{
                Text = "Para efetuar o pagamento da sua " + member.currentFee.name.Substring(member.currentFee.name.IndexOf(" - ") + 3) + " - " + member.currentFee.valor + "€ use os dados indicados em baixo.",
                VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = 50
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
				TextColor = App.normalTextColor,
				//LineBreakMode = LineBreakMode.NoWrap,
				HeightRequest = 100,
				FontSize = 30
			};

			/*Label feeInactiveCommentLabel = new Label
			{
				Text = "Atenção: Com as quotas inativas o aluno não poderá participar em eventos e não terá seguro desportivo em caso de lesão.",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.White,
				FontSize = 20
			};

			Button activateButton = new Button
			{
				Text = "ATIVAR",
				BackgroundColor = Color.Green,
				TextColor = Color.White,
				WidthRequest = 100,
				HeightRequest = 50
			};
			//activateButton.Clicked += OnActivateButtonClicked;

			*/

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
				TextColor = App.normalTextColor,
                FontSize = 20
			};
			Label referenceLabel = new Label
			{
				Text = "Referência:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
                FontSize = 20
			};
			Label valueLabel = new Label
			{
				Text = "Valor:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Start,
				TextColor = App.normalTextColor,
                FontSize = 20
			};

			Label entityValue = new Label
			{
				Text = App.member.currentFee.entidade,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
                FontSize = 20
			};
			Label referenceValue = new Label
			{
				Text = App.member.currentFee.referencia,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
                FontSize = 20
			};
			Label valueValue = new Label
			{
                Text = String.Format("{0:0.00}", App.member.currentFee.valor) + "€",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.End,
				TextColor = App.normalTextColor,
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

			gridMBPayment.Children.Add(feeYearLabel, 0, 0);
			Grid.SetColumnSpan(feeYearLabel, 2);

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

		public QuotasMBPageCS(Member member)
		{

			this.member = member;

			this.initLayout();
			this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfilePageCS());
		}


		async Task<int> GetFeePayment(Member member)
		{
			Debug.WriteLine("GetFeePayment");
			MemberManager memberManager = new MemberManager();

			var result = await memberManager.GetCurrentFees(member);
			if (result == -1)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return result;
			}
			return result;
		}

	}
}

