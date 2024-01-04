using System;
using Xamarin.Forms;
using SportNow.Model;
using System.Diagnostics;
using SportNow.CustomViews;
using System.Net;
using SportNow.Services.Data.JSON;
using System.Collections.Generic;
//Ausing Acr.UserDialogs;
using System.Globalization;
using System.Threading.Tasks;

namespace SportNow.Views.CompleteRegistration
{
	public class CompleteRegistration_Payment_PageCS : DefaultPage
	{

		protected async override void OnAppearing()
		{
		}


		protected async override void OnDisappearing()
		{

		}


		private ScrollView scrollView;

		//private Member member;


		FormValue valueQuotaADCPN, valueFiliacaoFPG, valueSeguroFPG, valueMensalidadeADCPN, valueTotal;
		Picker familiaresPicker;
		double valorQuotaADCPN, valorFiliacaoFGP, valorSeguroFGP;
		string paymentID;
		Payment payment;

		bool paymentDetected;

		public void initLayout()
		{
			Title = "PAGAMENTO";

			relativeLayout = new RelativeLayout
			{
				Margin = new Thickness(10)
			};
			Content = relativeLayout;

		}


		public async void initSpecificLayout()
		{
			showActivityIndicator();
			MemberManager memberManager = new MemberManager();
			string season = "";

			if (DateTime.Now.Month >= 8)
			{
				season = DateTime.Now.ToString("yyyy") + "_" + DateTime.Now.AddYears(1).ToString("yyyy");
			}
			else
			{
				season = DateTime.Now.AddYears(-1).ToString("yyyy") + "_" + DateTime.Now.ToString("yyyy");
			}


			paymentID = await memberManager.CreateAllFees(App.original_member.id, App.member.id, App.member.name, App.member.member_type, season);

			List<Fee> allFees = await memberManager.GetFees(App.member.id, season);

			string year = DateTime.Now.Year.ToString();
			Debug.Print("year = " + year);
			string month = "";
			if (DateTime.Now.Month == 8)
			{
				month = "9";
			}
			else
			{
				month = DateTime.Now.Month.ToString();
			}

			MonthFeeManager monthFeeManager = new MonthFeeManager();
			string monthFeeID = "";
			string valor_mensalidade = "";
			if (App.member.member_type == "praticante")
			{
				monthFeeID = await monthFeeManager.CreateMonthFee(App.original_member.id, App.member.id, App.member.name, App.member.aulaid, App.member.aulavalor, year, month, "emitida", paymentID, "0");
				valor_mensalidade = calculateMensalidade(0).ToString("0.00").Replace(",", ".");
				await monthFeeManager.Update_MonthFee_Value_byID(monthFeeID, valor_mensalidade);
			}
			hideActivityIndicator();


			valorQuotaADCPN = getValorQuota(allFees, "quota_associacao");
			valorFiliacaoFGP = getValorQuota(allFees, "filiacao_fgp");
			valorSeguroFGP = getValorQuota(allFees, "seguro_fgp");

			Frame backgroundFrame = new Frame
			{
				CornerRadius = 10,
				IsClippedToBounds = true,
				BackgroundColor = Color.FromRgb(240, 240, 240),
				HasShadow = false
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

			int y_index = CreateHeader();
			y_index = y_index + 10;

			Label labelQuotaADCPN = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
			labelQuotaADCPN.Text = "Quota Anual Praticante";
			relativeLayout.Children.Add(labelQuotaADCPN,
				xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (120 * App.screenHeightAdapter));
				}),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);

