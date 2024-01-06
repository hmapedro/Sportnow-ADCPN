using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SportNow.Views;
using System.Collections.Generic;
using SportNow.Model;
using Xamarin.Essentials;
using System.Diagnostics;
using Plugin.FirebasePushNotification;
using SportNow.Services.Data.JSON;
using System.Threading.Tasks;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using SportNow.Views.Profile;

namespace SportNow
{
    public partial class App : Application
    {

        public static List<Member> members;
        public static Member original_member;
        public static Member member;

        public static string VersionNumber = "1.1.0";
        public static string BuildNumber = "35";

        public static Competition competition;

        public static Competition_Participation competition_participation;
        public static Event_Participation event_participation;

        public static double screenWidthAdapter = 1, screenHeightAdapter = 1;
        public static int bigTitleFontSize = 0, titleFontSize = 0, menuButtonFontSize = 0,
                formLabelFontSize = 0, formValueFontSize = 0, itemTitleFontSize = 0, gridTextFontSize = 0, itemTextFontSize = 0, consentFontSize = 0, smallTextFontSize = 0;

        public static int ItemWidth = 0, ItemHeight = 0;

        //SELECTED TABS
        public static string DO_activetab = "quotas";
        public static string EVENTOS_activetab = "estagios";
        public static string EQUIPAMENTOS_activetab = "karategis";

        public static string token = "";

        public static string notification = "";

        public static bool isToPop = false;

        public static Color topColor = Color.FromRgb(107, 192, 102);
        public static Color bottomColor = Color.FromRgb(169, 219, 166);
        public static Color alternativeColor = Color.FromRgb(54, 62, 130);
        public static Color normalTextColor = Color.FromRgb(117, 117, 117);

        public static string DefaultImageId = "default_image";
        public static string ImageIdToSave = null;

        public App(bool hasNotification = false, object notificationData = null)
        {
            Debug.Print("App Constructor");
            if (hasNotification)
            {
                Debug.Print("App Constructor hasNotification true");
            }
            else
            {
                Debug.Print("App Constructor hasNotification false");
                new App();
            }
        }

        public App()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                startNotifications();
            }
            
            checkPreviousLoginOk();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjY3MzM4OUAzMjMyMmUzMDJlMzBsd1VhMkpBbXh5dU1KVWhLUXVRdHNxR1UyQ0ltRU1uMFU1SThuQ3V2VGJBPQ==");

