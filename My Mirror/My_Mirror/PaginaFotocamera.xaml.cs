using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace My_Mirror
{
    public partial class PaginaFotocamera : ContentPage
    {
        public PaginaFotocamera()
        {
            
            InitializeComponent();
            if (Device.RuntimePlatform == Device.UWP || Device.RuntimePlatform == Device.Android)
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                PermissionAsync();
            }

        }
       

        private async Task PermissionAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await DisplayAlert("Need camera access", "Enable camera permission for this app in your phone settings", "OK");
                    }

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
