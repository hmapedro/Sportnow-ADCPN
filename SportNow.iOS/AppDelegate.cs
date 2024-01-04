using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using Firebase.Analytics;
using Foundation;
using Plugin.FirebasePushNotification;
using UIKit;
using UserNotifications;
using SportNow.Views;
//using Syncfusion.XForms.iOS.MaskedEdit;
//using Syncfusion.SfBusyIndicator.XForms.iOS;

//using Firebase.CloudMessaging;

namespace SportNow.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            app.StatusBarStyle = UIStatusBarStyle.LightContent;

            Firebase.Core.App.Configure();

            //If debug you should reset the token each time.
            FirebasePushNotificationManager.Initialize(options, true);
            /*#if DEBUG
                        FirebasePushNotificationManager.Initialize(options, true);
            #else
                        FirebasePushNotificationManager.Initialize(options,false);
            #endif*/
            /*FirebasePushNotificationManager.Initialize(options, new NotificationUserCategory[]
            {
                new NotificationUserCategory("message",new List<NotificationUserAction> {
                    new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground)
                }),
                new NotificationUserCategory("request",new List<NotificationUserAction> {
                    new NotificationUserAction("Accept","Accept"),
                    new NotificationUserAction("Reject","Reject",NotificationActionType.Destructive)
                })

            });*/

            //SfMaskedEditRenderer.Init();

            Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
            Debug.Print("ERRO "+error.ToString());

        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Debug.Print("DidReceiveRemoteNotification");
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }


        // Details: https://github.com/wcoder/Xamarin.Plugin.DeviceOrientation#xamarinforms-ios
        [Export("application:supportedInterfaceOrientationsForWindow:")]
        public UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, IntPtr forWindow)
        {
            return Plugin.DeviceOrientation.DeviceOrientationImplementation.SupportedInterfaceOrientations;
        }

    }
}
