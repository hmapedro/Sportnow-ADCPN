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
using Newtonsoft.Json.Linq;
//using Syncfusion.XForms.MaskedEdit;

namespace SportNow.Views.Profile
{
    public class NewMemberPageCS : DefaultPage
	{
		private Member member;

		private Grid gridGeral;

        FormValueEdit nameValue;
		FormValueEdit emailValue;
        FormValueEdit nifValue;
        FormValueEditDate birthdateValue;
        FormValueEditPicker tipoSocioValue;
        FormValueEditPicker federadoValue;

        RoundButton confirmButton;

        Button activateButton;

        protected async override void OnAppearing()
        {
            Debug.Print("NewMemberPageCS.OnAppearing");
        }

        protected async override void OnDisappearing()
        {
            Debug.Print("NewMemberPageCS.OnDisappearing");			
        }

        public void initLayout()
		{
			Title = "NOVO SÓCIO";
		}


		public async Task<string> initSpecificLayout()
		{
			Debug.Print("NewMemberPageCS - initSpecificLayout");

            relativeLayout = new RelativeLayout
            {
                Margin = new Thickness(10)
            };
            Content = relativeLayout;

			CreateGridGeral();
            CreateConfirmButton();

            return "";
		}

		public void CreateGridGeral() {


            Image logo_adcpn = new Image
            {
                Source = "logo_login.png",
                HorizontalOptions = LayoutOptions.Center,
            };
            relativeLayout.Children.Add(logo_adcpn,
                xConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return ((parent.Width / 2) - (70 * App.screenHeightAdapter));
                }),
                yConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
                widthConstraint: Constraint.Constant(140 * App.screenHeightAdapter)
            );

            gridGeral = new Grid { Padding = 0, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
			//gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			gridGeral.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); //GridLength.Auto
			gridGeral.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); //GridLength.Auto 

			FormLabel nameLabel = new FormLabel { Text = "NOME", HorizontalTextAlignment = TextAlignment.Start };
			nameValue = new FormValueEdit("", Keyboard.Text);

            FormLabel nifLabel = new FormLabel { Text = "NIF" };
			nifValue = new FormValueEdit("", Keyboard.Text);

            FormLabel emailLabel = new FormLabel { Text = "EMAIL" };
            emailValue = new FormValueEdit("", Keyboard.Email);

            FormLabel birthdateLabel = new FormLabel { Text = "NASCIMENTO" };
			birthdateValue = new FormValueEditDate("");

            List<string> tipoSocioList = new List<string>();
			foreach (KeyValuePair<string, string> entry in Constants.member_type)
			{
                tipoSocioList.Add(entry.Value);
			}

			FormLabel tipoSocioLabel = new FormLabel { Text = "TIPO SÓCIO" };
            tipoSocioValue = new FormValueEditPicker("Praticante", tipoSocioList);


            List<string> federadoList = new List<string>();
            federadoList.Add("Não");
			federadoList.Add("Sim");
            
            FormLabel federadoLabel = new FormLabel { Text = "FEDERADO" };
            federadoValue = new FormValueEditPicker("Não", federadoList);

            gridGeral.Children.Add(nameLabel, 0, 0);
			gridGeral.Children.Add(nameValue, 1, 0);

			gridGeral.Children.Add(nifLabel, 0, 1);
			gridGeral.Children.Add(nifValue, 1, 1);

            gridGeral.Children.Add(emailLabel, 0, 2);
            gridGeral.Children.Add(emailValue, 1, 2);

            gridGeral.Children.Add(birthdateLabel, 0, 3);
			gridGeral.Children.Add(birthdateValue, 1, 3);

			gridGeral.Children.Add(tipoSocioLabel, 0, 4);
			gridGeral.Children.Add(tipoSocioValue, 1, 4);

            gridGeral.Children.Add(federadoLabel, 0, 5);
            gridGeral.Children.Add(federadoValue, 1, 5);

