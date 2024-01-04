using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using Xamarin.Essentials;
//Ausing Acr.UserDialogs;
using System.Text.RegularExpressions;

namespace SportNow.Views.Profile
{
    public class ApproveRegistrationPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{	
		}

		//Image estadoQuotaImage;


		private CollectionView collectionViewMembers;
		List<Member> members_To_Approve;
		Label titleLabel;

		Member new_member;
		List<Class_Detail> allClasses;

		Frame frameClassPicker;
		Label classPickerTitleLabel;
		Picker classPicker;
		CollectionView classesCollectionView;

		RegisterButton confirmClasseButton;

		public void initLayout()
		{
			Title = "APROVAR INSCRIÇÕES";

			relativeLayout = new RelativeLayout
			{
				Margin = new Thickness(0)
			};
			Content = relativeLayout;

		}


		public async void initSpecificLayout()
		{
			App.AdaptScreen();
			showActivityIndicator();
			members_To_Approve = await GetMembers_To_Approve();

			Debug.Print("SelectMemberPageCS.initSpecificLayout App.titleFontSize = " + App.titleFontSize);

			titleLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = App.topColor, LineBreakMode = LineBreakMode.WordWrap };
			if (members_To_Approve.Count == 0)
			{
				titleLabel.Text = "Não tens novos sócios para aprovar.";
			}
			else
			{
				titleLabel.Text = "Tens os seguintes novos sócios para aprovar:";
			}
			
