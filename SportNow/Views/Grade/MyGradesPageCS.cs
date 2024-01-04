using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.Views.Profile;

namespace SportNow.Views
{
	public class myGradesPageCS : DefaultPage
	{

		protected override void OnDisappearing() {
			collectionViewExaminations.SelectedItem = null;
		}




		private CollectionView collectionViewExaminations;

		//private Member member;

		StackLayout stackButtons;
		StackLayout stackProgramasExameButtons;

		MenuButton programasExameButton, minhasGraduacoesButton;

		OptionButton under6OptionButton, under12OptionButton, over12OptionButton;

		

		public void initLayout()
		{
			Title = "GRADUAÇÕES";

			/*var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "perfil.png"

			};
			toolbarItem.Clicked += OnPerfilButtonClicked;
			ToolbarItems.Add(toolbarItem);*/

		}

		public async void initSpecificLayout(string type)
		{
			//member = App.member;

			var result = await GetExaminations(App.member);

			CreateStackButtons(type);
			CreateMinhasGraduacoesColletion();
			CreateProgramasExame();
			//CreateParticipacoesEventosColletion();

			if (type == "ProgramasExame")
			{
				OnProgramasExameButtonClicked(null, null);

			}
			else if (type == "MinhasGraduaçoes")
			{
				OnMinhasGraduacoesButtonClicked(null, null);

			}
		}

		public void CreateStackButtons(string type)
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width - 50) / 2;


			minhasGraduacoesButton = new MenuButton("MINHAS GRADUAÇÕES", buttonWidth, 60);
			minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;

			programasExameButton = new MenuButton("PROGRAMAS EXAME", buttonWidth, 60);
			programasExameButton.button.Clicked += OnProgramasExameButtonClicked;


