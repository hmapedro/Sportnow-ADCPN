using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Firebase;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;

namespace SportNow.Droid
{
    //You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);


           /* var options = new FirebaseOptions.Builder()
               .SetApplicationId("")
               .SetApiKey("<APIKey>")
               .SetDatabaseUrl("https://<url>.firebaseio.com")
               .SetStorageBucket("*.appspot.com")
               .SetGcmSenderId("<senderID").Build();*/


            FirebaseApp.InitializeApp(this);

            FirebasePushNotificationManager.NotificationActivityType = typeof(MainActivity);

            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }


            //If debug you should reset the token each time.
            FirebasePushNotificationManager.Initialize(this, true);
/*#if DEBUG
                       FirebasePushNotificationManager.Initialize(this, true);
#else
            FirebasePushNotificationManager.Initialize(this,false);
           #endif*/
            

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);


            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("NOTIFICATION OPENED", p.Data);


            };

        }
    }
}
