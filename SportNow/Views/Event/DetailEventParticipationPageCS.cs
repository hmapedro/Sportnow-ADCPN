using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using System.Diagnostics;
using SportNow.CustomViews;
using Xamarin.Essentials;

namespace SportNow.Views
{
	public class DetailEventParticipationPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			gridEvent = null;
			//App.competition_participation = competition_participation;

		}


		FormValue estadoValue = new FormValue("");



		private Event_Participation event_participation;

		private List<Payment> payments;

		Button registerButton = new Button();

		private Grid gridEvent;

		public void initLayout()
		{
			Title = event_participation.evento_name;
		}


		public async void initSpecificLayout()
		{
			gridEvent = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridEvent.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			gridEvent.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label dateLabel = new FormLabel { Text = "DATA" };
			FormValue dateValue = new FormValue(event_participation.evento_detailed_date);

			FormLabel placeLabel = new FormLabel { Text = "LOCAL" };
			FormValue placeValue = new FormValue(event_participation.evento_local);

			FormLabel typeLabel = new FormLabel { Text = "TIPO" };
			FormValue typeValue = new FormValue(Constants.event_type[event_participation.evento_tipo]);

			FormLabel websiteLabel = new FormLabel { Text = "WEBSITE" };
			FormValue websiteValue = new FormValue(event_participation.evento_website);


			websiteValue.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async () => {
					try
					{
						await Browser.OpenAsync(event_participation.evento_website, BrowserLaunchMode.SystemPreferred);
					}
					catch (Exception ex)
					{
						// An unexpected error occured. No browser may be installed on the device.
					}
				})
			});


			Image eventoImage = new Image { Aspect = Aspect.AspectFill, Opacity = 0.25 };
			eventoImage.Source = event_participation.imagemSource;

			relativeLayout.Children.Add(eventoImage,
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

			gridEvent.Children.Add(dateLabel, 0, 0);
			gridEvent.Children.Add(dateValue, 1, 0);

			gridEvent.Children.Add(placeLabel, 0, 1);
			gridEvent.Children.Add(placeValue, 1, 1);

			gridEvent.Children.Add(typeLabel, 0, 2);
			gridEvent.Children.Add(typeValue, 1, 2);

			gridEvent.Children.Add(websiteLabel, 0, 3);
			gridEvent.Children.Add(websiteValue, 1, 3);


			relativeLayout.Children.Add(gridEvent,
				xConstraint: Constraint.Constant(0),
				yConstraint: Constraint.Constant(0),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - 10); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Height); // center of image (which is 40 wide)
				})
			);
		}



		public DetailEventParticipationPageCS(Event_Participation event_participation)
		{
			this.event_participation = event_participation;
			//App.event_participation = event_participation;

			this.initLayout();
			//this.initSpecificLayout();
		}

	}
}

