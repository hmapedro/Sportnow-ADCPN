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

namespace SportNow.Views.Profile
{
	public class DocumentsDetailPageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{
		}

		//Image estadoQuotaImage;


		public Document document;

		public void initLayout()
		{
			Title = "DETALHE DOCUMENTO";

			relativeLayout = new RelativeLayout
			{
				Margin = new Thickness(10)
			};
			Content = relativeLayout;

		}


		public async void initSpecificLayout()
		{

			Image documentImage = new Image
			{
				Source =  (ImageSource) this.document.imagesourceObject,
				HorizontalOptions = LayoutOptions.Center,
			};


			relativeLayout.Children.Add(documentImage,
				xConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (20 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Height) - (20 * App.screenHeightAdapter));
				})
			);

		}

		public DocumentsDetailPageCS(Document document)
		{
			this.document = document;
			this.initLayout();
			this.initSpecificLayout();

			
		}

	}

}