using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
//Ausing Acr.UserDialogs;
using Xamarin.CommunityToolkit;
using Xamarin.CommunityToolkit.UI.Views;
using SportNow.ViewModel;
using Xamarin.Essentials;

namespace SportNow.Views
{
	public class GradeProgramPageCS : DefaultPage
	{

		protected override void OnDisappearing() {
			//collectionViewExaminations.SelectedItem = null;
		}





		List<Examination_Program> programasExameClean, programasExameAll, selectedProgramasExame, programasExameUnder6, programasExameUnder12, programasExameOver12;

		private CollectionView programasExameCollectionView;

		private Member member;

		StackLayout stackButtons;

		MenuButton under6Button, under12Button, over12Button;


		public void initLayout()
		{
			Title = "GRADUAÇÕES";
		}

		public void CleanProgramasExameCollectionView()
		{
			Debug.Print("CleanProgramasExameollectionView");
			//valida se os objetos já foram criados antes de os remover
			if (programasExameCollectionView != null)
			{
				relativeLayout.Children.Remove(programasExameCollectionView);
				programasExameCollectionView = null;
			}

		}

		public async void initSpecificLayout(string type)
		{
			showActivityIndicator();
			programasExameAll  = await GetExaminationProgramAll();
			createProgramasExameLists();
			CreateStackButtons(type);

			if (type == "under6")
			{
				OnUnder6ButtonClicked(null, null);
			}
			else if (type == "under12")
			{
				OnUnder12ButtonClicked(null, null);
			}
			else if (type == "over12")
			{
				OnOver12ButtonClicked(null, null);
			}
            hideActivityIndicator();
        }

		public void createProgramasExameLists()
		{

			List<Belt> member_belts = Constants.belts;
			//bool isNextGradeLocked= false;

			programasExameClean = new List<Examination_Program>();
			programasExameUnder6 = new List<Examination_Program>();
			programasExameUnder12 = new List<Examination_Program>();
			programasExameOver12 = new List<Examination_Program>();
			
			foreach (Examination_Program examination_program in programasExameAll)
			{
				examination_program.examinationTo_string = "Exame para " + Constants.grades[examination_program.grade];

				foreach (Belt belt in member_belts)
				{
					if (belt.gradecode == examination_program.grade)
					{
						examination_program.image = "belt_" + belt.gradecode + ".png";
					}
				}

				if (examination_program.type == "menos6")
                {
					programasExameUnder6.Add(examination_program);
                }
				else if (examination_program.type == "menos12")
				{
					programasExameUnder12.Add(examination_program);
				}
				else if (examination_program.type == "mais12")
				{
					programasExameOver12.Add(examination_program);
				}
			}
		}

		public void CreateStackButtons(string type)
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width-50) / 3;

			
			under6Button = new MenuButton("-6 ANOS", buttonWidth, 60);
			under6Button.button.Clicked += OnUnder6ButtonClicked;

			under12Button = new MenuButton("-12 ANOS", buttonWidth, 60);
			under12Button.button.Clicked += OnUnder12ButtonClicked;

			over12Button = new MenuButton("+12 ANOS", buttonWidth, 60);
			over12Button.button.Clicked += OnOver12ButtonClicked;

			stackButtons = new StackLayout
			{
				//WidthRequest = 370,
				Margin = new Thickness(0),
				Spacing = 5,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 40,
				Children =
				{
					under6Button,
					under12Button,
					over12Button
				}
			};

