using Android.Content;

namespace ApduServiceCardApp.Droid.Services
{
    public class MessageReceiver : BroadcastReceiver
    {
        public override async void OnReceive(Context context, Intent intent)
        {
            var message = intent.GetStringExtra("MSG_DATA");
            await App.DisplayAlertAsync(message);
        }
    }
}