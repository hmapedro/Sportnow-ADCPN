using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Essentials;

namespace SportNow.Views
{
	public class DetalheGraduacaoPageCS : DefaultPage
	{

		protected override void OnDisappearing()
		{
			this.CleanScreen();
		}

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		public void CleanScreen()
		{
			Debug.Print("CleanScreen");
			//valida se os objetos já foram criados antes de os remover
			if (gridGrade != null)
			{
				relativeLayout.Children.Remove(gridGrade);
				gridGrade = null;
			}

		}



		private StackLayout stackLayout;

		private Grid gridGrade;

		public List<MainMenuItem> MainMenuItems { get; set; }

		private Examination examination;

		public void initLayout()
		{
			Title = "Graduação";

			var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "iconshare.png",
			
			};
			toolbarItem.Clicked += OnShareButtonClicked;
			ToolbarItems.Add(toolbarItem);

		}


		public async Task initSpecificLayout()
		{

			gridGrade = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter });
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = 230 * App.screenHeightAdapter });
			
			gridGrade.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto 

			Label gradeLabel = new Label
			{
				Text = Constants.grades[examination.grade],
				TextColor = Xamarin.Forms.Color.White,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = 30 * App.screenHeightAdapter,
			};



			Image gradeImage = new Image
			{
				Source = examination.image,
			};

			var textdategrade = examination.place + " | " + examination.date + " | " + examination.examiner;
			Label dategradeLabel = new Label
			{
				Text = textdategrade,
				TextColor = Xamarin.Forms.Color.White,
				HorizontalTextAlignment = TextAlignment.Center,
				FontSize = App.titleFontSize
			};



			var browser = new WebView
			{
				//Source = "http://xamarin.com",
				HeightRequest = 230 * App.screenHeightAdapter,
				WidthRequest = 297 * App.screenHeightAdapter,
				VerticalOptions = LayoutOptions.Fill,
                //HorizontalOptions = LayoutOptions.FillAndExpand,
                //Source = "https://www.google.com"
                //Source = "https://docs.google.com/viewer?url=https://20.234.59.92/nkslcrm/upload/planta.pdf"
                //Source = "https://20.234.59.92/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id

                /*#if __ANDROID__
								Source = "https://docs.google.com/gview?url=http://20.234.59.92/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id + "&embedded=true%22"
				#else
								Source = "https://20.234.59.92/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id
				#endif*/
            };

			var pdfUrl = "https://20.234.59.92/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id;
			var androidUrl = "https://docs.google.com/gview?url=" + pdfUrl + "&embedded=true";
			Debug.Print("androidUrl=" + androidUrl);
			if (Device.RuntimePlatform == Device.iOS)
			{
				browser.Source = pdfUrl;
			}
			else if (Device.RuntimePlatform == Device.Android)
			{
				browser.Source = new UrlWebViewSource() { Url = androidUrl };
			}

			if (browser.Source == null)
			{
				Debug.Print("browser.Source = null");
			}
			else {
				Debug.Print("browser.Source != null");
			}

			gridGrade.Children.Add(gradeLabel, 0, 0);

			gridGrade.Children.Add(gradeImage, 0, 1);

			gridGrade.Children.Add(dategradeLabel, 0, 3);

			gridGrade.Children.Add(browser, 0, 4);

			List<Payment> payments = await GetExamination_Payment(examination.id);

			if (payments.Count > 0)
			{
				if ((payments[0].invoiceid != null) & (payments[0].invoiceid != ""))
				{
					Label invoiceLabel = new Label
					{
						Text = "Obter fatura",
						TextColor = Xamarin.Forms.Color.White,
						HorizontalTextAlignment = TextAlignment.Center,
						FontSize = App.titleFontSize
					};

					var invoiceLabel_tap = new TapGestureRecognizer();
					invoiceLabel_tap.Tapped += async (s, e) =>
					{
						await Navigation.PushAsync(new InvoiceDocumentPageCS(payments[0]));
					};
					invoiceLabel.GestureRecognizers.Add(invoiceLabel_tap);
					gridGrade.RowDefinitions.Add(new RowDefinition { Height = 80 * App.screenHeightAdapter });
					gridGrade.Children.Add(invoiceLabel, 0, 5);
				}
			}

			relativeLayout.Children.Add(gridGrade,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width / 2) - 143.5 * App.screenHeightAdapter; // center of image (which is 40 wide)
				}),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height); // center of image (which is 40 wide)
				})
			);

		}

		public DetalheGraduacaoPageCS(Member member, Examination examination)
		{
			NavigationPage.SetBackButtonTitle(this, "");
			this.examination = examination;
			this.initLayout();
		}

		async void OnShareButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnShareButtonClicked");
			await Share.RequestAsync(new ShareTextRequest
			{
				//Uri = "https://plataforma.nksl.org/diploma_1.jpg",
				Uri = "https://20.234.59.92/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id,
				Title = "Partilha Diploma"
			});
		}

		async Task<List<Payment>> GetExamination_Payment(string examinationid)
		{
			Debug.WriteLine("GetExamination_Payment");
			ExaminationSessionManager examination_sessionManager = new ExaminationSessionManager();

			List<Payment> payments = await examination_sessionManager.GetExamination_Payment(examinationid);
			if (payments == null)
			{
				Debug.WriteLine("GetExamination_Payment is null");
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return payments;
		}


		
	}


}

