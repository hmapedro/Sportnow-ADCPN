using System;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;

namespace SportNow.Views
{
	public class EquipamentsOrderPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			//initSpecificLayout();
		}



		Equipment equipment;

		public void initLayout()
		{
			Title = "ENCOMENDA EQUIPAMENTO";


		}


		public void CleanScreen()
		{

		}

		public async void initSpecificLayout()
		{
			CreateEquipmentView();
		}

		

		public void CreateEquipmentView()
		{

			string subType = "";

			if (equipment.type == "fato_treino")
			{
				subType = "Fato de Treino Oficial do Clube";
			}
			else if (equipment.type == "equipamento_treino")
			{
				subType = "Equipameto de Treino";
			}

			Label subtypeLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
			subtypeLabel.Text = subType + " - " + equipment.subtype;

			relativeLayout.Children.Add(subtypeLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 5 * 4) - (10 * App.screenHeightAdapter); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

			Label valueTitleLabel = new Label { Text = "VALOR", BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };

			relativeLayout.Children.Add(valueTitleLabel,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 5 * 4); // center of image (which is 40 wide)
				}),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 5) - 10; // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 30 * App.screenHeightAdapter; // 
				}));


			Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTitleFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
			nameLabel.Text = equipment.name;

			Frame nameFrame = new Frame
			{
				BackgroundColor = Color.White,
				BorderColor = Color.FromRgb(43, 53, 129),
				CornerRadius = 10,
				IsClippedToBounds = true,
				Padding = new Thickness(5, 0, 0, 0),
				HasShadow = false
			};
			nameFrame.Content = nameLabel;

			relativeLayout.Children.Add(nameFrame,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(40 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 5 * 4) - 10; // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 30 * App.screenHeightAdapter; // 
			}));

			Label valueLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			valueLabel.Text = equipment.valueFormatted;

			Frame valueFrame = new Frame
			{
				BackgroundColor = Color.White,
				BorderColor = Color.FromRgb(43, 53, 129),
				CornerRadius = 10,
				IsClippedToBounds = true,
				Padding = new Thickness(5, 0, 0, 0),
				HasShadow = false
			};
			valueFrame.Content = valueLabel;

			relativeLayout.Children.Add(valueFrame,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 5 * 4); // center of image (which is 40 wide)
				}),
				yConstraint: Constraint.Constant(40 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 5) - 10; // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 30 * App.screenHeightAdapter; // 
			}));

			RoundButton orderButton = new RoundButton("ENCOMENDAR EQUIPAMENTO", 100, 40);
			//Button orderButton = new Button { BackgroundColor = Color.Transparent, VerticalOptions = LayoutOptions.Center, HorizontalOptions= LayoutOptions.Center, FontSize = 20, TextColor = Color.White};
			orderButton.button.Clicked += OnOrderButtonClicked;

			relativeLayout.Children.Add(orderButton,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(140 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 50 * App.screenHeightAdapter; // 
				}));


			/*Label orderdescLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.itemTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			orderdescLabel.Text = "Ao solicitar este equipamento iremos efetuar uma validação do Stock disponível e o responsável da sua Escola irá levar-lhe a sua encomenda com a maior brevidade possível.";

			relativeLayout.Children.Add(orderdescLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(200 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 80 * App.screenHeightAdapter; // 
			}));*/


		}


		
		public EquipamentsOrderPageCS(Equipment equipment)
		{
			this.equipment = equipment;

			this.initLayout();
			initSpecificLayout();
		}

		async void OnOrderButtonClicked(object sender, EventArgs e)
		{
			showActivityIndicator();
			Debug.WriteLine("OnOrderButtonClicked");
			EquipmentManager equipmentManager = new EquipmentManager();

			var result = await equipmentManager.CreateEquipmentOrder(App.member.id, App.member.name, equipment.id, equipment.type + " - " + equipment.subtype + " - " + equipment.name);
			Debug.Print("result = " + result);
			if ((result == "-1") | (result == "-2"))
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
			}
            hideActivityIndicator();

            EquipmentOrder equipmentOrder = await equipmentManager.GetEquipment_Order_byID(result);

            await Navigation.PushAsync(new EquipamentOrderPaymentPageCS(equipmentOrder));

            //await DisplayAlert("EQUIPAMENTO SOLICITADO", "A sua encomenda foi realizada com sucesso. Fale com o seu treinador para saber quando conseguirá entregar a mesma.", "OK");

		}
	}
}
