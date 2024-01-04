using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Collections.ObjectModel;

namespace SportNow.Views.Profile
{
	public class SelectStudentPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		MenuButton proximosEventosButton;
		MenuButton participacoesEventosButton;


		private StackLayout stackButtons;

		private CollectionView collectionViewMembers, collectionViewStudents;

		//private List<Member> members;

		public void initLayout()
		{
			Debug.Print("SelectStudentPageCS.initLayout");
			Title = "ESCOLHER ALUNO";


		}


		public void CleanScreen()
		{
			Debug.Print("SelectMemberPageCS.CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (stackButtons != null)
            {
				relativeLayout.Children.Remove(stackButtons);
				relativeLayout.Children.Remove(collectionViewMembers);

				stackButtons = null;
				collectionViewMembers = null;
			}

		}

		public async void initSpecificLayout()
		{
			Label titleLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
			titleLabel.Text = "Pode também utilizar a aplicação com a conta de um dos seus alunos:";


			relativeLayout.Children.Add(titleLabel,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(0),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter));

			App.member.students = await GetMemberStudents();

			CreateStudentsColletion();

			if (App.original_member.id != App.member.id)
			{
				RoundButton confirmButton = new RoundButton("VOLTAR ORIGINAL", 100, 50);
				confirmButton.button.Clicked += OnVoltarOriginalButtonClicked;

				relativeLayout.Children.Add(confirmButton,
					xConstraint: Constraint.Constant(10),
					yConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height) - 60; // 
				}),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - 20 * App.screenHeightAdapter);
					}),
					heightConstraint: Constraint.Constant(50));
			}


		}

		public void CreateStudentsColletion()
		{
			

			Debug.Print("SelectMemberPageCS.CreateStudentsColletion");
			//COLLECTION GRADUACOES
			collectionViewStudents = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = App.member.students,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 10, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não tem Sócios associados.", HorizontalTextAlignment = TextAlignment.Center, TextColor = App.normalTextColor, FontSize = App.itemTitleFontSize },
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
			yConstraint: Constraint.Constant(50 * App.screenHeightAdapter),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width); // center of image (which is 40 wide)
			}),
			heightConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height- (120 * App.screenHeightAdapter)); // 
			}));

		}

		public SelectStudentPageCS()
		{
			Debug.WriteLine("SelectStudentPageCS");
			this.initLayout();
			//this.initSpecificLayout();

		}


		async Task<List<Member>> GetMemberStudents()
		{
			MemberManager memberManager = new MemberManager();
			List<Member> students = await memberManager.GetMemberStudents(App.original_member.id);

			List<Member> students_without_duplicates = new List<Member>();
			foreach (Member member in students)
            {
				bool already_exists = false;
				foreach (Member member_without_duplicates in students_without_duplicates)
				{
					if (member.id == member_without_duplicates.id)
					{
						already_exists = true;
					}
				}
				if (already_exists == false)
				{
					students_without_duplicates.Add(member);
				}

			}

			return students_without_duplicates;
		}

		async void OnCollectionViewStudentsSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("SelectMemberPageCS.OnCollectionViewMembersSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{

				Member member = (sender as CollectionView).SelectedItem as Member;
				
				App.member = member;
				App.member.students_count = await GetMemberStudents_Count(App.member.id);
                
				App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
				{
					BarBackgroundColor = Color.White,
					BackgroundColor = Color.White, 
					BarTextColor = Color.Black//FromRgb(75, 75, 75)
				};

                /*await Navigation.PopAsync();
                await Navigation.PopAsync();
                await Navigation.PushAsync(new ProfilePageCS());*/
                /*Navigation.InsertPageBefore(new MainTabbedPageCS("", ""), this);
				await Navigation.PopToRootAsync();*/

                //await Navigation.PopAsync();
            }
		}

		async Task<int> GetMemberStudents_Count(string memberid)
		{
			Debug.WriteLine("MainTabbedPageCS.GetMemberStudents_Count");
			MemberManager memberManager = new MemberManager();

			var result = await memberManager.GetMemberStudents_Count(memberid);

			return result;
		}

		async void OnVoltarOriginalButtonClicked(object sender, EventArgs e)
		{
			App.member = App.original_member;


            /*await Navigation.PopAsync();
            await Navigation.PopAsync();
            await Navigation.PushAsync(new ProfilePageCS());*/
            App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
            {
                BarBackgroundColor = Color.White,
                BackgroundColor = Color.White,
                BarTextColor = Color.Black//FromRgb(75, 75, 75)
            };

            //await Navigation.PopAsync();
			//await Navigation.PushAsync(new CompleteRegistration_Payment_PageCS());
		}
	}
}