			stackButtons = new StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 5 * App.screenHeightAdapter,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 60 * App.screenHeightAdapter,
				Children =
				{
					minhasGraduacoesButton,
					programasExameButton,
				}
			};

			relativeLayout.Children.Add(stackButtons,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter));

		}

		public async void CreateMinhasGraduacoesColletion()
		{

			//COLLECTION GRADUACOES

			var vsg = new VisualStateGroup();
			var vs = new VisualState {
				Name = "Selected"
			};

			collectionViewExaminations = new CollectionView {
				SelectionMode = SelectionMode.Single,
				ItemsSource = Constants.belts, //member.examinations,
				ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não tem exames registados.", HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.White, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			collectionViewExaminations.SelectionChanged += OnCollectionViewSelectionChanged;



			List<Belt> member_belts = Constants.belts;
			bool isNextGradeLocked= false;

			Debug.WriteLine("member.grade = " + App.member.grade + " "+isNextGradeLocked);
			if (App.member.grade == "10_Kyu") {
				
				isNextGradeLocked = true;
			}

			foreach (Belt member_belt in member_belts) {
				//Debug.WriteLine("member_belt = "+ member_belt.gradecode);
				foreach (Examination member_examination in App.member.examinations)
				{
					
					if (member_belt.gradecode == member_examination.grade) {
						member_belt.hasgrade = true;
					}
				}

				//Debug.WriteLine("member_belt.grade = " + member_belt.gra + " " + isNextGradeLocked);

				if (isNextGradeLocked == true)
				{
					member_belt.image = "belt_" + member_belt.gradecode + "_bloq.png";
				}
				else
				{
					member_belt.image = "belt_" + member_belt.gradecode + ".png";
				}

                if (member_belt.gradecode == App.member.grade)
				{
					Debug.WriteLine("member_belt.gradecode == member.grade poe a true " + member_belt.gradecode);
					isNextGradeLocked = true;
				}
			}

			collectionViewExaminations.ItemTemplate = new DataTemplate(() =>
			{

				Grid grid = new Grid { Padding = 10 };
				grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				//grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto 

				Image image = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				image.SetBinding(Image.SourceProperty, "image");

				Label gradeLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = Color.White, LineBreakMode = LineBreakMode.NoWrap };
				gradeLabel.SetBinding(Label.TextProperty, "grade");

				//Label locationLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White, LineBreakMode = LineBreakMode.NoWrap};
				//locationLabel.SetBinding(Label.TextProperty, "hasgrade");

				//vs.Setters.Add(new Setter { Property = grid.BackgroundColor, Value = Color.Red });

				grid.Children.Add(image, 0, 0);
				grid.Children.Add(gradeLabel, 0, 1);
				//grid.Children.Add(locationLabel, 0, 2);

				return grid;
			});

			

			relativeLayout.Children.Add(collectionViewExaminations,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(80 * App.screenHeightAdapter),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width); // center of image (which is 40 wide)
			}),
			heightConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height - (80 * App.screenHeightAdapter)); 
			}));


		}

		public void CreateProgramasExame()
        {
			var width = Constants.ScreenWidth;
			var buttonWidth = (width) / 3;

			under6OptionButton = new OptionButton("-6 ANOS", "fotomenos6anos.png", buttonWidth, 60);
			//minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
			var under6OptionButton_tap = new TapGestureRecognizer();
			under6OptionButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new GradeProgramPageCS("under6"));
			};
			under6OptionButton.GestureRecognizers.Add(under6OptionButton_tap);

			under12OptionButton = new OptionButton("-12 ANOS", "fotomenos12anos.png", buttonWidth, 60);
			var under12OptionButton_tap = new TapGestureRecognizer();
			under12OptionButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new GradeProgramPageCS("under12"));
			};
			under12OptionButton.GestureRecognizers.Add(under12OptionButton_tap);

			over12OptionButton = new OptionButton("+12 ANOS", "fotomais12anos.png", buttonWidth, 60);
			//minhasGraduacoesButton.button.Clicked += OnMinhasGraduacoesButtonClicked;
			var over12OptionButton_tap = new TapGestureRecognizer();
			over12OptionButton_tap.Tapped += (s, e) =>
			{
				Navigation.PushAsync(new GradeProgramPageCS("over12"));
			};
			over12OptionButton.GestureRecognizers.Add(over12OptionButton_tap);

			
			stackProgramasExameButtons = new StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 50 * App.screenHeightAdapter,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 280 * App.screenHeightAdapter,
				Children =
				{
					under6OptionButton,
					under12OptionButton,
					over12OptionButton
				}
			};

			relativeLayout.Children.Add(stackProgramasExameButtons,
			xConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width / 4);
			}),
			yConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width / 2);
			}),
			heightConstraint: Constraint.Constant(430 * App.screenHeightAdapter));
		}

		public myGradesPageCS(string type)
		{
			NavigationPage.SetBackButtonTitle(this, "");
			this.initLayout();
			this.initSpecificLayout(type);

			//Parent.

		}

		async void OnPerfilButtonClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ProfilePageCS());
		}


		async Task<int> GetExaminations(Member member)
		{
			Debug.WriteLine("GetExaminations");
			MemberManager memberManager = new MemberManager();

			var result = await memberManager.GetExaminations(member);
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

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewSelectionChanged member.examinations.Count " + App.member.examinations.Count);


			if ((sender as CollectionView).SelectedItem != null) { 

				Belt belt = (sender as CollectionView).SelectedItem as Belt;

				Debug.WriteLine("OnCollectionViewSelectionChanged belt.gradecode " + belt.gradecode);

				foreach (Examination examination in App.member.examinations)
				{
					if (belt.gradecode == examination.grade)
					{
						await Navigation.PushAsync(new DetalheGraduacaoPageCS(App.member, examination));
					}
				}

				//Debug.WriteLine("OnCollectionViewSelectionChanged examination = " + examination.grade);

				//await Navigation.PushAsync(new DetalheGraduacaoPageCS(member, examination));
				/*Navigation.InsertPageBefore(new DetalheGraduacaoPageCS(examination), this);
				await Navigation.PopAsync();*/

				//(sender as CollectionView).SelectedItem = null;
			}
		}

		async void OnMinhasGraduacoesButtonClicked(object sender, EventArgs e)
		{

			programasExameButton.deactivate();
			minhasGraduacoesButton.activate();

			relativeLayout.Children.Add(collectionViewExaminations,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(80 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height - (80 * App.screenHeightAdapter)); // 
			}));


			relativeLayout.Children.Remove(stackProgramasExameButtons);

			//collectionViewProximasCompeticoes.IsVisible = false;
			//collectionViewResultadosCompeticoes.IsVisible = true;
			/*			gridGeral.IsVisible = true;
						gridIdentificacao.IsVisible = false;
						gridMorada.IsVisible = false;
						gridEncEducacao.IsVisible = false;*/

		}

		async void OnProgramasExameButtonClicked(object sender, EventArgs e)
		{

			programasExameButton.activate();
			minhasGraduacoesButton.deactivate();

			relativeLayout.Children.Add(stackProgramasExameButtons,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 4);
				}),
				yConstraint: Constraint.Constant(80 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 2);
				}),
				heightConstraint: Constraint.Constant(430));

			relativeLayout.Children.Remove(collectionViewExaminations);

		}

	}

}

