using Android.App;
using Android.Content;
using Android.Nfc.CardEmulators;
using Android.OS;
using System;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

// HCE
// http://www.androiddocs.com/guide/topics/connectivity/nfc/hce.html

// COMPLETE LIST OF APDU RESPONSES
// https://www.eftlab.com/knowledge-base/complete-list-of-apdu-responses/

namespace ApduServiceCardApp.Droid.Services
{
    [Service(Exported = true, Enabled = true, Permission = "android.permission.BIND_NFC_SERVICE"),
       IntentFilter(new[] { "android.nfc.cardemulation.action.HOST_APDU_SERVICE" }, Categories = new[] { "android.intent.category.DEFAULT" }),
       MetaData("android.nfc.cardemulation.host_apdu_service", Resource = "@xml/aid_list")]
    public class CardService : HostApduService
    {
        // ISO-DEP command HEADER for selecting an AID.
        // Format: [Class | Instruction | Parameter 1 | Parameter 2]
        private static readonly byte[] SELECT_APDU_HEADER = new byte[] { 0x00, 0xA4, 0x04, 0x00 };

        // "OK" status word sent in response to SELECT AID command (0x9000)
        private static readonly byte[] SELECT_OK_SW = new byte[] { 0x90, 0x00 };

        // "UNKNOWN" status word sent in response to invalid APDU command (0x0000)
        private static readonly byte[] UNKNOWN_CMD_SW = new byte[] { 0x00, 0x00 };

        public override byte[] ProcessCommandApdu(byte[] commandApdu, Bundle extras)
        {
            var IsEnabled = Preferences.Get("IsEnabled", false);
            if (!IsEnabled) return UNKNOWN_CMD_SW;

            if (commandApdu.Length >= SELECT_APDU_HEADER.Length
                && Enumerable.SequenceEqual(commandApdu.Take(SELECT_APDU_HEADER.Length), SELECT_APDU_HEADER))
            {
                var hexString = string.Join("", Array.ConvertAll(commandApdu, b => b.ToString("X2")));

                //StartActivity(typeof(MainActivity));
                var intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("MSG_DATA", $"data for application - {hexString}");
                StartActivity(intent);

                //SendMessageToActivity($"Recieved message from reader: {hexString}");

                var messageToReader = $"Hello Reader! - {Preferences.Get("key1", "key1 not found")}";
                var messageToReaderBytes = Encoding.UTF8.GetBytes(messageToReader);
                return messageToReaderBytes.Concat(SELECT_OK_SW).ToArray();
            }

            return UNKNOWN_CMD_SW;
        }

        public override void OnDeactivated(DeactivationReason reason) { }

        private void SendMessageToActivity(string msg)
        {
            Intent intent = new Intent("MSG_NAME");
            intent.PutExtra("MSG_DATA", msg);
            SendBroadcast(intent);
        }
    }
}