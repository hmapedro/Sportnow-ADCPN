using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SportNow.Model;
using SportNow.Services.Data.JSON;
using System.Diagnostics;
using SportNow.CustomViews;

namespace SportNow.Views.Profile
{
    public class ConsentPageCS : DefaultPage
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
		Button confirmConsentButton;

		CheckBox checkBoxAssembleiaGeral, checkBoxRegulamentoInterno, checkBoxTratamentoDados, checkBoxRegistoImagens, checkBoxFotografiaSocio, checkBoxWhatsApp;


		private ScrollView scrollView;

		public void initLayout()
		{
			Title = "CONSENTIMENTOS";



		}


		public async void initSpecificLayout()
		{

			Frame backgroundFrame= new Frame
			{
				CornerRadius = 10,
				IsClippedToBounds = true,
				BackgroundColor = Color.FromRgb(240,240,240),
				HasShadow = false,
				
			};

			relativeLayout.Children.Add(backgroundFrame,
				xConstraint: Constraint.Constant(10 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(0 * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (20 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Height) - (90 * App.screenHeightAdapter));
				})
			);

			relativeLayout.LowerChild(backgroundFrame);

			int y_index = 20;

			if (Constants.GetAge(DateTime.Parse(App.member.birthdate)) > 18)
			{
				checkBoxAssembleiaGeral = new CheckBox { Color = Color.FromRgb(100, 100, 100) };
				if (App.member.consentimento_assembleia == "1")
				{
					checkBoxAssembleiaGeral.IsChecked = true;
				}
				else
				{
					checkBoxAssembleiaGeral.IsChecked = false;
				}

				relativeLayout.Children.Add(checkBoxAssembleiaGeral,
					xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
				);
				Label labelAssembleiaGeral = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.consentFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
				labelAssembleiaGeral.Text = "Concordo em receber convocatórias para Assembleia Geral por via electrónica *";

				relativeLayout.Children.Add(labelAssembleiaGeral,
					xConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (100 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
				);

				y_index = y_index + 50;
			}

			checkBoxRegulamentoInterno = new CheckBox { Color = Color.FromRgb(100, 100, 100) };
			if (App.member.consentimento_regulamento== "1")
			{
				checkBoxRegulamentoInterno.IsChecked = true;
			}
			else
			{
				checkBoxRegulamentoInterno.IsChecked = false;
			}

			relativeLayout.Children.Add(checkBoxRegulamentoInterno,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);

			Label labelRegulamentoInterno = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.consentFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
			labelRegulamentoInterno.Text = "Li e concordo com o Reg. Interno da Associação (disponível em www.adcpn.pt) *";

			relativeLayout.Children.Add(labelRegulamentoInterno,
				xConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (100 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
			);

			y_index = y_index + 50;

			checkBoxTratamentoDados = new CheckBox { Color = Color.FromRgb(100, 100, 100) };
			if (App.member.consentimento_dados == "1")
			{
				checkBoxTratamentoDados.IsChecked = true;
			}
			else
			{
				checkBoxTratamentoDados.IsChecked = false;
			}

			relativeLayout.Children.Add(checkBoxTratamentoDados,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);
			Label labelTratamentoDados = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.consentFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };

			if (App.member.member_type == "praticante")
			{
				
				labelTratamentoDados.Text = "Autorizo o tratamento dos dados pessoais do sócio praticante pela ADCPN, para faturação, para os processos associados à atividade desportiva, em particular a filiação/refiliação em federações desportivas, inscrições em eventos desportivos nacionais ou internacionais, deslocações e estadas e processos de contratação de seguros desportivos, candidatura a subsídios e ao envio de mensagens sobre a atividade da ADCPN (SMS, MMS e correio eletrónico). *";
				relativeLayout.Children.Add(labelTratamentoDados,
					xConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (100 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(130 * App.screenHeightAdapter)
				);

				y_index = y_index + 140;
			}
			else
			{
				labelTratamentoDados.Text = "Autorizo o tratamento dos dados pessoais do sócio não praticante pela ADCPN, para faturação e para o envio de mensagens sobre a atividade da ADCPN (SMS, MMS, WhatsApp e correio eletrónico). *";
				relativeLayout.Children.Add(labelTratamentoDados,
					xConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (100 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(70 * App.screenHeightAdapter)
				);

				y_index = y_index + 80;
			}

			checkBoxRegistoImagens = new CheckBox { Color = Color.FromRgb(100, 100, 100) };
			if (App.member.consentimento_imagem == "1")
			{
				checkBoxRegistoImagens.IsChecked = true;
			}
			else
			{
				checkBoxRegistoImagens.IsChecked = false;
			}

			relativeLayout.Children.Add(checkBoxRegistoImagens,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);
			Label labelRegistoImagens = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.consentFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };

			if (App.member.member_type == "praticante")
			{
				labelRegistoImagens.Text = "No âmbito das atividades que desenvolve, a ADCPN procede ao registo, gravação e captação de imagens dos participantes em treinos, competições e eventos para utilização com finalidades pedagógicas e/ou promocionais. A ADCPN procede, por vezes, à divulgação, total ou parcial, dessas atividades e das imagens que lhe estão associadas, de pessoas e bens, através das páginas eletrónicas, portais ou redes sociais e órgãos de comunicação social. Cedo, a título gratuito, à ADCPN, para a divulgação acima descrita, os direitos de imagem associados à minha participação e do meu educando nas iniciativas em que a ADCPN esteja envolvida.";
			}
			else
			{
				labelRegistoImagens.Text = "No âmbito das atividades que desenvolve, a ADCPN procede ao registo, gravação e captação de imagens dos participantes em treinos, competições e eventos para utilização com finalidades pedagógicas e/ou promocionais. A ADCPN procede, por vezes, à divulgação, total ou parcial, dessas atividades e das imagens que lhe estão associadas, de pessoas e bens, através das páginas eletrónicas, portais ou redes sociais e órgãos de comunicação social. Cedo, a título gratuito, à ADCPN, para a divulgação acima descrita, os direitos de imagem associados à minha participação nas iniciativas em que a ADCPN esteja envolvida.";
			}
			relativeLayout.Children.Add(labelRegistoImagens,
				xConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (100 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(185 * App.screenHeightAdapter)
			);

			y_index = y_index + 190;

			checkBoxFotografiaSocio = new CheckBox { Color = Color.FromRgb(100, 100, 100) };
			if (App.member.consentimento_fotosocio == "1")
			{
				checkBoxFotografiaSocio.IsChecked = true;
			}
			else
			{
				checkBoxFotografiaSocio.IsChecked = false;
			}

			relativeLayout.Children.Add(checkBoxFotografiaSocio,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);
			Label labelFotografiaSocio = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.consentFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };

			if (App.member.member_type == "praticante")
			{
				labelFotografiaSocio.Text = "Autorizo a ADCPN a recolher a foto tipo passe do sócio praticante para uso na ficha de sócio e para a emissão de credenciais de eventos. Caso o evento seja organizado por uma entidade externa, autorizo a ADCPN a enviar a minha foto tipo passe à entidade organizadora do evento.";
				relativeLayout.Children.Add(labelFotografiaSocio,
					xConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (100 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(80 * App.screenHeightAdapter)
				);

				y_index = y_index + 90;

			}
			else
			{
				labelFotografiaSocio.Text = "Autorizo a ADCPN a recolher a foto tipo passe do sócio não praticante para uso na ficha de sócio.";
				relativeLayout.Children.Add(labelFotografiaSocio,
					xConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (100 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
				);

				y_index = y_index + 50;
			}

			checkBoxWhatsApp = new CheckBox { Color = Color.FromRgb(100, 100, 100) };
			if (App.member.consentimento_whatsapp == "1")
			{
				checkBoxWhatsApp.IsChecked = true;
			}
			else
			{
				checkBoxWhatsApp.IsChecked = false;
			}

			/*relativeLayout.Children.Add(checkBoxWhatsApp,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(30 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);*/
			Label labelWhatsApp = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Start, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.consentFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
			labelWhatsApp.Text = "Pretendo pertencer ao grupo WhatsApp da ADCPN enquanto veículo de partilha de informação interna da Associação.";
			/*relativeLayout.Children.Add(labelWhatsApp,
				xConstraint: Constraint.Constant(60 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (100 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
			);*/

			RoundButton confirmButton = new RoundButton("CONFIRMAR", 100, 50);
			confirmButton.button.Clicked += confirmConsentButtonClicked;

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

		public ConsentPageCS()
		{
			this.initLayout();
			this.initSpecificLayout();
		}

		async void confirmConsentButtonClicked(object sender, EventArgs e)
		{

			//SAVE CONSENTIMENTOS!!!!!
			MemberManager memberManager = new MemberManager();

			bool checkBoxAssembleiaGeral_isChecked = false;
			if (checkBoxAssembleiaGeral != null)
			{
				if (checkBoxAssembleiaGeral.IsChecked == false)
				{
					checkBoxRegulamentoInterno.IsChecked = true;
					await DisplayAlert("Consentimento Assembleia Geral", "Para ser Sócio da ADCPN tem de concordar receber convocatórias para Assembleia Geral por via electrónica", "Ok");
					return;
				}
				checkBoxAssembleiaGeral_isChecked = checkBoxAssembleiaGeral.IsChecked;
			}

			if (checkBoxRegulamentoInterno.IsChecked == false)
			{
				checkBoxRegulamentoInterno.IsChecked = true;
				await DisplayAlert("Consentimento Regulamento Interno", "Para ser Sócio da ADCPN tem de aceitar o Regulamento Interno", "Ok");
				return;
			}
			if (checkBoxTratamentoDados.IsChecked == false)
			{
				checkBoxTratamentoDados.IsChecked = true;
				await DisplayAlert("Consentimento Tratamento Dados", "Para ser Sócio da ADCPN tem de consentir o Tratamento de Dados", "Ok");
				return;
			}
			if (checkBoxFotografiaSocio.IsChecked == false)
			{
				await memberManager.Delete_Member_Photo(App.member.id);
				//return;
			}

			/*if (checkBoxFotografiaSocio.IsChecked == false)
			{
				bool answerMemberPhoto = await DisplayAlert("Foto Membro", "Confirma que não quer consentir a utilização da sua foto? Desta forma não conseguiremos ...", "Confirmar", "Voltar");
				if (!answerMemberPhoto)
				{
					return;
				}
					
			}*/

			App.member.consentimento_assembleia = Convert.ToInt16(checkBoxAssembleiaGeral_isChecked).ToString();
			App.member.consentimento_regulamento = Convert.ToInt16(checkBoxRegulamentoInterno.IsChecked).ToString();
			App.member.consentimento_dados = Convert.ToInt16(checkBoxTratamentoDados.IsChecked).ToString();
			App.member.consentimento_imagem = Convert.ToInt16(checkBoxRegistoImagens.IsChecked).ToString();
			App.member.consentimento_fotosocio = Convert.ToInt16(checkBoxFotografiaSocio.IsChecked).ToString();
			App.member.consentimento_whatsapp = Convert.ToInt16(checkBoxWhatsApp.IsChecked).ToString();


			App.member.consentimento_assembleia = Convert.ToInt32(checkBoxAssembleiaGeral_isChecked).ToString();
			App.member.consentimento_regulamento = Convert.ToInt32(checkBoxRegulamentoInterno.IsChecked).ToString();
			App.member.consentimento_dados = Convert.ToInt32(checkBoxTratamentoDados.IsChecked).ToString();
			App.member.consentimento_imagem = Convert.ToInt32(checkBoxRegistoImagens.IsChecked).ToString();
			App.member.consentimento_fotosocio = Convert.ToInt32(checkBoxFotografiaSocio.IsChecked).ToString();
			App.member.consentimento_whatsapp = Convert.ToInt32(checkBoxWhatsApp.IsChecked).ToString();


			var result = await memberManager.Update_Member_Authorizations(App.member.id, App.member.consentimento_assembleia, App.member.consentimento_regulamento, App.member.consentimento_dados, App.member.consentimento_imagem, App.member.consentimento_fotosocio, App.member.consentimento_whatsapp);

			//await Navigation.PushAsync(new CompleteRegistration_Documents_PageCS());
		}
	}

}