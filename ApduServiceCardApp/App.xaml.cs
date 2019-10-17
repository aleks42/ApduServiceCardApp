using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ApduServiceCardApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Preferences.Set("key1", Guid.NewGuid().ToString());
            Preferences.Set("IsEnabled", true);

            MainPage = new MainPage();
        }

        public static async Task DisplayAlertAsync(string msg) => 
            await Device.InvokeOnMainThreadAsync(async () => await Current.MainPage.DisplayAlert("message from service", msg, "ok"));
    }
}
