using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Messaging;
using Plugin.Share;
using Xamarin.Forms;
using static My_Mirror.App;
using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using System.Diagnostics;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace My_Mirror
{
    public partial class HomePage : ContentPage
    {
        IAdInterstitial adInterstitial;

        public HomePage()
        {

            if (Device.RuntimePlatform == Device.Android)
            {
                PermissionAsync();
            }

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
                adBanner.Size = AdBanner.Sizes.FullBanner;
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

            ToolbarItem scanItem = new ToolbarItem();
            scanItem.Text = "Info";


            if (Device.RuntimePlatform == Device.UWP)
            {
                scanItem.Order = ToolbarItemOrder.Primary;
                scanItem.Icon = "IconInfo.png";
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                scanItem.Order = ToolbarItemOrder.Secondary;              
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                scanItem.Order = ToolbarItemOrder.Primary;
            }
            ToolbarItems.Add(scanItem);
            scanItem.Clicked += toolbar_Clicked;

           
            


        }

        void toolbar_Clicked(object sender, EventArgs e)
        {
            
            Navigation.PushAsync(new PaginaInfo());
        }

      
      

        void Show_Interstitial(object sender, EventArgs e)
        {
            adInterstitial.ShowAd();
        }

        private async void NavigateButton_OnClicked(object sender, EventArgs e)
        {
            
           
                await Navigation.PushAsync(new PaginaFotocamera());


            
        }


        private async Task PermissionAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera);
                   

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {

                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Camera access Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {


            }
        }


    }
}