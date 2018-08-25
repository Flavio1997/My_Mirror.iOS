using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace My_Mirror
{
    public class ComandiVocali : Application
    {
        public ComandiVocali()
        {
            MainPage = new NavigationPage(new HomePage());
            App.Current.MainPage.Navigation.PushAsync(new PaginaFotocamera());

           
            MainPage.On<Xamarin.Forms.PlatformConfiguration.Windows>().SetToolbarPlacement(ToolbarPlacement.Bottom);



        }
    }
}
