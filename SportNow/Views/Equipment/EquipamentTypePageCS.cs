using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Collections.ObjectModel;
using Syncfusion.SfChart.XForms;
using SportNow.Model.Charts;
using SportNow.Views.Profile;

namespace SportNow.Views
{
	public class EquipamentTypePageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private RelativeLayout equipamentosRelativeLayout;


		private StackLayout stackButtons;

		private OptionButton fatotreinoButton, equipamentotreinoButton;
	

		public void initLayout()
		{
			Title = "EQUIPAMENTOS";



			var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "perfil.png",

			};
			toolbarItem.Clicked += OnPerfilButtonClicked;
			ToolbarItems.Add(toolbarItem);

		}


		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (stackButtons != null)
			{
				relativeLayout.Children.Remove(stackButtons);
				relativeLayout.Children.Remove(equipamentosRelativeLayout);

				stackButtons = null;
				equipamentosRelativeLayout = null;
			}

		}

		public async void initSpecificLayout()
		{
			CreateEquipamentos();
		}


		public void CreateEquipamentos()
		{
			equipamentosRelativeLayout = new RelativeLayout
			{
				Margin = new Thickness(5)
			};

			CreateEquipamentosOptionButtons();

			relativeLayout.Children.Add(equipamentosRelativeLayout,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(20),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 20;
				}));
		}

		public void CreateEquipamentosOptionButtons()
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width) / 2;

			 
			fatotreinoButton = new OptionButton("FATO DE TREINO OFICIAL", "fato_treino_oficial.png", buttonWidth, 60);
			//minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
			var fatotreinoButton_tap = new TapGestureRecognizer();
			fatotreinoButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new EquipamentsPageCS("fato_treino"));
			};
			fatotreinoButton.GestureRecognizers.Add(fatotreinoButton_tap);

			equipamentotreinoButton = new OptionButton("EQUIPAMENTO PARA TREINO", "equipamento_treino.png", buttonWidth, 60);
			var equipamentotreinoButton_tap = new TapGestureRecognizer();
			equipamentotreinoButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new EquipamentsPageCS("equipamento_treino"));
			};
			equipamentotreinoButton.GestureRecognizers.Add(equipamentotreinoButton_tap);


			StackLayout stackEquipamentosButtons = new StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 50,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 350,
				Children =
				{
					fatotreinoButton,
					equipamentotreinoButton,
				}
			};

			equipamentosRelativeLayout.Children.Add(stackEquipamentosButtons,
			xConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width / 4);
			}),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width/2);
			}),
			heightConstraint: Constraint.Constant(400));
		}




		public EquipamentTypePageCS()
		{

			this.initLayout();
			//this.initSpecificLayout();

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfilePageCS());
		}

	}
}
