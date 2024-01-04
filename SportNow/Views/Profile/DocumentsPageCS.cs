using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
//Ausing Acr.UserDialogs;
using Xamarin.Essentials;
using SportNow.Services.Camera;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;
using SportNow.Views.Profile;
using static System.Net.WebRequestMethods;
using SkiaSharp;
using SportNow.Views.CompleteRegistration;

namespace SportNow.Views.Profile
{
	public class DocumentsPageCS : DefaultPage
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
		RegisterButton confirmDocumentsButton;
		Image loadedDocument;
		ImageSource source;
		Stream streamSource;

		Picker documentTypePicker;

		Stream stream;


        private CollectionView documentsCollectionView;

		public void initLayout()
		{
			Title = "DOCUMENTOS";
		}


		public async void initSpecificLayout()
		{

			createDocumentsCollection();


			Label titleLabel = new Label { Text = "ESCOLHA O TIPO DE DOCUMENTO E FAÇA UPLOAD VIA GALERIA OU CÂMARA:", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.formLabelFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };

			relativeLayout.Children.Add(titleLabel,
				xConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant((App.ItemHeight - (20 * App.screenHeightAdapter)) + (100 * App.screenHeightAdapter)),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (0 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
			);

			createDocumentTypePicker();

			Image imageGallery = new Image
			{
				Source = "iconabrirgaleria.png",
				HorizontalOptions = LayoutOptions.Center,
			};

			var imageGallery_tap = new TapGestureRecognizer();
			imageGallery_tap.Tapped += OpenGalleryTapped;
			imageGallery.GestureRecognizers.Add(imageGallery_tap);

			relativeLayout.Children.Add(imageGallery,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width / 2) - (80 * App.screenHeightAdapter));
				}),
				yConstraint: Constraint.Constant((App.ItemHeight - (20 * App.screenHeightAdapter)) + (180 * App.screenHeightAdapter)),
				widthConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter)
			);

			Image imagePhoto = new Image
			{
				Source = "icontirarfoto.png",
				HorizontalOptions = LayoutOptions.Center,
			};
			var imagePhoto_tap = new TapGestureRecognizer();
			imagePhoto_tap.Tapped += TakeAPhotoTapped;
			imagePhoto.GestureRecognizers.Add(imagePhoto_tap);

			relativeLayout.Children.Add(imagePhoto,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width / 2) + (20 * App.screenHeightAdapter));
				}),
				yConstraint: Constraint.Constant((App.ItemHeight - (20 * App.screenHeightAdapter)) + (180 * App.screenHeightAdapter)),
				widthConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(60 * App.screenHeightAdapter)
			);

			Label termoResponsabilidadeLabel = new Label { Text = "Fazer download do formulário do Termo de Responsabilidade", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = 15 * App.screenWidthAdapter, TextColor = App.topColor, LineBreakMode = LineBreakMode.NoWrap };

			var termoResponsabilidadeLabel_tap = new TapGestureRecognizer();
			termoResponsabilidadeLabel_tap.Tapped += async (s, e) =>
			{
				try
				{
					await Browser.OpenAsync("https://www.adcpn.pt/wp-content/uploads/2022/08/ADCPN_TermoResponsabilidade.pdf", BrowserLaunchMode.SystemPreferred);
				}
				catch (Exception ex)
				{
				}
			};
			termoResponsabilidadeLabel.GestureRecognizers.Add(termoResponsabilidadeLabel_tap);

			/*relativeLayout.Children.Add(termoResponsabilidadeLabel,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant((App.ItemHeight - (20 * App.screenHeightAdapter)) + (225 * App.screenHeightAdapter)),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (40 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);*/


			Label atestadoLabel = new Label { Text = "Fazer download do formulário do Atestado Médico", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = 15 * App.screenWidthAdapter, TextColor = App.topColor, LineBreakMode = LineBreakMode.NoWrap };

			var atestadoLabel_tap = new TapGestureRecognizer();
			atestadoLabel_tap.Tapped += async (s, e) =>
			{
				try
				{
					await Browser.OpenAsync("https://www.adcpn.pt/wp-content/uploads/2019/05/Exame-Medico-Desportivo.pdf", BrowserLaunchMode.SystemPreferred);
				}
				catch (Exception ex)
				{
				}
			};
			atestadoLabel.GestureRecognizers.Add(atestadoLabel_tap);


			/*relativeLayout.Children.Add(atestadoLabel,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant((App.ItemHeight - (20 * App.screenHeightAdapter)) + (255 * App.screenHeightAdapter)),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (40 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);*/

			confirmDocumentsButton = new RegisterButton("GRAVAR", 100, 50);
			confirmDocumentsButton.button.Clicked += confirmDocumentsButtonClicked;

            loadedDocument = new Image() { };
            relativeLayout.Children.Add(loadedDocument,
                xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
                yConstraint: Constraint.Constant(415 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return ((parent.Width) - (40 * App.screenHeightAdapter));
                }),
                heightConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return ((parent.Height) - (520 * App.screenHeightAdapter));
                })
            );

        }

		public async void createDocumentTypePicker()
		{
			List<string> tipoDocumentoList = new List<string> { "Termo de Responsabilidade", "Atestado Médico" };
			int selectedIndex = 0;
			int selectedIndex_temp = 0;

			documentTypePicker = new Picker
			{
				Title = "",
				TitleColor = Color.Black,
				BackgroundColor = Color.Transparent,
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.formLabelFontSize

            };
			documentTypePicker.ItemsSource = tipoDocumentoList;
			documentTypePicker.SelectedIndex = selectedIndex;

			documentTypePicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{
				Debug.Print("documentTypePicker selectedItem = " + documentTypePicker.SelectedItem.ToString());
			};



			relativeLayout.Children.Add(documentTypePicker,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant((App.ItemHeight - (20 * App.screenHeightAdapter)) + (130 * App.screenHeightAdapter)),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(50));
		}


		public async void createDocumentsCollection()
		{

            Label documentsLabel = new Label { Text = "DOCUMENTOS SUBMETIDOS:", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.formLabelFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.NoWrap };

			relativeLayout.Children.Add(documentsLabel,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(15 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (40 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);

			foreach (Document document in App.member.documents)
			{
				document.imagesourceObject = new UriImageSource
				{
					Uri = new Uri(Constants.images_URL + document.imagesource),
					CachingEnabled = true,
					CacheValidity = new TimeSpan(5, 0, 0, 0)
				};
				Debug.Print("document.imagesource = " + document.imagesource);
				Debug.Print("document.imagesourceObject = " + Constants.images_URL + document.imagesource);
				if (document.status== "Aprovado")
				{
					document.statusimage = "iconcheck.png";
				}
				else if (document.status == "Under Review")
				{
					document.statusimage = "iconporconfirmar.png";
				}
				else
				{
					document.statusimage = "iconinativo.png";
				}

				if (document.type == "termo_responsabilidade")
				{
					document.typeString = "Termo de Responsabilidade";
				}
				else if (document.type == "atestado_medico")
				{
					document.typeString = "Atestado Médico";
				}
			}

			//COLLECTION Classes
			documentsCollectionView = new CollectionView
			{
				SelectionMode = SelectionMode.Multiple,
				ItemsSource = App.member.documents,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
				ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Horizontal) { VerticalItemSpacing = 5 * App.screenHeightAdapter, HorizontalItemSpacing = 10 * App.screenWidthAdapter, },
				EmptyView = new ContentView
				{
					Content = new StackLayout
					{
						Children =
							{
								new Label { Text = "Não existem aulas agendadas.", HorizontalTextAlignment = TextAlignment.Start, TextColor = Color.White, FontSize = App.itemTitleFontSize },
							}
					}
				}
			};

			documentsCollectionView.SelectionChanged += OnDocumentsCollectionViewSelectionChanged;

			documentsCollectionView.ItemTemplate = new DataTemplate(() =>
			{
				RelativeLayout itemRelativeLayout = new RelativeLayout
				{
					HeightRequest = (App.ItemHeight - (20 * App.screenHeightAdapter)),
					WidthRequest = App.ItemWidth,
				};

				Frame itemFrame = new Frame
				{
					CornerRadius = 5 * (float)App.screenHeightAdapter,
					IsClippedToBounds = true,
					BorderColor = Color.Transparent,
					BackgroundColor = Color.White,
					Padding = new Thickness(0, 0, 0, 0),
					HeightRequest = (App.ItemHeight - (20 * App.screenHeightAdapter)),// -(10 * App.screenHeightAdapter),
					VerticalOptions = LayoutOptions.Center,
					HasShadow = false
				};

				Image eventoImage = new Image
				{
					Aspect = Aspect.AspectFill,
					BackgroundColor = Color.White,
					//Opacity = 0.4,
				};
				eventoImage.SetBinding(Image.SourceProperty, "imagesourceObject");

				itemFrame.Content = eventoImage;

				/*var itemFrame_tap = new TapGestureRecognizer();
				itemFrame_tap.Tapped += (s, e) =>
				{
					Navigation.PushAsync(new EquipamentsPageCS("protecoescintos"));
				};
				itemFrame.GestureRecognizers.Add(itemFrame_tap);*/

				itemRelativeLayout.Children.Add(itemFrame,
					xConstraint: Constraint.Constant(0),
					yConstraint: Constraint.Constant(0),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width);// - (5 * App.screenWidthAdapter));
					}),
					heightConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height - (40 * App.screenHeightAdapter));
					}));

				Label nameLabel = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTitleFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				nameLabel.SetBinding(Label.TextProperty, "typeString");

				itemRelativeLayout.Children.Add(nameLabel,
					xConstraint: Constraint.Constant(3 * App.screenWidthAdapter),
					yConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height - (40 * App.screenHeightAdapter));
					}),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (6 * App.screenWidthAdapter));
					}),
					heightConstraint: Constraint.Constant(20 * App.screenHeightAdapter));

				Label dateLabel = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.itemTextFontSize, TextColor = App.normalTextColor, LineBreakMode = LineBreakMode.WordWrap };
				dateLabel.SetBinding(Label.TextProperty, "active_date");

				itemRelativeLayout.Children.Add(dateLabel,
					xConstraint: Constraint.Constant(25 * App.screenWidthAdapter),
					yConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Height - (20 * App.screenHeightAdapter));
					}),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (50 * App.screenWidthAdapter)); // center of image (which is 40 wide)
					}),
					heightConstraint: Constraint.Constant((20 * App.screenHeightAdapter)));


				Image participationImagem = new Image { Aspect = Aspect.AspectFill }; //, HeightRequest = 60, WidthRequest = 60
				participationImagem.SetBinding(Image.SourceProperty, "statusimage");

				itemRelativeLayout.Children.Add(participationImagem,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width - (20 * App.screenHeightAdapter));
					}),
					yConstraint: Constraint.Constant(3 * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant((20 * App.screenHeightAdapter)),
					heightConstraint: Constraint.Constant((20 * App.screenHeightAdapter)));

				return itemRelativeLayout;
			});
			relativeLayout.Children.Add(documentsCollectionView,
				xConstraint: Constraint.Constant(15 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant((40 * App.screenHeightAdapter)),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (30 * App.screenHeightAdapter)); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant((App.ItemHeight - (20 * App.screenHeightAdapter)) + (50 * App.screenHeightAdapter)));


		}

		public DocumentsPageCS()
		{
			this.initLayout();
			this.initSpecificLayout();

			/*MessagingCenter.Subscribe<byte[]>(this, "documentPhoto", (args) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Debug.Print("DocumentsPageCS - BeginInvokeOnMainThread");

					if (loadedDocument != null)
					{
						relativeLayout.Children.Remove(loadedDocument);
						loadedDocument = null;
					}
					streamSource = new MemoryStream(args);
					source = ImageSource.FromStream(() => new MemoryStream(args));
					loadedDocument = new Image() { };
					relativeLayout.Children.Add(loadedDocument,
						xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
						yConstraint: Constraint.Constant(410 * App.screenHeightAdapter),
						widthConstraint: Constraint.RelativeToParent((parent) =>
						{
							return ((parent.Width) - (40 * App.screenHeightAdapter));
						}),
						heightConstraint: Constraint.RelativeToParent((parent) =>
						{
							return ((parent.Height) - (500 * App.screenHeightAdapter));
						})
					);
					loadedDocument.Source = source;

					relativeLayout.Children.Add(confirmDocumentsButton,
						xConstraint: Constraint.Constant(10),
						yConstraint: Constraint.RelativeToParent((parent) =>
						{
							return (parent.Height) - (60 * App.screenHeightAdapter); // 
						}),
						widthConstraint: Constraint.RelativeToParent((parent) =>
						{
							return (parent.Width - 20 * App.screenHeightAdapter); // center of image (which is 40 wide)
						}),
						heightConstraint: Constraint.Constant((50 * App.screenHeightAdapter))
					);
				});

			});*/
		}


        async void confirmDocumentsButtonClicked(object sender, EventArgs e)
        {
            Debug.Print("confirmDocumentsButtonClicked");


            //if (documentSubmitted == false)
            //{
            if (stream != null)
            {
                MemberManager memberManager = new MemberManager();

                string documentname = "";
                string type = "";

                Debug.Print("App.member.aulatipo = " + App.member.aulatipo);

                if (documentTypePicker.SelectedItem.ToString() == "Termo de Responsabilidade")
                {
                    documentname = "Termo de Responsabilidade - " + DateTime.Now.ToString("yyyy") + " - " + App.member.name;
                    type = "termo_responsabilidade";

                    if (App.member.aulatipo == "acro")
                    {
                        await DisplayAlert("Atestado Médico necessário", "O Atestado Médico é obrigatório para sócios que pretendem frequentar as aulas de ACRO. Pode prosseguir o seu processo de inscrição mas o mesmo irá ficar pendente até que submeta esse documento.", "OK");
                    }
                }
                else if (documentTypePicker.SelectedItem.ToString() == "Atestado Médico")
                {
                    documentname = "Atestado Médico - " + DateTime.Now.ToString("yyyy") + " - " + App.member.name;
                    type = "atestado_medico";
                }
                string filename = documentname + ".png";
                string status = "Under Review";
                string startdate = DateTime.Now.ToString("yyyy-MM-dd");// "2022-07-22";

                string enddate = "";// DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                if (DateTime.Now.Month < 8)
                {
                    enddate = DateTime.Now.Year + "-08-31";
                }
                else
                {
                    enddate = DateTime.Now.AddYears(1).Year + "-08-31";
                }

                showActivityIndicator();
                //documentSubmitted = true;
                _ = await memberManager.Upload_Member_Document(stream, App.member.id, filename, documentname, status, type, startdate, enddate);
                App.member.documents = await memberManager.Get_Member_Documents(App.member.id);
                await Navigation.PopAsync();
                hideActivityIndicator();


            }
            else
            {
                await DisplayAlert("Documento não submetido", "Para continuar tem de submeter um documento.", "OK");

                //UserDialogs.Instance.Alert(new AlertConfig() { Title = "Documento não submetido", Message = "Para continuar tem de submeter um documento.", OkText = "Ok" });
                //await Navigation.PushAsync(new CompleteRegistration_Profile_PageCS());
            }
            //}
            /*else
			{
				await Navigation.PushAsync(new CompleteRegistration_Profile_PageCS());
			}*/




        }

        /*async void confirmDocumentsButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("confirmDocumentsButtonClicked");

			showActivityIndicator();

			if (stream != null)
			{
				MemberManager memberManager = new MemberManager();

				string documentname = "";
				string type = "";

				Debug.Print("App.member.aulatipo = " + App.member.aulatipo);

				if (documentTypePicker.SelectedItem.ToString() == "Termo de Responsabilidade")
				{
					documentname = "Termo de Responsabilidade - " + DateTime.Now.ToString("yyyy") + " - " + App.member.name;
					type = "termo_responsabilidade";

					if (App.member.aulatipo == "acro")
					{
						await DisplayAlert("Atestado Médico necessário", "o Atestado Médico é obrigatório para sócios que pretendem frequentar as aulas de ACRO. Pode prosseguir o seu processo de inscrição mas o mesmo irá ficar pendente até que submeta esse documento.", "OK");
					}
				}
				else if (documentTypePicker.SelectedItem.ToString() == "Atestado Médico")
				{
					documentname = "Atestado Médico - " + DateTime.Now.ToString("yyyy") + " - " + App.member.name;
					type = "atestado_medico";
				}
				string filename = documentname + ".jpg";
				string status = "Under Review";
				string startdate = DateTime.Now.ToString("yyyy-MM-dd");// "2022-07-22";
				string enddate = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");

				_= await memberManager.Upload_Member_Document(stream, App.member.id, filename, documentname, status, type, startdate, enddate);
				await Navigation.PopAsync();


			}
			else
			{

                await DisplayAlert("Documento não submetido", "Para continuar tem de submeter um documento.", "OK");
//                UserDialogs.Instance.Alert(new AlertConfig() { Title = "Documento não submetido", Message = "Para continuar tem de submeter um documento.", OkText = "Ok" });
			}
            hideActivityIndicator();


        }*/

		/*
		void ImageTapped(object sender, System.EventArgs e)
		{
			LoadFromStream((sender as Image).Source);
		}

		private async void LoadFromStream(ImageSource source)
		{
			await Navigation.PushAsync(new SfImageEditorPage() { ImageSource = source });
		}*/





        /*void TakeAPhotoTapped(object sender, System.EventArgs e)
		{
			TakePhotoAsync();
            
			Device.BeginInvokeOnMainThread(() =>
			{
				var fileName = "documentPhoto";//SetImageFileName();
				DependencyService.Get<CameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
			});
        }

		void OpenGalleryTapped(object sender, System.EventArgs e)
		{
			Debug.Print("OpenGalleryTapped");
			Device.BeginInvokeOnMainThread(() =>
			{
				var fileName = "documentPhoto";//SetImageFileName();
				Debug.Print("OpenGalleryTapped fileName = " + fileName);
				Debug.Print("OpenGalleryTapped FileFormatEnum.JPEG = " + FileFormatEnum.JPEG);
				try
				{
					CameraInterface cameraInterface = DependencyService.Get<CameraInterface>();
					//cameraInterface = new DependencyService.Get<CameraInterface>();
					cameraInterface.LaunchGallery(FileFormatEnum.JPEG, fileName);
					//DependencyService.Get<CameraInterface>().LaunchGallery(FileFormatEnum.JPEG, "teste.jpeg");
				}
				catch (Exception ex)
				{
					Debug.Print(ex.Message);
					Debug.Print(ex.StackTrace);
				}
				//loadedDocument.Source = fileName;
			});
		}*/

        async void OpenGalleryTapped(System.Object sender, System.EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Por favor escolha uma foto"
            });

            if (result != null)
            {
                Stream stream_aux = await result.OpenReadAsync();
                Stream localstream = await result.OpenReadAsync();

                loadedDocument.Source = ImageSource.FromStream(() => localstream);
                if (Device.RuntimePlatform == Device.iOS)
                {
                    loadedDocument.Rotation = 0;
                    stream = RotateBitmap(stream_aux, 0);
                }
                else
                {
                    loadedDocument.Rotation = 90;
                    stream = RotateBitmap(stream_aux, 90);
                }

                //MemberManager memberManager = new MemberManager();
                //await memberManager.Upload_Member_Photo(stream);
                showConfirmDocumentsButton();
            }
        }

        async void TakeAPhotoTapped(System.Object sender, System.EventArgs e)
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                Stream stream_aux = await result.OpenReadAsync();
                Stream localstream = await result.OpenReadAsync();

                loadedDocument.Source = ImageSource.FromStream(() => localstream);
                loadedDocument.Rotation = 90;
                stream = RotateBitmap(stream_aux, 90);

                //MemberManager memberManager = new MemberManager();
                //await memberManager.Upload_Member_Photo(stream);
                showConfirmDocumentsButton();
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

        public void showConfirmDocumentsButton()
		{
            relativeLayout.Children.Add(confirmDocumentsButton,
				xConstraint: Constraint.Constant(10),
				yConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height) - (60 * App.screenHeightAdapter); // 
				}),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 20 * App.screenHeightAdapter); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant((50 * App.screenHeightAdapter))
			);
        }

        async void OnDocumentsCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			showActivityIndicator();
			Debug.WriteLine("DocumentsPageCS.OnDocumentsCollectionViewSelectionChanged");

			if ((sender as CollectionView).SelectedItems.Count != 0)
			{
				Document document = (sender as CollectionView).SelectedItems[0] as Document;
				await Navigation.PushAsync(new DocumentsDetailPageCS(document));

				((CollectionView)sender).SelectedItems.Clear();
                hideActivityIndicator();
            }
		}



    }

}