            relativeLayout.Children.Add(gridGeral,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(160 * App.screenHeightAdapter),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width); // center of image (which is 40 wide)
                }),
                heightConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height - (160 * App.screenHeightAdapter)); // center of image (which is 40 wide)
                })
            );
        }

        public void CreateConfirmButton()
        {

            confirmButton = new RoundButton("CRIAR NOVO SÓCIO", 100, 40);
            confirmButton.button.Clicked += OnConfirmButtonClicked;

            relativeLayout.Children.Add(confirmButton,
                xConstraint: Constraint.Constant(10),
                yConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Height) - (60 * App.screenHeightAdapter); // 
                }),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return (parent.Width - 20 * App.screenHeightAdapter);
                }),
                heightConstraint: Constraint.Constant((50 * App.screenHeightAdapter))
            );
        }


        public NewMemberPageCS()
        {
            this.initLayout();
            this.initSpecificLayout();

        }

        async void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnConfirmButtonClicked");
            confirmButton.button.IsEnabled = false;
            CreateNewMember();
            confirmButton.button.IsEnabled = true;
        }

        async Task<string> CreateNewMember()
        {
            Debug.Print("CreateNewMember");

            if ((nameValue.entry.Text == "") | (App.CountWords(nameValue.entry.Text) < 2))
            {
                await DisplayAlert("DADOS INVÁLIDOS", "O nome introduzido não é válido.", "OK");
                return "-1";
            }

            Debug.Print("App.IsValidContrib(nifValue.entry.Text) = " + App.IsValidContrib(nifValue.entry.Text));
            if (App.IsValidContrib(nifValue.entry.Text) == false)
            {
                await DisplayAlert("DADOS INVÁLIDOS", "O número de identificação fiscal introduzido não é válido.", "Ok");
                return "-1";
            }

            if (!Regex.IsMatch(emailValue.entry.Text, @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$"))
            {
                await DisplayAlert("DADOS INVÁLIDOS", "O email introduzido não é válido.", "Ok");
                return "-1";
            }

            if (!Regex.IsMatch(birthdateValue.entry.Text, @"^\d{4}$|^\d{4}-((0?\d)|(1[012]))-(((0?|[12])\d)|3[01])$"))
            {
                await DisplayAlert("DADOS INVÁLIDOS", "A data de nascimento introduzida não é válida.", "Ok");
                return "-1";
            }
            else
            {
                if ((DateTime.Parse(birthdateValue.entry.Text) - DateTime.Now).Days > 0)
                {
                    await DisplayAlert("DADOS INVÁLIDOS", "A data de nascimento introduzida não é válida.", "Ok");
                    return "-1";
                }
            }

            App.member = new Member();
                 
            App.member.name = nameValue.entry.Text;
            App.member.birthdate = birthdateValue.entry.Text;
            App.member.nif = nifValue.entry.Text;
            App.member.email = emailValue.entry.Text;
            App.member.member_type = Constants.member_type.KeyByValue(tipoSocioValue.picker.SelectedItem.ToString());
            App.member.number_fnkp = federadoValue.picker.SelectedItem.ToString();

            Debug.Print("CreateNewMember AQUIIII1 2");

            MemberManager memberManager = new MemberManager();

            showActivityIndicator();

            Debug.Print("CreateNewMember AQUIIII1 3");
            var result = await memberManager.createNewMember(App.member);

            hideActivityIndicator();

            if (result == "1")
            {
                await Navigation.PushAsync(new NewMemberSuccessPageCS());

            }
            else if (result == "-1")
            {
                await DisplayAlert("DADOS INVÁLIDOS", "Tem de preencher todos os dados obrigatórios", "Ok");

            }
            else if (result == "-2")
            {
                await DisplayAlert("SÓCIO JÁ EXISTE", "Já existe um sócio no nosso sistema com este Número de Contribuinte.", "Ok");

            }
            else
            {
                Xamarin.Forms.Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
                {
                    BarBackgroundColor = Color.FromRgb(15, 15, 15),
                    BarTextColor = Color.White
                };
                return "-1";
            }



            return result;
        }

    }


}