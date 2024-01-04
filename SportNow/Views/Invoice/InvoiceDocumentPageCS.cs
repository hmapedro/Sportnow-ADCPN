using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Essentials;
//Ausing Acr.UserDialogs;

namespace SportNow.Views
{
	public class InvoiceDocumentPageCS : DefaultPage
	{

		private StackLayout stackLayout;

		private Grid gridGrade;

		public List<MainMenuItem> MainMenuItems { get; set; }

		private Payment payment;

		public void initLayout()
		{
			Title = "Fatura";

			var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "iconshare.png",
			
			};
			toolbarItem.Clicked += OnShareButtonClicked;
			ToolbarItems.Add(toolbarItem);

		}


		public void initSpecificLayout()
		{

			gridGrade= new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridGrade.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star});
			gridGrade.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto 

			var browser = new WebView
			{
				BackgroundColor = Color.FromRgb(25, 25, 25),
				HeightRequest = 300 * App.screenWidthAdapter,
				WidthRequest = 440 * App.screenWidthAdapter,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.Fill,
			};

			browser.Navigating += OnNavigating;
			browser.Navigated += OnNavigated;

			var pdfUrl = Constants.RestUrl_Get_Invoice_byID + "?invoiceid=" + payment.invoiceid;
			Debug.Print("InvoiceDocumentPageCS - initSpecificLayout - pdfUrl = " + pdfUrl);
			var androidUrl = "https://docs.google.com/gview?url=" + pdfUrl + "&embedded=true";
			Debug.Print("pdfUrl=" + pdfUrl);
			Debug.Print("androidUrl="+androidUrl);
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

			gridGrade.Children.Add(browser, 0, 0);

			relativeLayout.Children.Add(gridGrade,
				xConstraint: Constraint.Constant(0),
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

            /*Image diplomaImage = new Image
			{
				Source = new UriImageSource
				{
					Uri = new Uri("https://20.234.59.92/services/PDF/create_PDF_diploma_ByID.php?exameid=" + examination.id),
					CachingEnabled = false,
					CacheValidity = new TimeSpan(5, 0, 0, 0)
				}
			};*/

        }

        public InvoiceDocumentPageCS(Payment payment)
		{
			this.payment = payment;
			this.initLayout();
			this.initSpecificLayout();
		}

		public void OnNavigating(object sender, WebNavigatingEventArgs e)
		{
			showActivityIndicator();
		}

		public void OnNavigated(object sender, WebNavigatedEventArgs e)
		{
            hideActivityIndicator();
        }

		async void OnShareButtonClicked(object sender, EventArgs e)
		{
			Debug.WriteLine("OnShareButtonClicked");
			await Share.RequestAsync(new ShareTextRequest
			{
				//Uri = "https://plataforma.nksl.org/diploma_1.jpg",
				Uri = Constants.RestUrl_Get_Invoice_byID + "?invoiceid=" + payment.invoiceid,
				Title = "Partilha Fatura"
			});
		}

	}


}

