using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
//Ausing Acr.UserDialogs;
using SportNow.Views.Profile;
using SportNow.CustomViews;

namespace SportNow.Views
{
	public class QuotasPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private CollectionView collectionViewQuotas;

		private Member member;

		private Grid gridInactiveFee, gridActiveFee;

		RoundButton activateButton;


		public void initLayout()
		{
			Title = "QUOTAS";

		}

		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (gridInactiveFee != null)
			{
				relativeLayout.Children.Remove(gridInactiveFee);
				gridInactiveFee = null;
			}
			if (gridActiveFee != null)
			{
				relativeLayout.Children.Remove(gridActiveFee);
				gridActiveFee = null;
			}
			

		}

		public async void initSpecificLayout()
		{

			member = App.member;

			var result = await GetCurrentFees(member);

			bool hasQuotaPayed = false;

			if (App.member.currentFee != null)
			{
				if ((App.member.currentFee.estado == "fechado") | (App.member.currentFee.estado == "recebido") | (App.member.currentFee.estado == "confirmado"))
				{
					hasQuotaPayed = true;
				}
			}

			if (hasQuotaPayed) {
				createActiveFeeLayout();
			}
			else {
				createInactiveFeeLayout();
			}
		}

		public void createInactiveFeeLayout() {
			gridInactiveFee = new Grid { Padding = 10, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = 100 });
			gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			gridInactiveFee.RowDefinitions.Add(new RowDefinition { Height = 50 });
			gridInactiveFee.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //
			gridInactiveFee.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label feeYearLabel = new Label
			{
				Text = member.currentFee.periodo.Replace("_","-"),//DateTime.Now.ToString("yyyy"),
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = 50
			};

			Image akslLogoFee = new Image
			{
				Source = "logo_login.png",
				WidthRequest = 100
			};

			Image fnkpLogoFee = new Image
			{
				Source = "logo_fgp.png",
				WidthRequest = 100
			};

			Label feeInactiveLabel = new Label
			{
				Text = "Quotas Inativas",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.Red,
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = 40
			};

			Label feeInactiveCommentLabel = new Label
			{
				Text = "",//"Atenção: Com as quotas inativas o aluno não poderá participar em eventos e não terá seguro desportivo em caso de lesão.",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.Black,
				FontSize = App.itemTitleFontSize
			};

            activateButton = new RoundButton("ACTIVAR", 100, 40);

			activateButton.button.Clicked += OnActivateButtonClicked;

			gridInactiveFee.Children.Add(feeYearLabel, 0, 0);
			Grid.SetColumnSpan(feeYearLabel, 2);

			gridInactiveFee.Children.Add(fnkpLogoFee, 0, 1);
			gridInactiveFee.Children.Add(akslLogoFee, 1, 1);

			gridInactiveFee.Children.Add(feeInactiveLabel, 0, 2);
			Grid.SetColumnSpan(feeInactiveLabel, 2);

			gridInactiveFee.Children.Add(feeInactiveCommentLabel, 0, 3);
			Grid.SetColumnSpan(feeInactiveCommentLabel, 2);

			gridInactiveFee.Children.Add(activateButton, 0, 5);
			Grid.SetColumnSpan(activateButton, 2);
			

			relativeLayout.Children.Add(gridInactiveFee,
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

		public void createActiveFeeLayout()
		{
			gridActiveFee = new Grid { Padding = 30, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = 100 });
			gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			gridActiveFee.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridActiveFee.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto
			gridActiveFee.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label feeYearLabel = new Label
			{
                Text = member.currentFee.periodo.Replace("_", "-"), //DateTime.Now.ToString("yyyy"),
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = 50
			};

			Image akslLogoFee = new Image
			{
				Source = "logo_login.png",
				WidthRequest = 100
			};

			Image fnkpLogoFee = new Image
			{
				Source = "logo_fgp.png",
				WidthRequest = 100

			};

			Label feeActiveLabel = new Label
			{
				Text = "Quotas Ativas",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.FromRgb(96, 182, 89),
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = 40
			};

            int found = member.currentFee.periodo.IndexOf("_");

            string finish_year = member.currentFee.periodo.Substring(found + 1);

            Label feeActiveDueDateLabel = new Label
			{
				Text = "Válida até 31-08-"+finish_year,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = 35
			};


			gridActiveFee.Children.Add(feeYearLabel, 0, 0);
			Grid.SetColumnSpan(feeYearLabel, 2);

			gridActiveFee.Children.Add(fnkpLogoFee, 0, 1);
			gridActiveFee.Children.Add(akslLogoFee, 1, 1);

			gridActiveFee.Children.Add(feeActiveLabel, 0, 2);
			Grid.SetColumnSpan(feeActiveLabel, 2);

			gridActiveFee.Children.Add(feeActiveDueDateLabel, 0, 3);
			Grid.SetColumnSpan(feeActiveDueDateLabel, 2);

			relativeLayout.Children.Add(gridActiveFee,
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

		public QuotasPageCS()
		{
			Debug.Print("App.member.dojo_faturavel = " + App.member.dojo_faturavel);
			Debug.Print("App.member.dojo_seguro = " + App.member.dojo_seguro);
			Debug.Print("App.member.member_type = " + App.member.member_type);

			this.initLayout();
			//this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfilePageCS());
		}

		async void OnActivateButtonClicked(object sender, EventArgs e)
		{

			Debug.Print("App.member.member_type = "+App.member.member_type);
			showActivityIndicator();
			activateButton.IsEnabled = false;

			MemberManager memberManager = new MemberManager();


			if (App.member.currentFee is null) {

				var result_create = 0;
				/*string fee_type = "";

				if (App.member.member_type == "praticante")
				{
					//if (App.member.dojo == "Ippon - Sporting")
					if (App.member.dojo_seguro == "1")
					{
						if (App.member.isTreinador == "1")
						{
							fee_type = "treinador_nksl_fnkp";
						}
						else if (App.member.isTreinador == "0")
						{
							fee_type = "nksl_fnkp";
						}
					}
					else
					{
						if (App.member.isTreinador == "1")
						{
							fee_type = "treinador_nksl_fnkp_seguro";
						}
						else if (App.member.isTreinador == "0")
						{
							fee_type = "nksl_fnkp_seguro";
						}
					}
				}
				else
				{
					fee_type = "nao_praticante";
				}*/

				//Debug.Print("fee_type=" + fee_type);
				result_create = await memberManager.CreateFee(member, DateTime.Now.ToString("yyyy"));
				if (result_create == -1)
				{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				}

				var result_get = await GetCurrentFees(member);
				if (result_create == -1)
				{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				}
			}
			
			await Navigation.PushAsync(new QuotasPaymentPageCS(member));
            hideActivityIndicator();
        }
		

		async Task<int> GetCurrentFees(Member member)
		{
			Debug.WriteLine("GetCurrentFees");
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

