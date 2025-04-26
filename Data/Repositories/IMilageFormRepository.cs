using FormsBoard.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormsBoard.Data.Repositories
{
    public interface IMileageFormRepository
    {
        Task<IEnumerable<MileageForm>> GetAllFormsAsync();
        Task<IEnumerable<MileageForm>> GetFormsByUserIdAsync(string userId);
        Task<IEnumerable<MileageForm>> GetFormsByStatusAsync(int statusId);
        Task<MileageForm> GetFormByIdAsync(int id);
        Task<MileageForm> CreateFormAsync(MileageForm form);
        Task<MileageForm> UpdateFormAsync(MileageForm form);
        Task DeleteFormAsync(int id);
        Task<IEnumerable<FormStatus>> GetFormStatusesAsync();
    }
}
