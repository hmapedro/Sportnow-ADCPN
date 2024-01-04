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
using SkiaSharp;

namespace SportNow.Views.CompleteRegistration
{
	public class CompleteRegistration_Documents_PageCS : DefaultPage
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

		bool documentSubmitted;

		Picker documentTypePicker;

		Stream stream;

		public void initLayout()
		{
			Title = "DOCUMENTOS";
		}

		public async void initSpecificLayout()
		{
			Frame backgroundFrame = new Frame
			{
				CornerRadius = 10,
				IsClippedToBounds = true,
				BackgroundColor = Color.FromRgb(240, 240, 240),
				HasShadow = false
			};

			relativeLayout.Children.Add(backgroundFrame,
				xConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(5 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (20 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Height) - (70 * App.screenHeightAdapter));
				})
			);


			Label titleLabel = new Label { Text = "ESCOLHA O TIPO DE DOCUMENTO E FAÇA UPLOAD VIA GALERIA OU CÂMARA:", VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.formLabelFontSize, TextColor = Color.FromRgb(100, 100, 100), LineBreakMode = LineBreakMode.WordWrap };

			relativeLayout.Children.Add(titleLabel,
				xConstraint: Constraint.Constant(15 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (30 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter)
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
					return ((parent.Width / 2) - (110 * App.screenHeightAdapter));
				}),
				yConstraint: Constraint.Constant(120 * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(100 * App.screenHeightAdapter)
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
					return ((parent.Width / 2) + (10 * App.screenHeightAdapter));
				}),
				yConstraint: Constraint.Constant(120 * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(100 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(100 * App.screenHeightAdapter)
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
				yConstraint: Constraint.Constant(240 * App.screenHeightAdapter),
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
				yConstraint: Constraint.Constant(270 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (40 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);*/

			confirmDocumentsButton = new RegisterButton("CONTINUAR", 100, 50);
			confirmDocumentsButton.button.Clicked += confirmDocumentsButtonClicked;

			/*relativeLayout.Children.Add(confirmDocumentsButton,
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
			);*/
		}

		public async void createDocumentTypePicker()
		{
			List<string> tipoDocumentoList = new List<string> { "Escolha o tipo de documento", "Termo de Responsabilidade", "Atestado Médico" };
			int selectedIndex = 0;
			int selectedIndex_temp = 0;

			documentTypePicker = new Picker
			{
				Title = "",
				TitleColor = Color.Black,
				BackgroundColor = Color.Transparent,
				TextColor = App.topColor,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.titleFontSize

			};
			documentTypePicker.ItemsSource = tipoDocumentoList;
			documentTypePicker.SelectedIndex = selectedIndex;

			documentTypePicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{
				Debug.Print("documentTypePicker selectedItem = " + documentTypePicker.SelectedItem.ToString());
			};

			relativeLayout.Children.Add(documentTypePicker,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(50 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (40 * App.screenHeightAdapter)); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(50));
		}


		public CompleteRegistration_Documents_PageCS()
		{
			documentSubmitted = false;
			this.initLayout();
			this.initSpecificLayout();

            loadedDocument = new Image() { };
            relativeLayout.Children.Add(loadedDocument,
                xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
                yConstraint: Constraint.Constant(300 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return ((parent.Width) - (40 * App.screenHeightAdapter));
                }),
                heightConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return ((parent.Height) - (390 * App.screenHeightAdapter));
                })
            );

            /*MessagingCenter.Subscribe<byte[]>(this, "documentPhoto", (args) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					documentSubmitted = false;
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
						yConstraint: Constraint.Constant(300 * App.screenHeightAdapter),
						widthConstraint: Constraint.RelativeToParent((parent) =>
						{
							return ((parent.Width) - (40 * App.screenHeightAdapter));
						}),
						heightConstraint: Constraint.RelativeToParent((parent) =>
						{
							return ((parent.Height) - (390 * App.screenHeightAdapter));
						})
					);
					loadedDocument.Source = source;
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

					if (documentTypePicker.SelectedIndex == 0)
					{
						await DisplayAlert("Escolher o tipo de documento", "Deve escolher um tipo de documento para poder prosseguir.", "OK");
						return;
					}
                    else if (documentTypePicker.SelectedIndex == 1)
                    {
						documentname = "Termo de Responsabilidade - " + DateTime.Now.ToString("yyyy") + " - " + App.member.name;
						type = "termo_responsabilidade";

						if (App.member.aulatipo == "acro")
						{
							//bool answer = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");

							await DisplayAlert("Atestado Médico necessário", "O Atestado Médico é obrigatório para sócios que pretendem frequentar a classe de ACRO. Pode prosseguir o seu processo de inscrição mas o mesmo irá ficar pendente até que submeta esse documento.", "OK");
						}
					}
					else if (documentTypePicker.SelectedIndex == 2)
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
					await Navigation.PushAsync(new CompleteRegistration_Profile_PageCS());
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

        /*void ImageTapped(object sender, System.EventArgs e)
		{
			LoadFromStream((sender as Image).Source);
		}

		private async void LoadFromStream(ImageSource source)
		{
			await Navigation.PushAsync(new SfImageEditorPage() { ImageSource = source });
		}

		void TakeAPhotoTapped(object sender, System.EventArgs e)
		{

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
					System.Diagnostics.Debug.Print(ex.Message);
					System.Diagnostics.Debug.Print(ex.StackTrace);
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
                //documentSubmitted = true;
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
				//documentSubmitted = true;
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
    }

}