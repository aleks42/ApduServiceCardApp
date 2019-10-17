namespace ApduServiceCardApp.Services
{
    public interface INfcHelper
    {
        NfcAdapterStatus GetNfcAdapterStatus();

        void GoToNFCSettings();
    }

    public enum NfcAdapterStatus
    {
        Enabled,
        Disabled,
        NoAdapter
    }
}
