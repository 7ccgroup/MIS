
namespace BaseAppUI.ViewModel.Notifies
{
    public class TransactionSuccessNotify : NotifyBase
    {
        public string Message { get; set; }

        public TransactionSuccessNotify(string message)
        {
            Message=message;
        }

    }
}
