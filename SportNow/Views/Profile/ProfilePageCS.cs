using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Text.RegularExpressions;
using SportNow.Services.Camera;
using System.Net;
using System.IO;
using SkiaSharp;
using Xamarin.Essentials;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;

namespace SportNow.Views.Profile
{
    public class ProfilePageCS : DefaultPage
	{
		private ScrollView scrollView;

		private Member member;

		Label nameLabel;

        MenuButton geralButton;
		MenuButton moradaButton;
		MenuButton encEducacaoButton;
		MenuButton infoEscolarButton;


		StackLayout stackButtons;
		private Grid gridGeral;
		private Grid gridMorada;
		private Grid gridEncEducacao;
		private Grid gridInfoEscolar;

		RoundImage memberPhotoImage;
		FormValue nameValue;
		FormValue emailValue;
		FormValue nifValue;
		FormValue birthdateValue;
		FormValueEdit cc_numberValue;
		FormValueEditPicker countryValue;
		FormValueEditPicker genderValue;
		FormValueEdit phoneValue;
        FormValueEditLongText addressValue;
		FormValueEdit cityValue;
		FormValueEditCodPostal postalcodeValue;
		FormValueEdit nameEmergencyContactValue;
		FormValueEdit phoneEmergencyContactValue;
		FormValueEdit EncEducacao1NomeValue;
		FormValueEdit EncEducacao1PhoneValue;
		FormValueEdit EncEducacao1MailValue;
		FormValueEdit EncEducacao2NomeValue;
		FormValueEdit EncEducacao2PhoneValue;
		FormValueEdit EncEducacao2MailValue;

		FormValueEdit schoolNameValue;
		FormValueEdit schoolNumberValue;
		FormValueEdit schoolYearValue;
		FormValueEdit schoolClassValue;

		FormValueEditLongText comentariosValue;

		bool changeMember = false;

		bool enteringPage = true;

		Button activateButton;

		Label currentVersionLabel;

		Stream stream;
        ImageSource source;

		Image documentosImage;

		int documentosImage_y_index;

        //bool isDocument = false;

        protected async override void OnAppearing()
        {

            showActivityIndicator();

            Debug.Print("OnAppearing");

			if (documentosImage == null)
			{
                if (App.member.member_type == "praticante")
                {
                    await createDocumentsGridButton(documentosImage_y_index);
                } 
			}
			if (changeMember == true)
			{
				relativeLayout.Children.Remove(memberPhotoImage);
                relativeLayout.Children.Remove(nameLabel);
                relativeLayout.Children.Remove(gridGeral);
                relativeLayout.Children.Remove(gridMorada);
                relativeLayout.Children.Remove(gridEncEducacao);
                relativeLayout.Children.Remove(gridInfoEscolar);

				CreatePhoto();
                CreateName();
                CreateGridGeral();
                CreateGridMorada();
                CreateGridEncEducacao();
                CreateGridInfoEscolar();
            }

            hideActivityIndicator();

        }

        protected async override void OnDisappearing()
        {
            Debug.Print("OnDisappearing");

			/*if (isDocument == true)
            {

				if (relativeLayout == null)
				{
                    Debug.Print("OnDisappearing relativeLayout == null");
                }
                if (stackButtons == null)
                {
                    Debug.Print("OnDisappearing stackButtons == null");
                }

                if ((relativeLayout != null) & (stackButtons != null))
				{
                    relativeLayout.Children.Remove(stackButtons);
                    stackButtons = null;
                }
            }*/

			/*relativeLayout = null;
            this.Content = null;
			this.streamSource = null;
			this.source = null;*/

			/*relativeLayout.Children.Remove(memberPhotoImage);
			memberPhotoImage = null;*/
			if (documentosImage != null)
			{
                relativeLayout.Children.Remove(documentosImage);
                documentosImage = null;
            }
            

            if (changeMember == false)
            {
                await UpdateMemberInfo();
            }

			/*           if (relativeLayout != null)
                        {
                            for (int i = 0; i < this.relativeLayout.Children.Count; i++)
                            {
                                this.relativeLayout.Children.RemoveAt(i);
                            }
                            scrollView = null;
                           //ToolbarItems.RemoveAt(0);
                        }*/

			/*if (isPhoto == false)
			{
                relativeLayout = null;
                this.Content = null;
            }
            isPhoto = false;*/

			
        }

        public void initLayout()
		{
			Title = "PERFIL";

			var toolbarItem = new ToolbarItem
			{
				Text = "Logout"
			};
			toolbarItem.Clicked += OnLogoutButtonClicked;
			ToolbarItems.Add(toolbarItem);

		}


		public async Task<string> initSpecificLayout()
		{
			Debug.Print("ProfilePageCS - initSpecificLayout");
			if (relativeLayout == null)
			{
                relativeLayout = new RelativeLayout
                {
                    Margin = new Thickness(10)
                };
                Content = relativeLayout;
            }
            member = App.member;

			scrollView = new ScrollView { Orientation = ScrollOrientation.Vertical, HeightRequest = 300, IsClippedToBounds = true, BackgroundColor = Color.White };

			relativeLayout.Children.Add(scrollView,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(270 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - 300 * App.screenHeightAdapter; // center of image (which is 40 wide)
				})
			);

			int countStudents = App.original_member.students_count;

			CreatePhoto();
			CreateName();
			CreateStackButtons();
			CreateGridGeral();
			CreateGridMorada();
			CreateGridEncEducacao();
			CreateGridInfoEscolar();
			CreateGridButtons();

			/*gridIdentificacao.IsVisible = false;
			gridMorada.IsVisible = false;
			gridEncEducacao.IsVisible = false;*/
			OnGeralButtonClicked(null, null);

			return "";
		}

