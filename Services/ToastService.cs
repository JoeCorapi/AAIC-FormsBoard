using Syncfusion.Blazor.Notifications;

namespace FormsBoard.Services
{
    public class ToastService : IToastService
    {
        public SfToast SfToast { get; set; }

        public void ShowMessage(string title, string content = null)
        {
            SfToast.ShowAsync(new ToastModel
            {
                Title = title,
                Content = content
            });
        }
    }
}
