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
	public class SelectMemberPageCS : DefaultPage
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

		private CollectionView collectionViewMembers;

		//private List<Member> members;

		public void initLayout()
		{
			Debug.Print("SelectMemberPageCS.initLayout");
			Title = "ESCOLHER UTILIZADOR";
			

			relativeLayout = new RelativeLayout
			{
				Margin = new Thickness(10)
			};
			Content = relativeLayout;

			//NavigationPage.SetHasNavigationBar(this, false);
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
			App.AdaptScreen();

			Debug.Print("SelectMemberPageCS.initSpecificLayout App.titleFontSize = " + App.titleFontSize);

			Label titleLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
			titleLabel.Text = "O seu email tem vários sócios associados.\n Escolha o sócio que pretende utilizar:";


			relativeLayout.Children.Add(titleLabel,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(30),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width); // center of image (which is 40 wide)
			}),
			heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter));

			CreateMembersColletion();
		}

		public void CreateMembersColletion()
		{

			Debug.Print("SelectMemberPageCS.CreateMembersColletion");
			//COLLECTION GRADUACOES
			collectionViewMembers = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = App.members,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 10, HorizontalItemSpacing = 5,  },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não tens Sócios associados.", HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.White, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			collectionViewMembers.SelectionChanged += OnCollectionViewMembersSelectionChanged;

			collectionViewMembers.ItemTemplate = new DataTemplate(() =>
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
						return ((parent.Width-(55 * App.screenWidthAdapter)) /2)-(5 * App.screenWidthAdapter);
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

			relativeLayout.Children.Add(collectionViewMembers,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height - 80 * App.screenHeightAdapter);
			}));

		}


		public SelectMemberPageCS()
		{
			Debug.WriteLine("SelectMemberPageCS");
			this.initLayout();
			//this.initSpecificLayout();

		}

		async void OnCollectionViewMembersSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("SelectMemberPageCS.OnCollectionViewMembersSelectionChanged");

			if ((sender as CollectionView).SelectedItem != null)
			{

				Member member = (sender as CollectionView).SelectedItem as Member;

				App.member = member;
				App.original_member = member;

				saveSelectedUser(member.id);

				if (Navigation.NavigationStack.Count > 1) //veio do profile
				{
					/*Debug.Print("SelectMemberPageCS.OnCollectionViewMembersSelectionChanged AQUIIIII "+ App.original_member.estado);
					if (App.original_member.estado == "activo")
					{
						await Navigation.PopAsync();
					}
					else
					{
						App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
						{
							BarBackgroundColor = Color.White,
							BackgroundColor = Color.White,
							BarTextColor = Color.Black
						};
					}*/
                    App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
                    {
                        BarBackgroundColor = Color.White,
                        BackgroundColor = Color.White,
                        BarTextColor = Color.Black
                    };

                }
				else //veio do login
				{
					Debug.Print("SelectMemberPageCS.OnCollectionViewMembersSelectionChanged AQUIIIII 2");
					App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
					{
						BarBackgroundColor = Color.White,
						BackgroundColor = Color.White,
						BarTextColor = Color.Black
					};
					/*
					Navigation.InsertPageBefore(new MainTabbedPageCS("", ""), this);
					await Navigation.PopToRootAsync();*/
				}
			}
		}

		protected void saveSelectedUser(string memberid)
		{
			Application.Current.Properties.Remove("SELECTEDUSER");
			Application.Current.Properties.Add("SELECTEDUSER", memberid);
			Application.Current.SavePropertiesAsync();
		}
	}
}