		public void CreateName()
		{
			nameLabel = new Label
			{
				Text = App.member.nickname,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = App.normalTextColor,
				LineBreakMode = LineBreakMode.WordWrap,
				FontSize = App.bigTitleFontSize,
				TextTransform = TextTransform.Uppercase
			};
			relativeLayout.Children.Add(nameLabel,
				xConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(160 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 20 * App.screenHeightAdapter); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant((70 * App.screenHeightAdapter))
			);


            /*var nameLabel_tap = new TapGestureRecognizer();
            nameLabel_tap.Tapped += memberNameLabelTappedAsync;
            nameLabel.GestureRecognizers.Add(nameLabel_tap);*/
        }

		public async void CreatePhoto()
		{
			memberPhotoImage = new RoundImage();

            if (App.member.consentimento_fotosocio == "1")
			{
				/*if (source != null)
				{
					Debug.Print("source != null "+ source.IsEmpty.ToString() + ".");
                    memberPhotoImage.photo.Source = source;
                    var memberPhotoImage_tap = new TapGestureRecognizer();
                    memberPhotoImage_tap.Tapped += memberPhotoImageTappedAsync;
                    memberPhotoImage.GestureRecognizers.Add(memberPhotoImage_tap);
                }
                else
				{*/
                    //Debug.Print("source == null");
                    WebResponse response;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Constants.images_URL + App.member.id + "_photo");

                    request.Method = "HEAD";
                    bool exists;
                    try
                    {
                        response = request.GetResponse();
                        Debug.Print("response.Headers.GetType()= " + response.Headers.GetType());
                        exists = true;
                    }
                    catch (Exception ex)
                    {
                        exists = false;
                    }

                    Debug.Print("Photo exists? = " + exists);

                    if (exists)
                    {

                        memberPhotoImage.Source = new UriImageSource
                        {
                            Uri = new Uri(Constants.images_URL + App.member.id + "_photo"),
                            CachingEnabled = false,
                            CacheValidity = new TimeSpan(0, 0, 0, 1)
                        };
                    }
                    else
                    {
                        memberPhotoImage.Source = "iconadicionarfoto.png";
                    }

                    var memberPhotoImage_tap = new TapGestureRecognizer();
                    memberPhotoImage_tap.Tapped += memberPhotoImageTappedAsync;
                    memberPhotoImage.GestureRecognizers.Add(memberPhotoImage_tap);

					relativeLayout.Children.Add(memberPhotoImage,
						xConstraint: Constraint.RelativeToParent((parent) =>
						{
							return (parent.Width / 2) - (80 * App.screenHeightAdapter);
						}),
						yConstraint: Constraint.Constant(0),
						widthConstraint: Constraint.Constant(180 * App.screenHeightAdapter),
						heightConstraint: Constraint.Constant(180 * App.screenHeightAdapter) // size of screen -80
					);
                //}
            }
			else
			{
				memberPhotoImage.Source = "iconadicionarfoto.png";

				var memberPhotoImage_tap = new TapGestureRecognizer();
				memberPhotoImage_tap.Tapped += memberPhotoImageTapped_NotAuthorized_Async;
				memberPhotoImage.GestureRecognizers.Add(memberPhotoImage_tap);

			}
            relativeLayout.Children.Add(memberPhotoImage,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 2) - (80 * App.screenHeightAdapter);
				}),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.Constant(180 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(180 * App.screenHeightAdapter) // size of screen -80
			);

		}

		public ProfilePageCS()
		{
            this.initLayout();
			this.initSpecificLayout();

        }

		public void CreateStackButtons()
		{
			var width = Constants.ScreenWidth;
			var buttonWidth = (width - (50 * App.screenWidthAdapter)) / 4;

			geralButton = new MenuButton("GERAL", buttonWidth, 40 * App.screenHeightAdapter);
			geralButton.button.Clicked += OnGeralButtonClicked;
			moradaButton = new MenuButton("MORADA", buttonWidth, 40 * App.screenHeightAdapter);
			moradaButton.button.Clicked += OnMoradaButtonClicked;
			encEducacaoButton = new MenuButton("ENC\nEDUCAÇÃO", buttonWidth, 40 * App.screenHeightAdapter);
			encEducacaoButton.button.Clicked += OnEncEducacaoButtonClicked;
			infoEscolarButton = new MenuButton("INFO\nESCOLAR", buttonWidth, 40 * App.screenHeightAdapter);
			infoEscolarButton.button.Clicked += OnInfoEscolarButtonClicked;

			stackButtons = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Children =
				{
					geralButton,
					moradaButton,
					encEducacaoButton,
					infoEscolarButton
				}
			};

			relativeLayout.Children.Add(stackButtons,
			xConstraint: Constraint.Constant(0),
			yConstraint: Constraint.Constant(220 * App.screenHeightAdapter),
			widthConstraint: Constraint.RelativeToParent((parent) =>
			{
				return (parent.Width);
			}),
			heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter) // size of screen -80
			);

			geralButton.activate();
			infoEscolarButton.deactivate();
			moradaButton.deactivate();
			encEducacaoButton.deactivate();
		}

		public async void CreateGridButtons()
		{
            Debug.Print("CreateGridButtons");

			MemberManager memberManager = new MemberManager();

            int y_button_left = 0;
			int y_button_right = 0;

			Image changePasswordImage = new Image
			{
				Source = "botaoalterarpass.png",
				Aspect = Aspect.AspectFit
			};

			TapGestureRecognizer changePasswordImage_tapEvent = new TapGestureRecognizer();
			changePasswordImage_tapEvent.Tapped += OnChangePasswordButtonClicked;
			changePasswordImage.GestureRecognizers.Add(changePasswordImage_tapEvent);

			relativeLayout.Children.Add(changePasswordImage,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 47.5);
				}),
				yConstraint: Constraint.Constant(y_button_right * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(35 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(35 * App.screenHeightAdapter)
			);

			//Debug.Print("y_button_right 0 = " + y_button_right);

			Label changePasswordLabel = new Label
			{
				Text = "Segurança",
				TextColor = App.normalTextColor,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Start,
				FontSize = App.smallTextFontSize
			};

			relativeLayout.Children.Add(changePasswordLabel,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - 60);
					}),
					yConstraint: Constraint.Constant((y_button_right + 37) * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(15 * App.screenHeightAdapter)
			);

			//Debug.Print("y_button_right 1 = " + y_button_right);

			y_button_right = y_button_right + 60;

			Image consentimentosImage = new Image
			{
				Source = "iconconsentimentos.png",
				Aspect = Aspect.AspectFit
			};

			TapGestureRecognizer consentimentosImage_tapEvent = new TapGestureRecognizer();
			consentimentosImage_tapEvent.Tapped += OnConsentimentosButtonClicked;
			consentimentosImage.GestureRecognizers.Add(consentimentosImage_tapEvent);

			relativeLayout.Children.Add(consentimentosImage,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 47.5);
				}),
				yConstraint: Constraint.Constant(y_button_right * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(35 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(35 * App.screenHeightAdapter)
			);

			Label consentimentosLabel = new Label
			{
				Text = "Consentimentos",
				TextColor = App.normalTextColor,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Start,
				FontSize = App.smallTextFontSize
			};

			relativeLayout.Children.Add(consentimentosLabel,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - 60);
					}),
					yConstraint: Constraint.Constant((y_button_right + 37) * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(15 * App.screenHeightAdapter)
			);

			y_button_right = y_button_right + 60;

			//Debug.Print("y_button_right 2 = " + y_button_right);

			if (App.member.member_type == "praticante")
			{
                y_button_right = await createDocumentsGridButton(y_button_right);
            }

			if (App.members.Count > 1)
			{

				Button changeMemberButton = new Button { HorizontalOptions = LayoutOptions.Center, BackgroundColor = Color.Transparent, ImageSource = "botaoalmudarconta.png", HeightRequest = 30 };

				/*gridButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = buttonWidth });
				//RoundButton changeMemberButton = new RoundButton("Login Outro Sócio", buttonWidth-5, 40);
				changeMemberButton.Clicked += OnChangeMemberButtonClicked;*/

				Image changeMemberImage = new Image
				{
					Source = "botaoalmudarconta.png",
					Aspect = Aspect.AspectFit
				};

				TapGestureRecognizer changeMemberImage_tapEvent = new TapGestureRecognizer();
				changeMemberImage_tapEvent.Tapped += OnChangeMemberButtonClicked;
				changeMemberImage.GestureRecognizers.Add(changeMemberImage_tapEvent);

				relativeLayout.Children.Add(changeMemberImage,
					xConstraint: Constraint.Constant(12.5 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_button_left * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(35 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(35 * App.screenHeightAdapter)
				);

				Label changeMemberLabel = new Label
				{
					Text = "Mudar Utilizador",
					TextColor = App.normalTextColor,
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Start,
					FontSize = App.smallTextFontSize
				};

				relativeLayout.Children.Add(changeMemberLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant((y_button_left + 37) * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(15 * App.screenHeightAdapter)
				);

				//Debug.Print("y_button_left 0 = " + y_button_left);

				y_button_left = y_button_left + 60;

				//Debug.Print("y_button_left 1 = " + y_button_left);
			}

			if (App.original_member.students_count > 1)
			{
				Image changeStudentImage = new Image
				{
					Source = "iconescolheraluno.png",
					Aspect = Aspect.AspectFit
				};

				TapGestureRecognizer changeStudentImage_tapEvent = new TapGestureRecognizer();
				changeStudentImage_tapEvent.Tapped += OnChangeStudentButtonClicked;
				changeStudentImage.GestureRecognizers.Add(changeStudentImage_tapEvent);

				relativeLayout.Children.Add(changeStudentImage,
					xConstraint: Constraint.Constant(12.5 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_button_left * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(35 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(35 * App.screenHeightAdapter)
				);

				Label changeStudentLabel = new Label
				{
					Text = "Login Aluno",
					TextColor = App.normalTextColor,
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Start,
					FontSize = App.smallTextFontSize
				};

				relativeLayout.Children.Add(changeStudentLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant((y_button_left + 37) * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(15 * App.screenHeightAdapter)
				);

				y_button_left = y_button_left + 60;

				//Debug.Print("y_button_left 2 = " + y_button_left);
			}

			//Debug.Print("App.member.isInstrutorResponsavel = " + App.member.isInstrutorResponsavel);
			//Debug.Print("App.member.isResponsavelAdministrativo = " + App.member.isResponsavelAdministrativo);

			if ((App.member.isInstrutorResponsavel == "1") | (App.member.isResponsavelAdministrativo == "1"))
			{
				Image membersToApproveImage = new Image
				{
					Source = "iconaprovarinscricoes.png",
					Aspect = Aspect.AspectFit
				};

				TapGestureRecognizer membersToApproveImage_tapEvent = new TapGestureRecognizer();
				membersToApproveImage_tapEvent.Tapped += membersToApproveImage_Clicked;
				membersToApproveImage.GestureRecognizers.Add(membersToApproveImage_tapEvent);


				relativeLayout.Children.Add(membersToApproveImage,
					xConstraint: Constraint.Constant(12.5 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_button_left * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(35 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(35 * App.screenHeightAdapter)
				);

				Label membersToApproveLabel = new Label
				{
					Text = "Aprovar Inscrições",
					TextColor = App.normalTextColor,
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Start,
					FontSize = App.smallTextFontSize
				};

				relativeLayout.Children.Add(membersToApproveLabel,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant((y_button_left + 37) * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(15 * App.screenHeightAdapter)
				);

				y_button_left = y_button_left + 60;
			}

			/*if (App.original_member.id != App.member.id)
			{
				Image originalMemberImage = new Image
				{
					Source = "botaoalterarpass.png",
					WidthRequest = 40 * App.screenHeightAdapter,
					HeightRequest = 40 * App.screenHeightAdapter,
					Aspect = Aspect.AspectFit
				};

				TapGestureRecognizer originalMemberImage_tapEvent = new TapGestureRecognizer();
				originalMemberImage_tapEvent.Tapped += OnBackOriginalButtonClicked;
				originalMemberImage.GestureRecognizers.Add(originalMemberImage_tapEvent);

				relativeLayout.Children.Add(originalMemberImage,
					xConstraint: Constraint.Constant(12.5 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_button_left),
					widthConstraint: Constraint.Constant(40 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
				);

				//gridButtons.Children.Add(originalMemberImage, numberOfButtons, 0);
			}*/

			currentVersionLabel = new Label
			{
				Text = "Version "+ App.VersionNumber + " "+App.BuildNumber,
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Start,
				VerticalTextAlignment = TextAlignment.Start,
				FontSize = 10 * App.screenHeightAdapter
			};

			relativeLayout.Children.Add(currentVersionLabel,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height - 20);
				}),
				widthConstraint: Constraint.Constant(150 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(20 * App.screenHeightAdapter)
			);

		}


        public async Task<int> createDocumentsGridButton(int y_button_right)
		{


            documentosImage = new Image
			{
				Aspect = Aspect.AspectFit
			};

            MemberManager memberManager = new MemberManager();
            App.member.documents = await memberManager.Get_Member_Documents(App.member.id);


            string termo_responsabilidade_status = "";
			string atestado_medico_status = "";

			foreach (Document document in App.member.documents)
			{
				if (document.type == "termo_responsabilidade")
				{
					termo_responsabilidade_status = document.status;
				}
				else if (document.type == "atestado_medico")
				{
					atestado_medico_status = document.status;
				}
			}

			Debug.Print("CreateGridButtons - App.member.aulatipo = " + App.member.aulatipo);

			if (App.member.aulatipo == "acro")
			{
				Debug.Print("CreateGridButtons - atestado_medico_status = " + atestado_medico_status);
				if (atestado_medico_status == "Aprovado")
				{
					documentosImage.Source = "icondocumentosaprovados.png";
				}
				else if (atestado_medico_status == "Under Review")
				{
					documentosImage.Source = "icondocumentosporconfirmar.png";
				}
				else
				{
					documentosImage.Source = "icondocumentosrejeitados.png";
                    await DisplayAlert("DOCUMENTOS EM FALTA", "Submeta por favor um atestado médico válido.", "OK");
					
                }
			}
			else
			{
				Debug.Print("CreateGridButtons - termo_responsabilidade_status = " + termo_responsabilidade_status);
				if (termo_responsabilidade_status == "Aprovado")
				{
					documentosImage.Source = "icondocumentosaprovados.png";
				}
				else if (termo_responsabilidade_status == "Under Review")
				{
					documentosImage.Source = "icondocumentosporconfirmar.png";
				}
				else
				{
					documentosImage.Source = "icondocumentosrejeitados.png";
                }
			}


			TapGestureRecognizer documentosImage_tapEvent = new TapGestureRecognizer();
			documentosImage_tapEvent.Tapped += OnDocumentosButtonClicked;
			documentosImage.GestureRecognizers.Add(documentosImage_tapEvent);

			relativeLayout.Children.Add(documentosImage,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 47.5);
				}),
				yConstraint: Constraint.Constant(y_button_right * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(35 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(35 * App.screenHeightAdapter)
			);

			Label documentosImageLabel = new Label
			{
				Text = "Documentos",
				TextColor = App.normalTextColor,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Start,
				FontSize = App.smallTextFontSize
			};

			relativeLayout.Children.Add(documentosImageLabel,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 60);
				}),
				yConstraint: Constraint.Constant((y_button_right + 37) * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(15 * App.screenHeightAdapter)
			);
            documentosImage_y_index = y_button_right;

            y_button_right = y_button_right + 60;

            return y_button_right;
		}

		public void CreateGridGeral() {

			gridGeral = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
			//gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter });
			//gridGeral.RowDefinitions.Add(new RowDefinition { Height = 1 });
			gridGeral.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			gridGeral.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			/*FormLabel nameLabel = new FormLabel { Text = "NOME", HorizontalTextAlignment = TextAlignment.Start };
			nameValue = new FormValue(member.name);*/

			FormLabel nifLabel = new FormLabel { Text = "NIF" };
			nifValue = new FormValue(member.nif);

			FormLabel birthdateLabel = new FormLabel { Text = "NASCIMENTO" };
			birthdateValue = new FormValue(member.birthdate);//?.ToString("yyyy-MM-dd"));

			FormLabel cc_numberLabel = new FormLabel { Text = "CC" };
			cc_numberValue = new FormValueEdit(member.cc_number, Keyboard.Numeric);

			List<string> gendersList = new List<string>();
			foreach (KeyValuePair<string, string> entry in Constants.genders)
			{
				gendersList.Add(entry.Value);
			}

			FormLabel genderLabel = new FormLabel { Text = "GÉNERO" };
			genderValue = new FormValueEditPicker(Constants.genders[member.gender], gendersList);


			List<string> countriesList = new List<string>();
			foreach (KeyValuePair<string, string> entry in Constants.countries)
			{
				countriesList.Add(entry.Value);
			}
			FormLabel countryLabel = new FormLabel { Text = "NACIONALIDADE" };
			countryValue = new FormValueEditPicker(Constants.countries[member.country], countriesList);

			FormLabel phoneLabel = new FormLabel { Text = "TELEFONE" };
			phoneValue = new FormValueEdit(member.phone, Keyboard.Telephone);

			FormLabel comentariosLabel = new FormLabel { Text = "COMENTÁRIOS" };
			comentariosValue = new FormValueEditLongText(member.comentarios, Keyboard.Text);
			comentariosValue.MinimumHeightRequest = 80 * App.screenHeightAdapter;
			comentariosValue.HeightRequest = 80 * App.screenHeightAdapter;

			/*FormLabel emailLabel = new FormLabel { Text = "EMAIL" };
			emailValue = new FormValue(member.email);*/

			/*gridGeral.Children.Add(nameLabel, 0, 0);
			gridGeral.Children.Add(nameValue, 1, 0);*/

			gridGeral.Children.Add(nifLabel, 0, 0);
			gridGeral.Children.Add(nifValue, 1, 0);

			gridGeral.Children.Add(cc_numberLabel, 0, 1);
			gridGeral.Children.Add(cc_numberValue, 1, 1);

			gridGeral.Children.Add(birthdateLabel, 0, 2);
			gridGeral.Children.Add(birthdateValue, 1, 2);

			gridGeral.Children.Add(genderLabel, 0, 3);
			gridGeral.Children.Add(genderValue, 1, 3);

			gridGeral.Children.Add(countryLabel, 0, 4);
			gridGeral.Children.Add(countryValue, 1, 4);

			gridGeral.Children.Add(phoneLabel, 0, 5);
			gridGeral.Children.Add(phoneValue, 1, 5);

			comentariosValue.entry.AutoSize = EditorAutoSizeOption.Disabled;
			comentariosValue.entry.Focused+= editor_Focused;


			gridGeral.Children.Add(comentariosLabel, 0, 6);
			gridGeral.Children.Add(comentariosValue, 1, 6);
		}


		async void editor_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
		{
			if (Device.RuntimePlatform == Device.iOS)
			{
				Debug.Print("scrollView.ScrollY = " + scrollView.ScrollY);
				scrollView.ScrollToAsync(scrollView.ScrollX, 300, true);
			}
			Debug.Print("scrollView.ScrollY after = " + scrollView.ScrollY);
		}

		public void CreateGridInfoEscolar()
		{

			gridInfoEscolar = new Grid { Padding = 10 };
			gridInfoEscolar.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInfoEscolar.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInfoEscolar.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInfoEscolar.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridInfoEscolar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			gridInfoEscolar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			FormLabel schoolNameLabel = new FormLabel { Text = "Escola" };
			schoolNameValue = new FormValueEdit(member.schoolname);

			FormLabel schoolNumberLabel = new FormLabel { Text = "Número" };
			schoolNumberValue = new FormValueEdit(member.schoolnumber);


			FormLabel schoolYearLabel = new FormLabel { Text = "Ano" };
			schoolYearValue = new FormValueEdit(member.schoolyear);

			FormLabel schoolClassLabel = new FormLabel { Text = "Turma" };
			schoolClassValue = new FormValueEdit(member.schoolclass);


			gridInfoEscolar.Children.Add(schoolNameLabel, 0, 0);
			gridInfoEscolar.Children.Add(schoolNameValue, 1, 0);

			gridInfoEscolar.Children.Add(schoolNumberLabel, 0, 1);
			gridInfoEscolar.Children.Add(schoolNumberValue, 1, 1);

			gridInfoEscolar.Children.Add(schoolYearLabel, 0, 2);
			gridInfoEscolar.Children.Add(schoolYearValue, 1, 2);

			gridInfoEscolar.Children.Add(schoolClassLabel, 0, 3);
			gridInfoEscolar.Children.Add(schoolClassValue, 1, 3);

		}

		public void CreateGridMorada()
		{

			gridMorada = new Grid { Padding = 10 };
			gridMorada.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMorada.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMorada.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridMorada.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			gridMorada.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

            FormLabel addressLabel = new FormLabel { Text = "ENDEREÇO" };
            addressValue = new FormValueEditLongText(member.address, Keyboard.Text);
            addressValue.MinimumHeightRequest = 60 * App.screenHeightAdapter;
            addressValue.HeightRequest = 60 * App.screenHeightAdapter;

            addressValue.entry.AutoSize = EditorAutoSizeOption.Disabled;
            addressValue.entry.Focused += editor_Focused;

            FormLabel postalcodeLabel = new FormLabel { Text = "CÓD POSTAL" };
			postalcodeValue = new FormValueEditCodPostal(member.postalcode);

			FormLabel cityLabel = new FormLabel { Text = "LOCALIDADE" };
			cityValue = new FormValueEdit(member.city);


			/*gridMorada.Children.Add(phoneLabel, 0, 1);
			gridMorada.Children.Add(phoneValue, 1, 1);*/

			gridMorada.Children.Add(addressLabel, 0, 0);
			gridMorada.Children.Add(addressValue, 1, 0);

			gridMorada.Children.Add(postalcodeLabel, 0, 1);
			gridMorada.Children.Add(postalcodeValue, 1, 1);

			gridMorada.Children.Add(cityLabel, 0, 2);
			gridMorada.Children.Add(cityValue, 1, 2);
		}

		public void CreateGridEncEducacao()
		{

			gridEncEducacao = new Grid { Padding = 10 };
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEncEducacao.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			gridEncEducacao.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

			FormLabel emergencyContactLabel = new FormLabel { Text = "CONTACTO EMERGÊNCIA", FontSize = App.itemTitleFontSize };

			FormLabel nameEmergencyContactLabel = new FormLabel { Text = "NOME" };
			nameEmergencyContactValue = new FormValueEdit(member.nameEmergencyContact);

			FormLabel phoneEmergencyContactLabel = new FormLabel { Text = "TELEFONE" };
			phoneEmergencyContactValue = new FormValueEdit(member.phoneEmergencyContact, Keyboard.Telephone);

			FormLabel EncEducacao1Label = new FormLabel { Text = "ENCARREGADO DE EDUCAÇÃO 1", FontSize = App.itemTitleFontSize };

			FormLabel EncEducacao1NomeLabel = new FormLabel { Text = "NOME" };
			EncEducacao1NomeValue = new FormValueEdit(member.name_enc1);

			FormLabel EncEducacao1PhoneLabel = new FormLabel { Text = "TELEFONE" };
			EncEducacao1PhoneValue = new FormValueEdit(member.phone_enc1, Keyboard.Telephone);

			FormLabel EncEducacao1MailLabel = new FormLabel { Text = "MAIL" };
			EncEducacao1MailValue = new FormValueEdit(member.mail_enc1, Keyboard.Email);

			FormLabel EncEducacao2Label = new FormLabel { Text = "ENCARREGADO DE EDUCAÇÃO 2", FontSize = App.itemTitleFontSize };

			FormLabel EncEducacao2NomeLabel = new FormLabel { Text = "NOME" };
			EncEducacao2NomeValue = new FormValueEdit(member.name_enc2);

			FormLabel EncEducacao2PhoneLabel = new FormLabel { Text = "TELEFONE" };
			EncEducacao2PhoneValue = new FormValueEdit(member.phone_enc2, Keyboard.Telephone);

			FormLabel EncEducacao2MailLabel = new FormLabel { Text = "MAIL" };
			EncEducacao2MailValue = new FormValueEdit(member.mail_enc2, Keyboard.Email);


			gridEncEducacao.Children.Add(emergencyContactLabel, 0, 0);
			Grid.SetColumnSpan(emergencyContactLabel, 2);

			gridEncEducacao.Children.Add(nameEmergencyContactLabel, 0, 1);
			gridEncEducacao.Children.Add(nameEmergencyContactValue, 1, 1);

			gridEncEducacao.Children.Add(phoneEmergencyContactLabel, 0, 2);
			gridEncEducacao.Children.Add(phoneEmergencyContactValue, 1, 2);


			gridEncEducacao.Children.Add(EncEducacao1Label, 0, 3);
			Grid.SetColumnSpan(EncEducacao1Label, 2);

			gridEncEducacao.Children.Add(EncEducacao1NomeLabel, 0, 4);
			gridEncEducacao.Children.Add(EncEducacao1NomeValue, 1, 4);

			gridEncEducacao.Children.Add(EncEducacao1PhoneLabel, 0, 5);
			gridEncEducacao.Children.Add(EncEducacao1PhoneValue, 1, 5);

			gridEncEducacao.Children.Add(EncEducacao1MailLabel, 0, 6);
			gridEncEducacao.Children.Add(EncEducacao1MailValue, 1, 6);

			gridEncEducacao.Children.Add(EncEducacao2Label, 0, 7);
			Grid.SetColumnSpan(EncEducacao2Label, 2);

			gridEncEducacao.Children.Add(EncEducacao2NomeLabel, 0, 8);
			gridEncEducacao.Children.Add(EncEducacao2NomeValue, 1, 8);

			gridEncEducacao.Children.Add(EncEducacao2PhoneLabel, 0, 9);
			gridEncEducacao.Children.Add(EncEducacao2PhoneValue, 1, 9);

			gridEncEducacao.Children.Add(EncEducacao2MailLabel, 0, 10);
			gridEncEducacao.Children.Add(EncEducacao2MailValue, 1, 10);
		}


		async void OnGeralButtonClicked(object sender, EventArgs e)
		{
			geralButton.activate();
			infoEscolarButton.deactivate();
			moradaButton.deactivate();
			encEducacaoButton.deactivate();

			scrollView.Content = gridGeral;

			if (enteringPage == false)
			{
				await UpdateMemberInfo();
				enteringPage = false;
			}


		}

		async void OnMoradaButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnMoradaButtonClicked");

			geralButton.deactivate();
			moradaButton.activate();
			encEducacaoButton.deactivate();
			infoEscolarButton.deactivate();

			scrollView.Content = gridMorada;

			await UpdateMemberInfo();
		}

		async void OnEncEducacaoButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnEncEducacaoButtonClicked");

			geralButton.deactivate();
			moradaButton.deactivate();
			encEducacaoButton.activate();
			infoEscolarButton.deactivate();

			scrollView.Content = gridEncEducacao;

			await UpdateMemberInfo();
		}


		async void OnInfoEscolarButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnIdentificacaoButtonClicked");
			geralButton.deactivate();
			moradaButton.deactivate();
			encEducacaoButton.deactivate();
			infoEscolarButton.activate();

			scrollView.Content = gridInfoEscolar;

			await UpdateMemberInfo();
		}

		async void OnLogoutButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnLogoutButtonClicked");

			Application.Current.Properties.Remove("EMAIL");
			Application.Current.Properties.Remove("PASSWORD");
			Application.Current.Properties.Remove("SELECTEDUSER");

			App.member = null;
			App.members = null;

			Application.Current.SavePropertiesAsync();

			Application.Current.MainPage = new NavigationPage(new LoginPageCS(""))
			{
				BarBackgroundColor = Color.FromRgb(15, 15, 15),
				BarTextColor = Color.White//FromRgb(75, 75, 75)
			};
		}

		async void OnChangePasswordButtonClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ChangePasswordPageCS(member));
		}


		async void OnConsentimentosButtonClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ConsentPageCS());
		}

		async void OnDocumentosButtonClicked(object sender, EventArgs e)
		{
			//isDocument = true;
			Navigation.PushAsync(new DocumentsPageCS());
		}

		async void membersToApproveImage_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ApproveRegistrationPageCS());
		}
		

		async void OnChangeMemberButtonClicked(object sender, EventArgs e)
		{

            await UpdateMemberInfo();
            changeMember = true;
			Navigation.PushAsync(new SelectMemberPageCS());
			//await Navigation.PopAsync();
			//await Navigation.PopAsync();
		}

		async void OnChangeStudentButtonClicked(object sender, EventArgs e)
		{
            await UpdateMemberInfo();
            changeMember = true;
			Navigation.PushAsync(new SelectStudentPageCS());
			//await Navigation.PopAsync();
			//await Navigation.PopAsync();

			//Navigation.PushAsync(new SelectStudentPageCS());
		}

		/*async void OnBackOriginalButtonClicked(object sender, EventArgs e)
		{
			//changeMember = true;
			App.member = App.original_member;
			//Navigation.PushAsync(new MainTabbedPageCS(""));
			Navigation.InsertPageBefore(new MainTabbedPageCS("",""), this);
			await Navigation.PopAsync();
			await Navigation.PopAsync();

		}*/

		async void OnActivateButtonClicked(object sender, EventArgs e)
		{

			activateButton.IsEnabled = false;

			MemberManager memberManager = new MemberManager();

			if (App.member.currentFee is null)
			{
				var result_create = await memberManager.CreateFee(member, DateTime.Now.ToString("yyyy"));
				if (result_create == -1)
				{
					Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
					{
						BarBackgroundColor = Color.FromRgb(15, 15, 15),
						BarTextColor = Color.White
					};
					return;
				}
				var result_get = await GetCurrentFees(member);
				if (result_get == -1)
				{
					Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
					{
						BarBackgroundColor = Color.FromRgb(15, 15, 15),
						BarTextColor = Color.White
					};
					return;
				}
			}

			await Navigation.PushAsync(new QuotasMBPageCS(member));
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
				return -1;
			}
			return result;
		}


		async Task<string> UpdateMemberInfo()
		{
			Debug.Print("UpdateMemberInfo");
			if (App.member != null)
			{
                if (string.IsNullOrEmpty(postalcodeValue.entry.Text))
				{
					postalcodeValue.entry.Text = "";
				}
				/*if (nameValue.entry.Text == "")
				{
					nameValue.entry.Text = App.member.name;
					UserDialogs.Instance.Alert(new AlertConfig() { Title = "DADOS INVÁLIDOS", Message = "O nome introduzido não é válido.", OkText = "Ok" });
					return "-1";
				}*/
				if (cc_numberValue.entry.Text == "")
				{
                    await DisplayAlert("DADOS INVÁLIDOS", "O número de identificação (CC) introduzido não é válido.", "OK");
                    //OnGeralButtonClicked(null, null);
                    return "-1";
                }


                if (genderValue.picker.SelectedItem.ToString() == "Não Definido")
                {
                    await DisplayAlert("DADOS INVÁLIDOS", "Defina por favor o Género.", "OK");
                    return "-1";
                }

                if (phoneValue.entry.Text == "")
				{
                    await DisplayAlert("DADOS INVÁLIDOS", "O telefone introduzido não é válido.", "OK");
                    //OnGeralButtonClicked(null, null);
                    return "-1";
                }
                else if ((phoneValue.entry.Text != "") & (phoneValue.entry.Text != null))
				{
					if ((phoneValue.entry.Text.Length > 0) & (phoneValue.entry.Text.Length < 9))
					{
                        await DisplayAlert("DADOS INVÁLIDOS", "O telefone introduzido tem de ter pelo menos 9 dígitos.", "OK");
                        //UserDialogs.Instance.Alert(new AlertConfig() { Title = "DADOS INVÁLIDOS", Message = "O telefone introduzido tem de ter pelo menos 9 dígitos.", OkText = "Ok" });
						//OnGeralButtonClicked(null, null);
						return "-1";
					}
					else if (!Regex.IsMatch(phoneValue.entry.Text, @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$"))
					{
						phoneValue.entry.Text = App.member.phone;
						//OnGeralButtonClicked(null, null);
                        await DisplayAlert("DADOS INVÁLIDOS", "O telefone introduzido não é válido.", "OK");
                        //UserDialogs.Instance.Alert(new AlertConfig() { Title = "DADOS INVÁLIDOS", Message = "O telefone introduzido não é válido.", OkText = "Ok" });
						return "-1";
					}
				}

                if ((postalcodeValue.entry.Text == "") | (addressValue.entry.Text == "") | (cityValue.entry.Text == ""))
                {

                    await DisplayAlert("DADOS INVÁLIDOS", "A morada não pode ser vazia.", "OK");
                    //OnMoradaButtonClicked(null, null);
                    return "-1";
                }

                if ((postalcodeValue.entry.Text != "") & (!Regex.IsMatch((postalcodeValue.entry.Text), @"^\d{4}-\d{3}$")))
				{
                    await DisplayAlert("DADOS INVÁLIDOS", "O código postal introduzido não é válido.", "OK");
					//OnMoradaButtonClicked(null, null);
					return "-1";
				}

                if ((App.member.member_type == "praticante") & ((nameEmergencyContactValue.entry.Text == "") | (phoneEmergencyContactValue.entry.Text == "")))
                {
                    await DisplayAlert("DADOS INVÁLIDOS", "Tem de introduzir os dados de um contacto de emergência.", "OK");
                    //OnEncEducacaoButtonClicked(null, null);
                    return "-1";
                }

                //App.member.name = nameValue.entry.Text;
                //App.member.email = emailValue.label.Text;
                //App.member.birthdate = birthdateValue.entry.Text; //DateTime.Parse(birthdateValue.entry.Text);
                //App.member.nif = nifValue.entry.Text;
                App.member.cc_number = cc_numberValue.entry.Text;
				App.member.country = Constants.KeyByValue(Constants.countries, countryValue.picker.SelectedItem.ToString());
				App.member.gender = Constants.KeyByValue(Constants.genders, genderValue.picker.SelectedItem.ToString());
				App.member.phone = phoneValue.entry.Text;
				App.member.address = addressValue.entry.Text;
				App.member.city = cityValue.entry.Text;
				App.member.postalcode = postalcodeValue.entry.Text;
				App.member.nameEmergencyContact = nameEmergencyContactValue.entry.Text;
				App.member.phoneEmergencyContact = phoneEmergencyContactValue.entry.Text;
				App.member.name_enc1 = EncEducacao1NomeValue.entry.Text;
				App.member.phone_enc1 = EncEducacao1PhoneValue.entry.Text;
				App.member.mail_enc1 = EncEducacao1MailValue.entry.Text;
				App.member.name_enc2 = EncEducacao2NomeValue.entry.Text;
				App.member.phone_enc2 = EncEducacao2PhoneValue.entry.Text;
				App.member.mail_enc2 = EncEducacao2MailValue.entry.Text;

				App.member.schoolname = schoolNameValue.entry.Text;
				App.member.schoolnumber = schoolNumberValue.entry.Text;
				App.member.schoolyear = schoolYearValue.entry.Text;
				App.member.schoolclass = schoolClassValue.entry.Text;

				App.member.comentarios = comentariosValue.entry.Text;

				MemberManager memberManager = new MemberManager();
				var result = await memberManager.UpdateMemberInfo(App.original_member.id, App.member);
				if (result == "-1")
				{
					Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
					{
						BarBackgroundColor = Color.FromRgb(15, 15, 15),
						BarTextColor = Color.White
					};
					return "-1";
				}
				return result;
			}
			return "";
		}

		void memberPhotoImageTappedAsync(object sender, System.EventArgs e)
		{
			displayMemberPhotoImageActionSheet();
		}

        void memberNameLabelTappedAsync(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new QuotasListPageCS());
        }


        async void memberPhotoImageTapped_NotAuthorized_Async(object sender, System.EventArgs e)
		{
			//DisplayAlert("Consentimento Foto Sócio", "Para poder fazer upload da sua foto, tem de dar o seu consentimento para que possamos fazer o tratamento da sua foto. Aceda a 'Consentimentos' e escolha essa opção. ", "OK");
			bool display_result = await DisplayAlert("Consentimento Foto Sócio", "Para poder fazer upload da sua foto, tem de dar o seu consentimento para que possamos fazer o tratamento da mesma. Pretende dar consentimento?", "Sim", "Não");
			if (display_result == true)
			{
				showActivityIndicator();

				MemberManager memberManager = new MemberManager();
				App.member.consentimento_fotosocio = "1";

				relativeLayout.Children.Remove(memberPhotoImage);
				CreatePhoto();

				var result = await memberManager.Update_Member_Authorizations(App.member.id, App.member.consentimento_assembleia, App.member.consentimento_regulamento, App.member.consentimento_dados, App.member.consentimento_imagem, App.member.consentimento_fotosocio, App.member.consentimento_whatsapp);
                hideActivityIndicator();
            }

		}


		async Task<string> displayMemberPhotoImageActionSheet()
		{
			var actionSheet = await DisplayActionSheet("Fotografia Sócio " + App.member.nickname, "Cancel", null, "Tirar Foto", "Galeria de Imagens");

			MemberManager memberManager = new MemberManager();
			string result = "";
			switch (actionSheet)
			{
				case "Cancel":
					break;
				case "Tirar Foto":
					TakeAPhotoTapped();
					break;
				case "Galeria de Imagens":
					OpenGalleryTapped();
					break;
			}

			return "";
		}

        async void OpenGalleryTapped()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Por favor escolha uma foto"
            });

            if (result != null)
            {
                Stream stream_aux = await result.OpenReadAsync();
                Stream localstream = await result.OpenReadAsync();

                memberPhotoImage.Source = ImageSource.FromStream(() => localstream);
                if (Device.RuntimePlatform == Device.iOS)
                {
                    memberPhotoImage.Rotation = 0;
                    stream = RotateBitmap(stream_aux, 0);
                }
                else
                {
                    memberPhotoImage.Rotation = 90;
                    stream = RotateBitmap(stream_aux, 90);
                }

                MemberManager memberManager = new MemberManager();
                memberManager.Upload_Member_Photo(stream);
            }
        }

        async void TakeAPhotoTapped()
        {
            var result = await MediaPicker.CapturePhotoAsync ();

            if (result != null)
            {
                Stream stream_aux = await result.OpenReadAsync();
                Stream localstream = await result.OpenReadAsync();

                memberPhotoImage.Source = ImageSource.FromStream(() => localstream);
                memberPhotoImage.Rotation = 90;
				stream = RotateBitmap(stream_aux, 90);

                MemberManager memberManager = new MemberManager();
                memberManager.Upload_Member_Photo(stream);
            }
        }

        public Stream RotateBitmap(Stream _stream, int angle)
        {
			Stream streamlocal = null;
            SKBitmap bitmap = SKBitmap.Decode(_stream);
            SKBitmap rotatedBitmap = new SKBitmap(bitmap.Height, bitmap.Width);
			if (angle != 0)
			{
				using (var surface = new SKCanvas(rotatedBitmap))
				{
					surface.Translate(rotatedBitmap.Width, 0);
					surface.RotateDegrees(angle);
					surface.DrawBitmap(bitmap, 0, 0);
				}
			}
			else
			{
				rotatedBitmap = bitmap;

                /*using (var surface = new SKCanvas(rotatedBitmap))
                {
                    surface.Translate(rotatedBitmap.Width, 0);
                    surface.DrawBitmap(bitmap, 0, 0);
                }*/
			}

            using (MemoryStream memStream = new MemoryStream())
            using (SKManagedWStream wstream = new SKManagedWStream(memStream))
            {
                rotatedBitmap.Encode(wstream, SKEncodedImageFormat.Jpeg, 40);
                byte[] data = memStream.ToArray();
                streamlocal = new MemoryStream(data);
            }
			return streamlocal;
        }

        private string SetImageFileName(string fileName = null)
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				if (fileName != null)
					App.ImageIdToSave = fileName;
				else
					App.ImageIdToSave = App.DefaultImageId;

				return App.ImageIdToSave;
			}
			else
			{
				if (fileName != null)
				{
					App.ImageIdToSave = fileName;
					return fileName;
				}
				else
					return null;
			}
		}
	}
}