            //MainPage = new MainPage();

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);

        }

        public void checkPreviousLoginOk()
        {
            if (App.Current.Properties.ContainsKey("EMAIL") & App.Current.Properties.ContainsKey("PASSWORD"))
            {
                MainPage = new NavigationPage(new MainTabbedPageCS("",""))
                {
                    BarBackgroundColor = Color.White,
                    BackgroundColor = Color.White,
                    BarTextColor = Color.Black//FromRgb(75, 75, 75)
                };
            }
            else
            {

                MainPage = new NavigationPage(new LoginPageCS(""))
                {
                    BarBackgroundColor = Color.FromRgb(15, 15, 15),
                    BarTextColor = Color.Black
                };
            }
        }

        protected void startNotifications() {
            CrossFirebasePushNotification.Current.Subscribe("General");

            CrossFirebasePushNotification.Current.OnTokenRefresh += async (s, p) =>
            {
                Debug.WriteLine($"TOKEN : {p.Token}");
                
                if ((App.original_member != null) & (App.token != p.Token))
                {
                    Debug.Print("App.original_member = " + App.original_member.id + ". App.token =" + App.token + ". p.Token=" + p.Token);
                    MemberManager memberManager = new MemberManager();
                    var result = await memberManager.updateToken(App.original_member.id, p.Token);
                }
                App.token = p.Token;
            };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                Debug.Print("OnNotificationReceived Cross");
                App.notification = "OnNotificationReceived";
            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += async (s, p) =>
            {
                Debug.Print("OnNotificationOpened Cross");

                App.notification = "OnNotificationOpened";

                executionActionPushNotification(p.Data);

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionID: {p.Identifier}");
                }

            };
        }

        protected override async void OnStart()

        {
            Debug.Print("OnStart");

            if (Device.RuntimePlatform != Device.Android)
            {
                startNotifications();
            }

        }

        public void executionActionPushNotification(IDictionary<string, object> dataDict)
        {
            var actiontype = "";
            var actionid = "";
            Debug.Print("Opened");
            App.notification = App.notification + " executionActionPushNotification";
            foreach (var data in dataDict)
            {
                Debug.Print("Push Notification " + data.Key.ToString() + " = "+data.Value.ToString());

                if (data.Key == "actiontype")
                {
                    Debug.Print("Push Notification Action = " + data.Value.ToString());
                    actiontype = data.Value.ToString();
                }

                if (data.Key == "actionid")
                {
                    Debug.Print("Push Notification Message = " + data.Value.ToString());
                    actionid = data.Value.ToString();
                    
                }
            }

            App.notification = App.notification + " actiontype = "+ actiontype;
            App.notification = App.notification + " actionid = " + actionid;
            if (((actiontype == "openevent") | (actiontype == "opencompetition") | (actiontype == "openexaminationsession")) & (actionid != ""))
            {
                MainPage = new NavigationPage(new MainTabbedPageCS(actiontype, actionid))
                {
                    BarBackgroundColor = Color.White,
                    BackgroundColor = Color.White,
                    BarTextColor = Color.Black//FromRgb(75, 75, 75)
                };
                //MainPage = new NavigationPage(new DetailEventPageCS(event_i));
            }
            else if ((actiontype == "openweekclass") | (actiontype == "opentodayclass") | (actiontype == "opentodayclassinstructor") | (actiontype == "authorizeregistration") | (actiontype == "openmonthfee"))
            {
                App.Current.MainPage = new NavigationPage(new MainTabbedPageCS(actiontype, actionid))
                {
                    BarBackgroundColor = Color.White,
                    BackgroundColor = Color.White,
                    BarTextColor = Color.Black
                };
            }
        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


        public static void screenWidthAdapterCalculator()
        {

            screenWidthAdapter = 1;
        }

        public static void screenHeightAdapterCalculator()
        {
            screenHeightAdapter = 1;
        }



        public static void AdaptScreen()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            App.screenWidthAdapter = ((mainDisplayInfo.Width) / mainDisplayInfo.Density) / 400;
            App.screenHeightAdapter = ((mainDisplayInfo.Height) / mainDisplayInfo.Density) / 850;
            //Debug.Print("App.screenWidthAdapter = " + App.screenWidthAdapter);
            //Debug.Print("App.screenHeightAdapter = " + App.screenHeightAdapter);

            App.ItemWidth = (int)(155 * App.screenWidthAdapter);
            App.ItemHeight = (int)(120 * App.screenHeightAdapter);

            App.bigTitleFontSize = (int)(26 * App.screenHeightAdapter);
            App.titleFontSize = (int)(18 * App.screenHeightAdapter);
            App.menuButtonFontSize = (int)(14 * App.screenHeightAdapter);
            App.formLabelFontSize = (int)(16 * App.screenHeightAdapter);
            App.formValueFontSize = (int)(16 * App.screenHeightAdapter);
            App.itemTitleFontSize = (int)(16 * App.screenHeightAdapter);
            App.itemTextFontSize = (int)(12 * App.screenHeightAdapter);
            App.gridTextFontSize = (int)(14 * App.screenHeightAdapter);
            App.smallTextFontSize = (int)(10 * App.screenHeightAdapter);
            App.consentFontSize = (int)(14 * App.screenHeightAdapter);
        }

        public static bool IsValidContrib(string Contrib)
        {
            Debug.WriteLine("IsValidContrib");

            if (Contrib.Length != 9)
            {
                return false;
            }

            bool functionReturnValue = false;
            functionReturnValue = false;
            string[] s = new string[9];
            string Ss = null;
            string C = null;
            int i = 0;
            long checkDigit = 0;

            s[0] = Convert.ToString(Contrib[0]);
            s[1] = Convert.ToString(Contrib[1]);
            s[2] = Convert.ToString(Contrib[2]);
            s[3] = Convert.ToString(Contrib[3]);
            s[4] = Convert.ToString(Contrib[4]);
            s[5] = Convert.ToString(Contrib[5]);
            s[6] = Convert.ToString(Contrib[6]);
            s[7] = Convert.ToString(Contrib[7]);
            s[8] = Convert.ToString(Contrib[8]);

            if (Contrib.Length == 9)
            {
                C = s[0];
                if (s[0] == "1" || s[0] == "2" || s[0] == "3" || s[0] == "5" || s[0] == "6" || s[0] == "9")
                {
                    checkDigit = Convert.ToInt32(C) * 9;
                    for (i = 2; i <= 8; i++)
                    {
                        checkDigit = checkDigit + (Convert.ToInt32(s[i - 1]) * (10 - i));
                    }
                    checkDigit = 11 - (checkDigit % 11);
                    if ((checkDigit >= 10))
                        checkDigit = 0;
                    Ss = s[0] + s[1] + s[2] + s[3] + s[4] + s[5] + s[6] + s[7] + s[8];
                    if ((checkDigit == Convert.ToInt32(s[8])))
                        functionReturnValue = true;
                }
            }
            return functionReturnValue;
        }

        public static int CountWords(string test)
        {
            int count = 0;
            bool wasInWord = false;
            bool inWord = false;

            for (int i = 0; i < test.Length; i++)
            {
                if (inWord)
                {
                    wasInWord = true;
                }

                if (Char.IsWhiteSpace(test[i]))
                {
                    if (wasInWord)
                    {
                        count++;
                        wasInWord = false;
                    }
                    inWord = false;
                }
                else
                {
                    inWord = true;
                }
            }

            // Check to see if we got out with seeing a word
            if (wasInWord)
            {
                count++;
            }

            return count;
        }
    }
}
