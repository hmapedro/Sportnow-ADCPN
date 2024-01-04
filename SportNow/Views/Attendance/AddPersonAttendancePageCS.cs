using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Collections.ObjectModel;

namespace SportNow.Views
{
	public class AddPersonAttendancePageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		private StackLayout stackButtons;

		private CollectionView collectionViewMembers, collectionViewStudents;

		Class_Schedule class_Schedule;
		List<Member> students;

		//private List<Member> members;

		public void initLayout()
		{
			Title = "ADICIONAR ALUNO";
		}


		public void CleanScreen()
		{
			Debug.Print("AddPersonAttendancePageCS.CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			/*if (collectionViewMembers != null)
            {
				relativeLayout.Children.Remove(collectionViewMembers);
				collectionViewMembers = null;
			}*/

		}

		public async void initSpecificLayout()
		{

			showActivityIndicator();

			Label titleLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			titleLabel.Text = "Escolha o aluno para o qual pretende adicionar uma presença:";


			relativeLayout.Children.Add(titleLabel,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter));


			CreateClassPicker();

			students = await GetStudentsClass("");

			

			CreateStudentsColletion();

            hideActivityIndicator();
        }

		public async void CreateClassPicker()
		{
			List<string> classList = new List<string>();
            classList.Add("Todas as Classes");
            List<Class_Detail> classes= await GetAllClasses(); 
			int selectedIndex = 0;
			int selectedIndex_temp = 0;

			foreach (Class_Detail class_detail in classes)
			{
                classList.Add(class_detail.name);
				/*if (class_detail.name == class_Schedule.name)
                {
					selectedIndex = selectedIndex_temp;
				}
				selectedIndex_temp++;*/
			}

			
			Debug.Print("selectedIndex = "+ selectedIndex);

			var classPicker = new Picker
			{
				Title = "",
				TitleColor = App.topColor,
				BackgroundColor = Color.Transparent,
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.titleFontSize

			};
            classPicker.ItemsSource = classList;
            classPicker.SelectedIndex = selectedIndex;

            classPicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{

				showActivityIndicator();

				Debug.Print("ClassPicker selectedItem = " + classPicker.SelectedItem.ToString());
				students = await GetStudentsClass(GetClassId(classes, classPicker.SelectedItem.ToString()));
				relativeLayout.Children.Remove(collectionViewStudents);
				collectionViewStudents = null;
				CreateStudentsColletion();

                hideActivityIndicator();

            };

			relativeLayout.Children.Add(classPicker,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(40 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(50));
		}

		public string GetClassId(List<Class_Detail> classes, string className)
		{
            foreach (Class_Detail class_detail in classes)
            {
                if (class_detail.name == className)
                {
					return class_detail.id;
                }
            }
            return "";
		}

		public void CreateStudentsColletion()
		{
			Debug.Print("AddPersonAttendancePageCS.CreateStudentsColletion");
			//COLLECTION GRADUACOES
			collectionViewStudents = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = students,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 10, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não tem sócios associados a esta classe.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			collectionViewStudents.SelectionChanged += OnCollectionViewStudentsSelectionChanged;

			collectionViewStudents.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					HeightRequest = 30 * App.screenHeightAdapter
				};

				FormValue numberLabel = new FormValue("");
				numberLabel.label.SetBinding(Label.TextProperty, "number_member");


				itemRelativeLayout.Children.Add(numberLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(50 * App.screenWidthAdapter),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

				FormValue nicknameLabel = new FormValue("");
				nicknameLabel.label.SetBinding(Label.TextProperty, "nickname");


				itemRelativeLayout.Children.Add(nicknameLabel,
					xConstraint: Constraint.Constant(55 * App.screenWidthAdapter),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width - (55 * App.screenWidthAdapter)) / 2) - (5 * App.screenWidthAdapter);
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

				FormValue dojoLabel = new FormValue("");
				dojoLabel.label.SetBinding(Label.TextProperty, "aulanome");

				itemRelativeLayout.Children.Add(dojoLabel,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (((parent.Width) - ((parent.Width - (55 * App.screenWidthAdapter)) / 2)));
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width - (55 * App.screenWidthAdapter)) / 2) - (5 * App.screenWidthAdapter);
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

				return itemRelativeLayout;
			});

			relativeLayout.Children.Add(collectionViewStudents,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(90 * App.screenHeightAdapter),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width); // center of image (which is 40 wide)
			}),
			heightConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height- (90 * App.screenHeightAdapter)); // 
			}));

		}

		public AddPersonAttendancePageCS(Class_Schedule class_Schedule)
		{
			Debug.WriteLine("AddPersonAttendancePageCS");
			this.class_Schedule = class_Schedule;
			this.initLayout();
			//this.initSpecificLayout();

		}


		async Task<List<Member>> GetStudentsClass(string classid)
		{
			MemberManager memberManager = new MemberManager();
			List<Member> students = await memberManager.GetStudentsClass(App.original_member.id, classid);

			return students;
		}

		async Task<List<Class_Detail>> GetAllClasses()
		{
			ClassManager classManager = new ClassManager();
			List<Class_Detail> classes = await classManager.GetAllClasses();

			return classes;
		}

		async void OnCollectionViewStudentsSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("AddPersonAttendancePageCS.OnCollectionViewMembersSelectionChanged");
            ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = true, Color = Color.Black, IsEnabled = true, IsVisible = true, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };

			showActivityIndicator();
            
            if ((sender as CollectionView).SelectedItem != null)
			{

				Member member = (sender as CollectionView).SelectedItem as Member;

				ClassManager classmanager = new ClassManager();	
				string class_attendance_id = await classmanager.CreateClass_Attendance(member.id, class_Schedule.classid, "confirmada", class_Schedule.date);
				Debug.Print("class_attendance_id=" + class_attendance_id);

				await Navigation.PopAsync();

				/*Navigation.InsertPageBefore(new MainTabbedPageCS("", ""), this);
				await Navigation.PopToRootAsync();*/

				//await Navigation.PopAsync();
			}
            hideActivityIndicator();
        }
	}
}