			relativeLayout.Children.Add(titleLabel,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(10),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width); // center of image (which is 40 wide)
			}),
			heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter));


			

			CreateMembersColletion();
            hideActivityIndicator();
        }

		public void CreateMembersColletion()
		{

			Debug.Print("SelectMemberPageCS.CreateMembersColletion");
			//COLLECTION GRADUACOES
			collectionViewMembers = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = members_To_Approve,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 10, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não tens novos sócios para aprovar.", HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.White, FontSize = App.itemTitleFontSize },
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
					xConstraint: Constraint.Constant(55),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width - (145 * App.screenWidthAdapter))) - (10 * App.screenWidthAdapter);
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

				FormValue idLabel = new FormValue("");
				idLabel.label.SetBinding(Label.TextProperty, "birthdate");

				itemRelativeLayout.Children.Add(idLabel,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (105 * App.screenWidthAdapter));
					}),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

				return itemRelativeLayout;
			});

			relativeLayout.Children.Add(collectionViewMembers,
			xConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
			yConstraint: Constraint.Constant(80 * App.screenHeightAdapter),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width - (20 * App.screenHeightAdapter));
			}),
			heightConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Height - 80 * App.screenHeightAdapter);
			}));
		}

		public async void createClassPicker(string membername)
		{
			ClassManager classManager = new ClassManager();
			showActivityIndicator();
			allClasses = await classManager.GetAllClasses();
			CompleteClass_Detail();
            hideActivityIndicator();

			frameClassPicker = new Frame() {
				CornerRadius = 10,
				IsClippedToBounds = true,
				BackgroundColor = Color.White,//.FromRgb(200, 200, 200),
				HasShadow = false
			};

			relativeLayout.Children.Add(frameClassPicker,
				xConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width) - (0 * App.screenWidthAdapter);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (0 * App.screenWidthAdapter);
				}));

			relativeLayout.LowerChild(collectionViewMembers);

            relativeLayout.RaiseChild(frameClassPicker);

            classPickerTitleLabel = new Label
			{
				Text = "Escolha a Classe a que o novo Sócio " + membername +" vai pertencer",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.titleFontSize,
				TextColor = App.topColor,
				LineBreakMode = LineBreakMode.WordWrap
			};

			relativeLayout.Children.Add(classPickerTitleLabel,
				xConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(110 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width) - (60 * App.screenWidthAdapter);
				}),
				heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter));


			CreateClassesColletion();

		}

		public void CompleteClass_Detail()
		{
			foreach (Class_Detail class_detail in allClasses)
			{
				if (class_detail.imagesource == null)
				{
					class_detail.imagesourceObject = "logo_login" + ".png";
                    Debug.Print("image = " + "logo_login" + ".png");
                }
				else
				{
                    Debug.Print("image = " + Constants.images_URL + class_detail.id + "_imagem_c");
                    class_detail.imagesourceObject = new UriImageSource
					{
						Uri = new Uri(Constants.images_URL + class_detail.id + "_imagem_c"),
						CachingEnabled = true,
						CacheValidity = new TimeSpan(5, 0, 0, 0)
					};
				}
				
			}
		}

		public void CreateClassesColletion()
		{
			//COLLECTION GRADUACOES
			classesCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Single,
				ItemsSource = allClasses,
				ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical) { VerticalItemSpacing = 5, HorizontalItemSpacing = 5, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem Classes.", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White, FontSize = 20 },
							}
					}
				}
			};

			//classesCollectionView.SelectionChanged += OnClassAttendanceCollectionViewSelectionChanged;
			classesCollectionView.SelectionChanged += confirmClassSelectionChanged;

			classesCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					HeightRequest = App.ItemHeight,
					WidthRequest = App.ItemWidth
				};

				Frame itemFrame = new Frame
				{
					CornerRadius = 5 * (float)App.screenWidthAdapter,
					IsClippedToBounds = true,
					BorderColor = Color.Transparent,
					BackgroundColor = Color.Black,
					Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = App.ItemHeight,
					VerticalOptions = LayoutOptions.Center,
				};

				Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.4 }; //, HeightRequest = 60, WidthRequest = 60
				eventoImage.SetBinding(Image.SourceProperty, "imagesourceObject");

				itemFrame.Content = eventoImage;

				itemRelativeLayout.Children.Add(itemFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - 5);
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height);
					}));

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 20 * App.screenWidthAdapter, TextColor = Color.White };
				nameLabel.SetBinding(Label.TextProperty, "name");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(3 * App.screenWidthAdapter),
					yConstraint: Constraint.Constant(25 * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (6 * App.screenWidthAdapter));
					}),
					heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter));

				return itemRelativeLayout;
			});
			relativeLayout.Children.Add(classesCollectionView,
				xConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(180 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width) - (0 * App.screenWidthAdapter);
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (180 * App.screenWidthAdapter);
				}));


		}

		public ApproveRegistrationPageCS()
		{
			this.initLayout();
			this.initSpecificLayout();
		}


		public async Task<List<Member>> GetMembers_To_Approve()
		{
			Debug.WriteLine("GetMembers_To_Approve");

			MemberManager memberManager = new MemberManager();

			List<Member> members;

			members = await memberManager.GetMembers_To_Approve();

			return members;

		}

		async void OnCollectionViewMembersSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Debug.WriteLine("ApproveRegistrationPageCS.OnCollectionViewMembersSelectionChanged");

			new_member = (sender as CollectionView).SelectedItem as Member;

			if (collectionViewMembers.SelectedItem != null)
            {
				var actionSheet = await DisplayActionSheet("Aprovar novo Sócio " + new_member.nickname, "Cancel", null, "Aprovar", "Rejeitar");

                showActivityIndicator();

                string result = "";
				switch (actionSheet)
				{
					case "Cancel":
						break;
					case "Aprovar":
						if (new_member.member_type == "praticante")
						{
                            createClassPicker(new_member.nickname);
                        }
						else
						{
							_ = await Update_Member_Status(this.new_member.id, this.new_member.nickname, this.new_member.email, "aprovado", null);
                        }
						break;
					case "Rejeitar":
						

						MemberManager memberManager = new MemberManager();
						result = await memberManager.Update_Member_Approved_Status(new_member.id, App.member.nickname, new_member.email, "rejeitado", null);
						collectionViewMembers.SelectedItem = null;
						collectionViewMembers.ItemsSource = null;

						members_To_Approve = await GetMembers_To_Approve();

						if (members_To_Approve.Count == 0)
						{
							titleLabel.Text = "Não tens novos sócios para aprovar.";
						}
						else
						{
							titleLabel.Text = "Tens os seguintes novos sócios para aprovar:";
						}

						collectionViewMembers.ItemsSource = members_To_Approve;
                        
                        break;
				}

                hideActivityIndicator();
            }

		}

		async void confirmClassSelectionChanged(object sender, EventArgs e)
		{
			if ((sender as CollectionView).SelectedItem != null)
			{
				showActivityIndicator();
				Class_Detail class_detail = (sender as CollectionView).SelectedItem as Class_Detail;
				classesCollectionView.SelectedItem = null;
				//string classId = getClassID(allClasses, class_detail.id);
				Debug.Print("classId = " + class_detail.id);
				_ = await Update_Member_Status(this.new_member.id, this.new_member.nickname, this.new_member.email, "aprovado", class_detail.id);
                hideActivityIndicator();
            }

			
		}

		public async Task<string> Update_Member_Status(string new_member_id, string new_member_name, string new_member_email, string status, string classId)
		{
			showActivityIndicator();
			MemberManager memberManager = new MemberManager();
			var result = await memberManager.Update_Member_Approved_Status(new_member_id, new_member_name, new_member_email, status, classId);

			await DisplayAlert("Sócio Aprovado", "O Sócio " + this.new_member.nickname + " foi "+status+".", "OK");

			collectionViewMembers.SelectedItem = null;
			collectionViewMembers.ItemsSource = null;

			showActivityIndicator();
            members_To_Approve = await GetMembers_To_Approve();
            if (members_To_Approve.Count == 0)
			{
				titleLabel.Text = "Não tens novos sócios para aprovar.";
			}
			else
			{
				titleLabel.Text = "Tens os seguintes novos sócios para aprovar:";
			}
            collectionViewMembers.ItemsSource = members_To_Approve;
			
			if (frameClassPicker != null)
            {
                relativeLayout.Children.Remove(frameClassPicker);
            }
            if (classPickerTitleLabel != null)
            {
                relativeLayout.Children.Remove(classPickerTitleLabel);
            }
            if (classesCollectionView != null)
            {
                relativeLayout.Children.Remove(classesCollectionView);
            }
            
			//relativeLayout.Children.Remove(confirmClasseButton);
			Debug.Print("Ola 3");
            hideActivityIndicator();
            Debug.Print("Ola 4");
            return "";
		}

		public string getClassID(List<Class_Detail> class_Details, string classId)
		{
			Debug.Print("getClassID begin "+ classId);
			foreach (Class_Detail class_detail in class_Details)
			{
				Debug.Print("getClassID class_detail.id = "+ class_detail.id);
				if (class_detail.id == classId)
				{
					return class_detail.id;
				}
			}
			return null;			
		}

	}

}