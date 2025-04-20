using Syncfusion.Blazor.Notifications;

namespace FormsBoard.Application.Interfaces
{
    public interface IToastService
    {
        SfToast SfToast { get; set; }

        void ShowMessage(string title, string content = null);
    }
}
