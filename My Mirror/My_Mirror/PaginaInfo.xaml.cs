using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Messaging;
using Plugin.Share;
using Xamarin.Forms;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using System.Diagnostics;
using static My_Mirror.App;

namespace My_Mirror
{
    public partial class PaginaInfo : ContentPage
    {
        IAdInterstitial adInterstitial;
       
        public PaginaInfo()
        {
            InitializeComponent();
            // Handle when your app starts          
            var adBanner = new AdBanner();
            if (Device.RuntimePlatform == Device.UWP)
            {
                adBanner.Size = AdBanner.Sizes.Leaderboard;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                adBanner.Size = AdBanner.Sizes.Leaderboard; 
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                adBanner.Size = AdBanner.Sizes.LargeBanner;
            }
            try
            {
                string Comprato = App.Current.Properties["Comprato"].ToString();

                if (Comprato == "0")
                {
                    stackLayout.Children.Add(adBanner);
                }
            }
            catch
            {
                stackLayout.Children.Add(adBanner);
            }
            adInterstitial = DependencyService.Get<IAdInterstitial>();

            //  if (Device.RuntimePlatform == Device.UWP)
            //  {
            //    var btn = new Button
            //     {
            //        Text = "Close",
            //        HorizontalOptions = LayoutOptions.CenterAndExpand,
            //   };
            //   btn.Clicked += btn_Clicked;
            //     stackLayout.Children.Add(btn);

            //   }

        }
        void Show_Interstitial(object sender, EventArgs e)
        {
            adInterstitial.ShowAd();
        }



        private void btn_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new Estensione());
        }

        private void NavigateButton1_OnClicked(object sender, EventArgs e)
        {

            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                // Send simple e-mail to single receiver without attachments, bcc, cc etc.
                emailMessenger.SendEmail("Flavio.Rausa@outlook.com", "My Mirror app", "");

            }
        }
        private void NavigateButton2_OnClicked(object sender, EventArgs e)
        {


            Device.OpenUri(new Uri("https://flaviorausa.com/my-apps/"));


        }

          async void NavigateButton3_OnClicked(object sender, EventArgs e)
        {


            try
            {
                var productId = "";
               
                    
               if (Device.RuntimePlatform == Device.UWP)
                {
                    productId = "Remove special offers";
                   
                }
               else if(Device.RuntimePlatform == Device.Android)
                {
                    productId = "remove_special_offers";
                  
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    productId = "Remove_special_offers";
                    
                }
                    var connected = await CrossInAppBilling.Current.ConnectAsync();

                if (!connected)
                {
                    //Couldn't connect to billing, could be offline, alert user
                    return;
                }

                //try to purchase item
                var purchase = await CrossInAppBilling.Current.PurchaseAsync(productId, ItemType.InAppPurchase, "apppayload");
                if (purchase == null)
                {
                    //Not purchased, alert the user
                    await DisplayAlert("Failure", "Something went wrong. Please try again or contact me on the Support button.", "Ok");
                }
                else
                {
                    //Purchased, save this information
                    var id = purchase.Id;
                    var token = purchase.PurchaseToken;
                    var state = purchase.State;
                    App.Current.Properties["Comprato"] = 1;

                    await DisplayAlert("Success", "The purchase was successful. Restart the application to make the changes effective.", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failure", "Something went wrong. Please try again or contact me on the Support button.", "Ok");
                //Something bad has occurred, alert user
            }
            finally
            {
                //Disconnect, it is okay if we never connected
                await CrossInAppBilling.Current.DisconnectAsync();
            }



        }







    }

}