			relativeLayout.Children.Add(stackButtons,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(40));
		}


		public async void CreateProgramasExameColletion()
		{

			//COLLECTION PROGRAMAS EXAME

			Debug.Print("selectedProgramasExame Count " + selectedProgramasExame.Count);
			programasExameCollectionView = new CollectionView {
				SelectionMode = SelectionMode.Single,
				ItemsSource = selectedProgramasExame,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem programas de exame.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White, FontSize = 20 },
							}
					}
				}
			};
			//ItemsUpdatingScrollMode = ItemsUpdatingScrollMode.KeepItemsInView,



			programasExameCollectionView.SelectionChanged += OProgramasExameCollectionViewSelectionChanged;

			DataTemplate expandedTemplate = new DataTemplate(() =>
			{
				Grid grid = new Grid { Padding = 10 };
				grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

				Image image = new Image { Aspect = Aspect.AspectFit, HeightRequest = 24, WidthRequest = 60 };
				image.SetBinding(Image.SourceProperty, "image");

				Label gradeLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.White, TextDecorations = TextDecorations.Underline, LineBreakMode = LineBreakMode.NoWrap, FontSize = App.titleFontSize };
				gradeLabel.SetBinding(Label.TextProperty, "examinationTo_string");

				Image youtubeImage = new Image { Aspect = Aspect.AspectFit, HeightRequest = 24, WidthRequest = 60, Source = "youtube.png" };
				youtubeImage.SetBinding(Image.AutomationIdProperty, "video");

				var youtubeImage_tap = new TapGestureRecognizer();
				youtubeImage_tap.Tapped += async (s, e) =>
				{
					try
					{
						await Browser.OpenAsync(((Image)s).AutomationId, BrowserLaunchMode.SystemPreferred);
					}
					catch (Exception ex)
					{
					}
				};
				youtubeImage.GestureRecognizers.Add(youtubeImage_tap);

				grid.Children.Add(image, 0, 0);
				grid.Children.Add(gradeLabel, 1, 0);
				grid.Children.Add(youtubeImage, 2, 0);

				Grid gridDetail = new Grid { Padding = 10 };
				gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				gridDetail.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				gridDetail.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

				Label kihonHeaderLabel, kihonLabel, kataHeaderLabel, kataLabel, kumiteHeaderLabel, kumiteLabel, shiaikumiteHeaderLabel, shiaikumiteLabel, youtubeLabel;

				kihonHeaderLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.FromRgb(246, 220, 178), LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
				kihonHeaderLabel.Text = "KIHON";
				gridDetail.Children.Add(kihonHeaderLabel, 0, 1);

				kihonLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
				kihonLabel.SetBinding(Label.TextProperty, "kihonText");
				gridDetail.Children.Add(kihonLabel, 0, 2);

				kataHeaderLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.FromRgb(246, 220, 178), LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
				kataHeaderLabel.Text = "KATA";
				gridDetail.Children.Add(kataHeaderLabel, 0, 3);
				Grid.SetColumnSpan(kataHeaderLabel, 3);

				kataLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
				kataLabel.SetBinding(Label.TextProperty, "kataText");
				gridDetail.Children.Add(kataLabel, 0, 4);
				Grid.SetColumnSpan(kataLabel, 3);

				kumiteHeaderLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.FromRgb(246, 220, 178), LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
				kumiteHeaderLabel.Text = "KUMITE";
				gridDetail.Children.Add(kumiteHeaderLabel, 0, 5);
				Grid.SetColumnSpan(kumiteHeaderLabel, 3);

				kumiteLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
				kumiteLabel.SetBinding(Label.TextProperty, "kumiteText");
				gridDetail.Children.Add(kumiteLabel, 0, 6);
				Grid.SetColumnSpan(kumiteLabel, 3);


				shiaikumiteHeaderLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.FromRgb(246, 220, 178), LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
				shiaikumiteHeaderLabel.Text = "SHIAI KUMITE";
				gridDetail.Children.Add(shiaikumiteHeaderLabel, 0, 7);
				Grid.SetColumnSpan(shiaikumiteHeaderLabel, 3);

				shiaikumiteLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap, FontSize = App.itemTitleFontSize };
				shiaikumiteLabel.SetBinding(Label.TextProperty, "shiaikumiteText");
				gridDetail.Children.Add(shiaikumiteLabel, 0, 8);
				Grid.SetColumnSpan(shiaikumiteLabel, 3);

				grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				grid.Children.Add(gridDetail, 0, 1);
				Grid.SetColumnSpan(gridDetail, 3);

				/*bool isExpanded = false;

				var gradeLabel_tap = new TapGestureRecognizer();
				gradeLabel_tap.Tapped += async (s, e) =>
				{

					//Debug.Print("isExpandedLabel.Text = " + isExpandedLabel.Text);
					if (isExpanded == false)
					//if (isExpandedLabel.Text == "false")
					{
						//isExpandedLabel.Text = "true";
					}
					else
					{
						grid.Children.RemoveAt(2);
						grid.RowDefinitions.RemoveAt(1);
						//isExpandedLabel.Text = "false";
					}
					isExpanded = !isExpanded;

					
				};
				gradeLabel.GestureRecognizers.Add(gradeLabel_tap);*/


				return grid;
			});


			DataTemplate notExpandedTemplate = new DataTemplate(() =>
			{
				Grid grid = new Grid { Padding = 10 };
				grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

				Image image = new Image { Aspect = Aspect.AspectFit, HeightRequest = 24, WidthRequest = 60 };
				image.SetBinding(Image.SourceProperty, "image");

				Label gradeLabel = new Label { HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.White, LineBreakMode = LineBreakMode.NoWrap, FontSize = App.titleFontSize };
				gradeLabel.SetBinding(Label.TextProperty, "examinationTo_string");

				grid.Children.Add(image, 0, 0);
				grid.Children.Add(gradeLabel, 1, 0);

				return grid;
			});

			programasExameCollectionView.ItemTemplate = new Examination_ProgramDataTemplateSelector
			{
				ExpandedTemplate = expandedTemplate,
				NotExpandedTemplate = notExpandedTemplate
			};

			relativeLayout.Children.Add(programasExameCollectionView,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(60),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height-60);
			}));


		}

		public GradeProgramPageCS(string type)
		{
			NavigationPage.SetBackButtonTitle(this, "");
			this.initLayout();
			this.initSpecificLayout(type);

			//Parent.

		}

       /* async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnCollectionViewSelectionChanged member.examinations.Count " + member.examinations.Count);


			if ((sender as CollectionView).SelectedItem != null) { 

				Belt belt = (sender as CollectionView).SelectedItem as Belt;

				Debug.WriteLine("OnCollectionViewSelectionChanged belt.gradecode " + belt.gradecode);

				foreach (Examination examination in member.examinations)
				{
					if (belt.gradecode == examination.grade)
					{
						await Navigation.PushAsync(new DetalheGraduacaoPageCS(member, examination));
					}
				}

				//Debug.WriteLine("OnCollectionViewSelectionChanged examination = " + examination.grade);

				//await Navigation.PushAsync(new DetalheGraduacaoPageCS(member, examination));
				/*Navigation.InsertPageBefore(new DetalheGraduacaoPageCS(examination), this);
				await Navigation.PopAsync();*/

				//(sender as CollectionView).SelectedItem = null;
			/*}
		}
		*/

		 void OnUnder6ButtonClicked(object sender, EventArgs e)
		{
			under6Button.activate();
			under12Button.deactivate();
			over12Button.deactivate();

			selectedProgramasExame = programasExameUnder6;
			CleanProgramasExameCollectionView();
			CreateProgramasExameColletion();
		}

		void OnUnder12ButtonClicked(object sender, EventArgs e)
		{
			under6Button.deactivate();
			under12Button.activate();
			over12Button.deactivate();

			selectedProgramasExame = programasExameUnder12;
			CleanProgramasExameCollectionView();
			CreateProgramasExameColletion();

		}

		void OnOver12ButtonClicked(object sender, EventArgs e)
		{
			under6Button.deactivate();
			under12Button.deactivate();
			over12Button.activate();

			selectedProgramasExame = programasExameOver12;
			CleanProgramasExameCollectionView();
			CreateProgramasExameColletion();
		}

		async Task<List<Examination_Program>> GetExaminationProgramAll()
		{
			Debug.WriteLine("GetProgramasExameAll");
			ExaminationManager examinationManager = new ExaminationManager();

			List<Examination_Program> programasExameAll = await examinationManager.GetExaminationProgramAll();
			if (programasExameAll == null)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return programasExameAll;
		}

		async Task<List<Examination_Technique>> GetExaminationProgram_Techniques(string examination_programid)
		{
			Debug.WriteLine("GetExaminationProgram_Techniques");
			ExaminationManager examinationManager = new ExaminationManager();

			List<Examination_Technique> examination_techniques = await examinationManager.GetExaminationProgram_Techniques(examination_programid);
			if (examination_techniques == null)
			{
								Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return examination_techniques;
		}


		async void OProgramasExameCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("OnProgramasExameCollectionViewSelectionChanged ");

			if ((sender as CollectionView).SelectedItem != null)
			{
				
				 
				Examination_Program examination_program = (sender as CollectionView).SelectedItem as Examination_Program;
				Debug.Print("SELECTED examination_program " + examination_program.name);

				if (examination_program.isExpanded == true)
				{
					examination_program.isExpanded = false;
				}
				else
				{
					examination_program.isExpanded = true;
				}

				//programasExameCollectionView.ItemsSource = programasExameClean;
				//programasExameCollectionView.ItemsSource = selectedProgramasExame;
				CleanProgramasExameCollectionView();
				CreateProgramasExameColletion();
				programasExameCollectionView.ScrollTo(examination_program, null, position: ScrollToPosition.End);
				/*for (int i = 0; i< selectedProgramasExame.Count; i++)
				{
					if (selectedProgramasExame[i].id == examination_program.id)
					{
						Debug.Print("OLAAAA");
						programasExameCollectionView.ScrollTo();
					}
				}*/

			}
			
			
		}


	}

	

}

