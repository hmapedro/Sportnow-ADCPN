using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using SportNow.ViewModel;
using System.Collections.ObjectModel;

namespace SportNow.Views
{
	public class ExaminationEvaluationConfirmPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
		}

		private CollectionView collectionViewExaminationSessionCall;

		ObservableCollection<Examination> examinations;
		Examination_Session examination_session;
		private ExaminationCollection examinationCollection;

		Label examinationMemberNameLabel;
		Label examinationGradeLabel;
		Label examinationTypeLabel;

		int currentExaminationIndex = 0;

		public void initLayout()
		{
			Title = "CONFIRMAÇÃO EXAMES";
		}


		public async void initSpecificLayout()
		{
			cleanExaminations();

			examinationCollection = new ExaminationCollection();
			examinationCollection.Items = examinations;

			createExaminationsEvaluation();
		}

		public void cleanExaminations()
		{

			ObservableCollection<Examination> examinations_new = new ObservableCollection<Examination>();
			foreach (Examination examination_i in examinations)
			{
				if (examination_i.selected == true)
				{
					examinations_new.Add(examination_i);
				}
			}
			this.examinations = examinations_new;
		}

		public async void createExaminationsEvaluation()
		{

			collectionViewExaminationSessionCall = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				//ItemsSource = examination_sessionCall,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical),
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
			{
				new Label { Text = "Ainda não foi criada convocatória para esta Sessão de Exames.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.Red, FontSize = 20 },
			}
					}
				}
			};

			this.BindingContext = examinationCollection;
			collectionViewExaminationSessionCall.SetBinding(ItemsView.ItemsSourceProperty, "Items");

			//collectionViewExaminationSessionCall.SelectionChanged += OncollectionViewExaminationSessionCallSelectionChanged;

			collectionViewExaminationSessionCall.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					Margin = new Thickness(3),
					HeightRequest = 80
				};

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 15, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "membername");
				//nameLabel.SetBinding(Label.TextColorProperty, "selectedColor");

				Frame nameFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Color.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0)
				};
				nameFrame.Content = nameLabel;

				itemRelativeLayout.Children.Add(nameFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 40; // 
					}));

				Label gradeTypeLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 13, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				gradeTypeLabel.SetBinding(Label.TextProperty, "typeLabel");
				//gradeTypeLabel.SetBinding(Label.TextColorProperty, "selectedColor");

				Frame typeFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Color.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0)
				};
				typeFrame.Content = gradeTypeLabel;

				itemRelativeLayout.Children.Add(typeFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(45),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 2); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 40; // 
					}));

				Label gradeLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = 13, TextColor = Color.White, LineBreakMode = LineBreakMode.WordWrap };
				gradeTypeLabel.SetBinding(Label.TextProperty, "gradeLabel");
				//gradeTypeLabel.SetBinding(Label.TextColorProperty, "selectedColor");

				Frame gradeFrame = new Frame
				{
					BorderColor = Color.FromRgb(246, 220, 178),
					BackgroundColor = Color.Transparent,
					CornerRadius = 10,
					IsClippedToBounds = true,
					Padding = new Thickness(5, 0, 0, 0)
				};
				gradeFrame.Content = gradeLabel;

				itemRelativeLayout.Children.Add(gradeFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(45),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 2); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return 40; // 
					}));

				return itemRelativeLayout;
			});

			relativeLayout.Children.Add(collectionViewExaminationSessionCall,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(100),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 170; // 
				})
			);
			/*

			examinationMemberNameLabel = new Label
			{
				Text = examinations[currentExaminationIndex].membername,
				BackgroundColor = Color.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 25,
				TextColor = Color.White
			};

			relativeLayout.Children.Add(examinationMemberNameLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 60; // 
				})
			);


			var grade_typePicker = new Picker
			{
				Title = "",
				TitleColor = Color.White,
				BackgroundColor = Color.Transparent,
				TextColor = Color.FromRgb(246, 220, 178),
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 20

			};

			var grade_typeList = new List<string>();


			foreach (var data in Constants.grade_type)
			{
				grade_typeList.Add(data.Value);
			}

			grade_typePicker.ItemsSource = grade_typeList;
			if (examinations[currentExaminationIndex].memberage <= 6)
            {
				grade_typePicker.SelectedIndex = 0;
			}
			else if (examinations[currentExaminationIndex].memberage <= 12)
			{
				grade_typePicker.SelectedIndex = 1;
			}
			else
			{
				grade_typePicker.SelectedIndex = 2;
			}

			grade_typePicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{
				Debug.Print("grade_typePicker.SelectedItem.ToString() = " + grade_typePicker.SelectedItem.ToString());

			};


			/*examinationGradeLabel = new Label
			{
				Text = examinations[currentExaminationIndex].gradeLabel,
				BackgroundColor = Color.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 25,
				TextColor = Color.White
			};*/

			/*relativeLayout.Children.Add(grade_typePicker,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(60),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width/2); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 60; // 
				})
			);

			examinationTypeLabel = new Label
			{
				Text = examinations[currentExaminationIndex].grade,
				BackgroundColor = Color.Transparent,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 25,
				TextColor = Color.White
			};

			relativeLayout.Children.Add(examinationTypeLabel,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 2); // center of image (which is 40 wide)
				}),
				yConstraint: Constraint.Constant(60),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 2); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return 60; // 
				})
			);*/

		}
		

		public ExaminationEvaluationConfirmPageCS(Examination_Session examination_session, ObservableCollection<Examination> examinations)
		{
			this.BackgroundColor = Color.FromRgb(25, 25, 25);
			this.examinations = examinations;
			this.examination_session = examination_session;
			this.initLayout();
			//this.initSpecificLayout();

		}

	}
}