			valueQuotaADCPN = new FormValue(valorQuotaADCPN.ToString("0.00") + "€", App.titleFontSize, Color.White, App.normalTextColor, TextAlignment.End);
			//valueQuotaADCPN.Text = calculateQuotaADCPN();
			relativeLayout.Children.Add(valueQuotaADCPN,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return ((parent.Width) - (80 * App.screenHeightAdapter));
				}),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(70 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);

			y_index = y_index + 35;

			if (App.member.member_type == "praticante")
			{
				Label labelFiliacaoFPG = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
				labelFiliacaoFPG.Text = "Filiação/Refiliação FGP";
				relativeLayout.Children.Add(labelFiliacaoFPG,
					xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (120 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
				);


				valueFiliacaoFPG = new FormValue(valorFiliacaoFGP.ToString("0.00") + "€", App.titleFontSize, Color.White, App.normalTextColor, TextAlignment.End);
				relativeLayout.Children.Add(valueFiliacaoFPG,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (80 * App.screenHeightAdapter));
					}),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(70 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
				);

				y_index = y_index + 35;

				Label labelSeguroFPG = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
				labelSeguroFPG.Text = "Seguro FGP";
				relativeLayout.Children.Add(labelSeguroFPG,
					xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (120 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
				);

				valueSeguroFPG = new FormValue(valorSeguroFGP.ToString("0.00") + "€", App.titleFontSize, Color.White, App.normalTextColor, TextAlignment.End);
				relativeLayout.Children.Add(valueSeguroFPG,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (80 * App.screenHeightAdapter));
					}),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(70 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
				);

				y_index = y_index + 35;


				List<string> numerofamialiaresList = new List<string> { "Selecione uma opção", "Sou o único atleta do agregado familiar", "2 atletas do mesmo agregado familiar", "3 atletas do mesmo agregado familiar", "4 ou mais atletas do mesmo agregado familiar", };
				int selectedIndex = 0;

				familiaresPicker = new Picker
				{
					Title = "",
					TitleColor = Color.Black,
					BackgroundColor = Color.Transparent,
					TextColor = App.topColor,
					HorizontalTextAlignment = TextAlignment.Center,
					FontSize = App.consentFontSize

				};
				familiaresPicker.ItemsSource = numerofamialiaresList;
				familiaresPicker.SelectedIndex = selectedIndex;

				double desconto = 0;
				familiaresPicker.SelectedIndexChanged += async (object sender, EventArgs e) =>
				{
					Debug.Print("familiaresPicker selectedItem = " + familiaresPicker.SelectedItem.ToString());
					if (familiaresPicker.SelectedIndex == 1) //1
					{
						desconto = 0;
					}
					else if (familiaresPicker.SelectedIndex == 2) //2
					{
						desconto = 0.03;
					}
					else if (familiaresPicker.SelectedIndex == 3) //3
					{
						desconto = 0.05;
					}
					else if (familiaresPicker.SelectedIndex == 4) //4 ou mais
					{
						desconto = 0.07;
					}
					valueMensalidadeADCPN.label.Text = calculateMensalidade(desconto).ToString("0.00") + "€"; //String.Format("{0:2}", calculateMensalidade(desconto)) + "€";
					valueTotal.label.Text = calculateTotal(desconto).ToString("0.00") + "€";

					showActivityIndicator();

					App.member.n_familiares = familiaresPicker.SelectedIndex.ToString();
					var result = await memberManager.UpdateMemberInfo(App.original_member.id, App.member);
					if (result == "-1")
					{
						Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
						{
							BarBackgroundColor = Color.FromRgb(15, 15, 15),
							BarTextColor = Color.White
						};
					}

					valor_mensalidade = calculateMensalidade(desconto).ToString("0.00").Replace(",", ".");
					await monthFeeManager.Update_MonthFee_Value_byID(monthFeeID, valor_mensalidade);
					hideActivityIndicator();
				};


				Label labelMensalidadeADCPN = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
				labelMensalidadeADCPN.Text = "Mensalidade";
				relativeLayout.Children.Add(labelMensalidadeADCPN,
					xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (120 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
				);


				/*if (familiaresPicker.SelectedIndex == 1) //2
				{
					desconto = 0.03;
				}
				else if (familiaresPicker.SelectedIndex == 2) //3
				{
					desconto = 0.05;
				}
				else if (familiaresPicker.SelectedIndex == 3) //4 ou mais
				{
					desconto = 0.07;
				}

				await monthFeeManager.Update_MonthFee_Value_byID(monthFeeID, calculateMensalidade(desconto).ToString());*/
				//AUserDialogs.Instance.HideLoading();   //Hide loader

				valueMensalidadeADCPN = new FormValue(calculateMensalidade(desconto).ToString("0.00") + "€", App.titleFontSize, Color.White, App.normalTextColor, TextAlignment.End);
				relativeLayout.Children.Add(valueMensalidadeADCPN,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (80 * App.screenHeightAdapter));
					}),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(70 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
				);

				y_index = y_index + 45;

				Label labelTotal = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
				labelTotal.Text = "Total";
				relativeLayout.Children.Add(labelTotal,
					xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (120 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
				);


				valueTotal = new FormValue(calculateTotal(desconto).ToString("0.00") + "€", App.titleFontSize, App.topColor, Color.White, TextAlignment.End);
				relativeLayout.Children.Add(valueTotal,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (80 * App.screenHeightAdapter));
					}),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(70 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
				);

				y_index = y_index + 50;


				Label labelFamiliares = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = App.titleFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
				labelFamiliares.Text = "Número de familiares inscritos praticantes na Associação?";
				relativeLayout.Children.Add(labelFamiliares,
					xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (40 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
				);

				y_index = y_index + 30;


				relativeLayout.Children.Add(familiaresPicker,
					xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (40 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(50 * App.screenHeightAdapter)
				);


				y_index = y_index + 60;
			}
			else // NÃO PRATICANTE
			{
				Label labelTotal = new Label { BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontSize = App.titleFontSize, TextColor = Color.Black, LineBreakMode = LineBreakMode.WordWrap };
				labelTotal.Text = "Total";
				relativeLayout.Children.Add(labelTotal,
					xConstraint: Constraint.Constant(20 * App.screenHeightAdapter),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (120 * App.screenHeightAdapter));
					}),
					heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
				);


				valueTotal = new FormValue(valorQuotaADCPN.ToString("0.00") + "€", App.titleFontSize, App.topColor, Color.White, TextAlignment.End);
				relativeLayout.Children.Add(valueTotal,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return ((parent.Width) - (80 * App.screenHeightAdapter));
					}),
					yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
					widthConstraint: Constraint.Constant(70 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(40 * App.screenHeightAdapter)
				);

				y_index = y_index + 50;
			}

			Label selectPaymentModeLabel = new Label
			{
				Text = "Escolha o modo de pagamento pretendido:",
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.Black,
				//LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.titleFontSize
			};

			relativeLayout.Children.Add(selectPaymentModeLabel,
				xConstraint: Constraint.Constant(20),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (20 * App.screenHeightAdapter)); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter)
			);

			y_index = y_index + 30;

			Image MBLogoImage = new Image
			{
				Source = "logomultibanco.png",
				MinimumHeightRequest = 115 * App.screenHeightAdapter,
				//WidthRequest = 100 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter,
				//BackgroundColor = Color.Red,
			};

			var tapGestureRecognizerMB = new TapGestureRecognizer();
			tapGestureRecognizerMB.Tapped += OnMBButtonClicked;
			MBLogoImage.GestureRecognizers.Add(tapGestureRecognizerMB);

			relativeLayout.Children.Add(MBLogoImage,
				xConstraint: Constraint.Constant(40),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(102 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(120 * App.screenHeightAdapter)
			);


			Image MBWayLogoImage = new Image
			{
				Source = "logombway.png",
				//BackgroundColor = Color.Green,
				//WidthRequest = 184 * App.screenHeightAdapter,
				MinimumHeightRequest = 115 * App.screenHeightAdapter,
				HeightRequest = 115 * App.screenHeightAdapter
			};

			var tapGestureRecognizerMBWay = new TapGestureRecognizer();
			tapGestureRecognizerMBWay.Tapped += OnMBWayButtonClicked;
			MBWayLogoImage.GestureRecognizers.Add(tapGestureRecognizerMBWay);

			relativeLayout.Children.Add(MBWayLogoImage,
				xConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width - (142 * App.screenHeightAdapter)); // center of image (which is 40 wide)
				}),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.Constant(102 * App.screenHeightAdapter),
				heightConstraint: Constraint.Constant(120 * App.screenHeightAdapter)
			);

		}



		public double getValorQuota(List<Fee> allFees, string tipoQuota)
		{
			foreach (Fee fee in allFees)
			{
				if (fee.tipo == tipoQuota)
				{
					return Convert.ToDouble(fee.valor);
				}
			}
			return 0;
		}

		public int CreateHeader()
		{
			int y_index = 10;
			/*WebResponse response;
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Constants.images_URL + App.member.id + "_photo");

			request.Method = "HEAD";
			bool exists;
			try
			{
				response = request.GetResponse();
				exists = true;
			}
			catch (Exception ex)
			{
				exists = false;
			}

			RoundImage memberPhotoImage = new RoundImage();

			

			if (exists)
			{
				memberPhotoImage.photo.Source = new UriImageSource
				{
					Uri = new Uri(Constants.images_URL + App.member.id + "_photo"),
					CachingEnabled = false,
					CacheValidity = new TimeSpan(0, 0, 0, 1)
				};

				relativeLayout.Children.Add(memberPhotoImage,
					xConstraint: Constraint.RelativeToParent((parent) =>
					{
						return (parent.Width / 2) - (90 * App.screenHeightAdapter);
					}),
					yConstraint: Constraint.Constant(y_index),
					widthConstraint: Constraint.Constant(180 * App.screenHeightAdapter),
					heightConstraint: Constraint.Constant(180 * App.screenHeightAdapter) // size of screen -80
				);

				y_index = y_index + 200;
			}*/

			Label nameLabel = new Label
			{
				Text = App.member.nickname,
				VerticalTextAlignment = TextAlignment.Center,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = Color.Gray,
				LineBreakMode = LineBreakMode.NoWrap,
				FontSize = App.bigTitleFontSize
			};
			relativeLayout.Children.Add(nameLabel,
				xConstraint: Constraint.Constant(10 * App.screenWidthAdapter),
				yConstraint: Constraint.Constant(y_index * App.screenHeightAdapter),
				widthConstraint: Constraint.RelativeToParent((parent) =>
				{
					return (parent.Width) - (20 * App.screenWidthAdapter); // center of image (which is 40 wide)
				}),
				heightConstraint: Constraint.Constant(30 * App.screenHeightAdapter));

			return y_index + 30;
		}

		public CompleteRegistration_Payment_PageCS()
		{

			this.initLayout();
			this.initSpecificLayout();

			paymentDetected = false;

			int sleepTime = 5;
			Device.StartTimer(TimeSpan.FromSeconds(sleepTime), () =>
			{
				if ((paymentID != null) & (paymentID != ""))
				{
					this.checkPaymentStatus(paymentID);
					if (paymentDetected == false)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				return true;
			});
		}

		async void checkPaymentStatus(string paymentID)
		{
			Debug.Print("checkPaymentStatus");
			this.payment = await GetPayment(paymentID);
			if ((payment.status == "confirmado") | (payment.status == "fechado"))
			{
				App.member.estado = "activo";
				App.original_member.estado = "activo";

				if (paymentDetected == false)
				{
					paymentDetected = true;

					await DisplayAlert("Pagamento Confirmado", "O seu pagamento foi recebido com sucesso. Já pode aceder à nossa App!", "Ok");
					App.Current.MainPage = new NavigationPage(new MainTabbedPageCS("", ""))
					{
						BarBackgroundColor = Color.White,
						BackgroundColor = Color.White,
						BarTextColor = Color.Black
					};

				}
			}
		}

		async Task<Payment> GetPayment(string paymentID)
		{
			Debug.WriteLine("GetPayment");
			PaymentManager paymentManager = new PaymentManager();

			Payment payment = await paymentManager.GetPayment(this.paymentID);

			if (payment == null)
			{
				Application.Current.MainPage = new NavigationPage(new LoginPageCS("Verifique a sua ligação à Internet e tente novamente."))
				{
					BarBackgroundColor = Color.White,
					BarTextColor = Color.Black
				};
				return null;
			}
			return payment;
		}

		public double calculateQuotaADCPN()
		{
			if ((App.member.registrationdate?.Month > 2) & (App.member.registrationdate?.Month < 9))
			{
				return 40;
			}
			else
			{
				return 60;
			}
		}

		public double calculateFiliacaoFPG()
		{

			DateTime dateTime_Registration = (DateTime)App.member.registrationdate;

			//NOVA FILIAÇÃO
			if (App.member.number_fnkp == null)
			{
				App.member.number_fnkp = "";
			}

			if (App.member.number_fnkp == "")
			{
				if (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 6)
				{
					return 12;
				}
				else if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 9) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 50))
				{
					return 21;
				}
				else if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 10) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 49))
				{
					return 27;
				}
			}
			else
			{
				if ((dateTime_Registration.Month >= 9) & (dateTime_Registration.Month <= 11))
				{
					if (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 6)
					{
						return 10;
					}
					else if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 9) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 50))
					{
						return 19;
					}
					else if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 10) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 49))
					{
						return 23;
					}
				}
				else if (dateTime_Registration.Month == 12)
				{
					if (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 6)
					{
						return 13;
					}
					else if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 9) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 50))
					{
						return 25;
					}
					else if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 10) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 49))
					{
						return 30;
					}
				}
				else if (dateTime_Registration.Month == 1)
				{
					if (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 6)
					{
						return 16;
					}
					else if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 9) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 50))
					{
						return 30;
					}
					else if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 10) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 49))
					{
						return 37;
					}
				}
				else if ((dateTime_Registration.Month >= 2) & (dateTime_Registration.Month <= 8))
				{
					if (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 6)
					{
						return 20;
					}
					else if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 9) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 50))
					{
						return 38;
					}
					else if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 10) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 49))
					{
						return 46;
					}
				}

			}

			return 0;

		}

		public double calculateSeguroFPG()
		{
			if ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 9) | (Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 50))
			{
				return 0;
			}
			else if (App.member.aulatipo == "gpt")
			{
				return 6;
			}
			else if ((App.member.aulatipo == "acro") & ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 10) & (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 12)))
			{
				return 6;
			}
			else if ((App.member.aulatipo == "acro") & ((Constants.GetAge(DateTime.Parse(App.member.birthdate)) >= 13) & (Constants.GetAge(DateTime.Parse(App.member.birthdate)) <= 49)))
			{
				return 22;
			}
			return 0;
		}



		public double calculateMensalidade(double desconto)
		{
			Debug.Print("calculateMensalidade App.member.aulavalor = " + App.member.aulavalor);
			Debug.Print("calculateMensalidade desconto = " + desconto);
			Debug.Print("calculateMensalidade desconto = " + desconto);

			//Debug.Print("App.member.aulavalor = " + String.Format("{0:0}", App.member.aulavalor) + "€");
			//return String.Format("{0:0}", App.member.aulavalor) + ";
			double aulavalor = double.Parse(App.member.aulavalor, CultureInfo.InvariantCulture);
			return aulavalor * (1 - desconto);

		}

		public double calculateTotal(double desconto)
		{
			return valorQuotaADCPN + valorFiliacaoFGP + valorSeguroFGP + calculateMensalidade(desconto);
			//return calculateQuotaADCPN() + calculateFiliacaoFPG() + calculateSeguroFPG() + calculateMensalidade();
		}

		async void OnMBButtonClicked(object sender, EventArgs e)
		{
			Debug.Print("member.member_type = " + App.member.member_type);

			if (App.member.member_type == "praticante")
			{
				if (familiaresPicker.SelectedIndex == 0) //2
				{
					await DisplayAlert("Escolha uma Opção", "Para poder prosseguir tem de escolher uma opção de Número de familiares inscritos praticantes na Associação.", "Ok");
				}
				else
				{
					await Navigation.PushAsync(new CompleteRegistration_PaymentMB_PageCS(paymentID));
				}
			}
			else
			{
				await Navigation.PushAsync(new CompleteRegistration_PaymentMB_PageCS(paymentID));
			}
		}


		async void OnMBWayButtonClicked(object sender, EventArgs e)
		{
			if (App.member.member_type == "praticante")
			{
				if (familiaresPicker.SelectedIndex == 0) //2
				{
					await DisplayAlert("Escolha uma Opção", "Para poder prosseguir tem de escolher uma opção de Número de familiares inscritos praticantes na Associação.", "Ok");
				}
				else
				{
					await Navigation.PushAsync(new CompleteRegistration_PaymentMBWay_PageCS(paymentID));
				}
			}
			else
			{
				await Navigation.PushAsync(new CompleteRegistration_PaymentMBWay_PageCS(paymentID));
			}

		}

	}

}