using System;
using System.Collections.Generic;
using System.Linq;
using UserNotifications;
using Foundation;
using UIKit;
using Intents;
using Xamarin.Forms;
using My_Mirror;

namespace My_Mirror.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        public UIApplicationShortcutItem LaunchedShortcutItem
        {
            get; set;
        }
        public bool HandleShortcutItem(UIApplicationShortcutItem shortcutItem)
        {
            var handled = false;

            // Anything to process?
            if (shortcutItem == null) return false;

            // Take action based on the shortcut type
            switch (shortcutItem.Type)
            {
                case ShortcutIdentifier.First:

                    //App.Current.MainPage = new NavigationPage(new PaginaFotocamera());
                    App.Current.MainPage = new PaginaFotocamera();
                    handled = true;
                    break;
               
            }

            // Return results
            return handled;
        }
        
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var shouldPerformAdditionalDelegateHandling = true;
            if (options != null)
            {
                LaunchedShortcutItem = options[UIApplication.LaunchOptionsShortcutItemKey] as UIApplicationShortcutItem;
                shouldPerformAdditionalDelegateHandling = (LaunchedShortcutItem == null);
            }

            
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) => {
                // Handle approval
                
            });

            // Get current notification settings
            UNUserNotificationCenter.Current.GetNotificationSettings((settings) => {
                var alertsAllowed = (settings.AlertSetting == UNNotificationSetting.Enabled);
            });

            var content = new UNMutableNotificationContent();
            content.Title = "Are you sure to look good?";
            content.Subtitle = "Take a look to yourself";
            content.Body = "With My Mirror you will be sure to look beautiful";          
            content.Badge = 1;

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(60 * 43800, true);

            var requestID = "sampleRequest";
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) => {
                if (err != null)
                {
                    // Do something with error...
                }
            });


            // Request access to Siri
           // INPreferences.RequestSiriAuthorization((INSiriAuthorizationStatus status) => {
                // Respond to returned status
             //   switch (status)
            //    {
              //      case INSiriAuthorizationStatus.Authorized:
              //          break;
              //      case INSiriAuthorizationStatus.Denied:
              //          break;
              //      case INSiriAuthorizationStatus.NotDetermined:
              //          break;
             //       case INSiriAuthorizationStatus.Restricted:
              //          break;
              //  }
           // });


            //return shouldPerformAdditionalDelegateHandling;
            return base.FinishedLaunching(app, options);
            

        }
        public override void OnActivated(UIApplication application)
        {
            // Handle any shortcut item being selected
            HandleShortcutItem(LaunchedShortcutItem);

            // Clear shortcut after it's been handled
            LaunchedShortcutItem = null;
        }
        public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
        {
            // Perform action
            completionHandler(HandleShortcutItem(shortcutItem));
        }
    }
}
