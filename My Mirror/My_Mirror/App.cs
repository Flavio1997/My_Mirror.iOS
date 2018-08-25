using Plugin.Fingerprint;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace My_Mirror
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = new NavigationPage(new HomePage());
            MainPage.On<Xamarin.Forms.PlatformConfiguration.Windows>().SetToolbarPlacement(ToolbarPlacement.Bottom);

          
            

        }


        public async Task<bool> WasItemPurchased(string productId)
        {

          
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync();

                if (!connected)
                {
                    //Couldn't connect
                    return false;
                }

                //check purchases
                if (Device.RuntimePlatform == Device.iOS)
                {
                    var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers!");
                    
                        var purchases = await billing.GetPurchasesAsync(ItemType.InAppPurchase);
                        if (purchases?.Any(p => p.ProductId == productId) ?? false)
                        {
                            App.Current.Properties["Comprato"] = 1;

                            //Purchase restored
                            return true;

                        }
                        else
                        {
                            App.Current.Properties["Comprato"] = 0;
                            //no purchases found
                            return false;
                        }
                    
                   
                   
                }
                else
                {
                    var purchases = await billing.GetPurchasesAsync(ItemType.InAppPurchase);
                    if (purchases?.Any(p => p.ProductId == productId) ?? false)
                    {
                        App.Current.Properties["Comprato"] = 1;

                        //Purchase restored
                        return true;

                    }
                    else
                    {
                        App.Current.Properties["Comprato"] = 0;
                        //no purchases found
                        return false;
                    }
                }
               

                
               
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Billing Exception handle this based on the type
                Debug.WriteLine("Error: " + purchaseEx);
            }
            catch (Exception ex)
            {
                //Something has gone wrong
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return false;
        }



        protected override void OnStart()
        {
            

           


            var productId = "";
            // Handle when your app starts
            if (Device.RuntimePlatform == Device.UWP)
            {
                productId = "Remove special offers";

            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                productId = "remove_special_offers";

            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                productId = "Remove_special_offers";

            }
            Task<bool> x = WasItemPurchased(productId);
        }

        protected override void OnSleep()
        {
            //Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            //{
            //    CrossLocalNotifications.Current.Show("Are you sure to look good?", "Take a look to yourself: with My Mirror you will be sure to look beautiful");
            //    return true;
           // });
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
