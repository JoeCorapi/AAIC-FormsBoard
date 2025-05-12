using FormsBoard.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormsBoard.Application.Interfaces
{
    public interface IMileageService
    {
        Task<IEnumerable<MileageForm>> GetUserFormsAsync(string userId);
        Task<IEnumerable<MileageForm>> GetFormsByStatusAsync(int statusId);
        Task<MileageForm> GetFormDetailsAsync(int formId);
        Task<MileageForm> CreateFormAsync(MileageForm form, string userId, string userEmail, string userDisplayName);
        Task<MileageForm> UpdateFormAsync(MileageForm form);
        Task DeleteFormAsync(int formId);
        Task<bool> SubmitFormAsync(int formId);
        Task<bool> ManagerApproveAsync(int formId, string managerId, string managerEmail, string managerDisplayName);
        Task<bool> ManagerRejectAsync(int formId, string managerId, string managerName, string reason);
        Task<bool> AccountingApproveAsync(int formId, string accountantId, string accountantEmail, string accountantDisplayName);
        Task<bool> AccountingRejectAsync(int formId, string accountantId, string accountantName, string reason);
        Task<bool> DiscardFormAsync(int formId, string discardedById, string discardedByName, string reason);

        Task<IEnumerable<FormStatus>> GetFormStatusesAsync();
    }
}
