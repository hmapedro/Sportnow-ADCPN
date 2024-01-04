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
	public class DetailCompetitionResultPageCS : DefaultPage
	{

		protected override void OnAppearing()
		{
			initSpecificLayout();
		}

		protected override void OnDisappearing()
		{
			gridCompetiton = null;
			//App.competition_participation = competition_participation;

		}


		FormValue estadoValue = new FormValue("");



		private Competition_Participation competition_participation;

		private List<Payment> payments;

		Button registerButton = new Button();

		private Grid gridCompetiton;

		public void initLayout()
		{
			Title = competition_participation.competicao_name;

			/*var toolbarItem = new ToolbarItem
			{
				//Text = "Logout",
				IconImageSource = "perfil.png",

			};
			toolbarItem.Clicked += OnPerfilButtonClicked;
			ToolbarItems.Add(toolbarItem);*/

		}


		public async void initSpecificLayout()
		{
			gridCompetiton = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand };
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridCompetiton.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			gridCompetiton.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			Label dateLabel = new FormLabel { Text = "DATA" };
			FormValue dateValue = new FormValue(competition_participation.competicao_detailed_date);

			FormLabel placeLabel = new FormLabel { Text = "LOCAL" };
			FormValue placeValue = new FormValue(competition_participation.competicao_local);

			FormLabel typeLabel = new FormLabel { Text = "TIPO" };
			FormValue typeValue = new FormValue(Constants.competition_type[competition_participation.competicao_tipo]);

			FormLabel websiteLabel = new FormLabel { Text = "WEBSITE" };
			FormValue websiteValue = new FormValue(competition_participation.competicao_website);


			websiteValue.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(async () => {
					try
					{
						await Browser.OpenAsync(competition_participation.competicao_website, BrowserLaunchMode.SystemPreferred);
					}
					catch (Exception ex)
					{
						// An unexpected error occured. No browser may be installed on the device.
					}
				})
			});

			FormLabel provaLabel = new FormLabel { Text = "PROVA" }; ;
			FormValue provaValue = new FormValue(competition_participation.categoria);

			FormLabel classificacaoLabel = new FormLabel { Text = "RESULTADO" }; ;
			FormValue classificacaoValue = new FormValue(competition_participation.classificacao);
			classificacaoValue.Padding = new Thickness(1, 1, 1, 1);
			//classificacaoValue.BackgroundColor = competition_participation.classificacaoColor;
			classificacaoValue.label.BackgroundColor = competition_participation.classificacaoColor;

			gridCompetiton.Children.Add(dateLabel, 0, 0);
			gridCompetiton.Children.Add(dateValue, 1, 0);

			gridCompetiton.Children.Add(placeLabel, 0, 1);
			gridCompetiton.Children.Add(placeValue, 1, 1);

			gridCompetiton.Children.Add(typeLabel, 0, 2);
			gridCompetiton.Children.Add(typeValue, 1, 2);

			gridCompetiton.Children.Add(websiteLabel, 0, 3);
			gridCompetiton.Children.Add(websiteValue, 1, 3);

			gridCompetiton.Children.Add(provaLabel, 0, 4);
			gridCompetiton.Children.Add(provaValue, 1, 4);

			gridCompetiton.Children.Add(classificacaoLabel, 0, 5);
			gridCompetiton.Children.Add(classificacaoValue, 1, 5);

			relativeLayout.Children.Add(gridCompetiton,
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



		public DetailCompetitionResultPageCS(Competition_Participation competition_participation)
		{
			this.competition_participation = competition_participation;
			App.competition_participation = competition_participation;

			this.initLayout();
			//this.initSpecificLayout();
		}


		async void OnRegisterButtonClicked(object sender, EventArgs e)
		{

			/*registerButton.IsEnabled = false;

			if (competition_participation != null)
			{
				Debug.WriteLine("OnRegisterButtonClicked competition_participation.name " + competition_participation.name);

                await Navigation.PushAsync(new CompetitionMBPageCS(competition_participation));

			}*/

		}




	}
}

