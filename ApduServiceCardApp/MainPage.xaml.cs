using ApduServiceCardApp.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ApduServiceCardApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CheckNfc();
        }

        private async Task CheckNfc()
        {
            var nfcHelper = DependencyService.Get<INfcHelper>();
            var status = nfcHelper.GetNfcAdapterStatus();

            switch (status)
            {
                case NfcAdapterStatus.Enabled:
                default:
                    await App.DisplayAlertAsync("nfc enabled!");
                    break;
                case NfcAdapterStatus.Disabled:
                    nfcHelper.GoToNFCSettings();
                    break;
                case NfcAdapterStatus.NoAdapter:
                    await App.DisplayAlertAsync("no nfc adapter found!");
                    break;
            }
        }
    }
}
