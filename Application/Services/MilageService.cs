using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FormsBoard.Domain.Entities;
using FormsBoard.Domain.FormModels;
using FormsBoard.Data.Repositories;
using FormsBoard.Application.Interfaces;

namespace FormsBoard.Application.Services
{
    public class MileageService : IMileageService
    {
        private readonly IMileageFormRepository _repository;

        public MileageService(IMileageFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MileageForm>> GetUserFormsAsync(string userId)
        {
            return await _repository.GetFormsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<MileageForm>> GetFormsByStatusAsync(int statusId)
        {
            return await _repository.GetFormsByStatusAsync(statusId);
        }

        public async Task<MileageForm> GetFormDetailsAsync(int formId)
        {
            return await _repository.GetFormByIdAsync(formId);
        }

        public async Task<MileageForm> CreateFormAsync(MileageForm form, string userId, string userEmail, string userDisplayName)
        {
            form.UserId = userId;
            form.UserEmail = userEmail;
            form.UserDisplayName = userDisplayName;
            form.DateSubmitted = DateTime.Now;
            form.FormStatusId = 1; // Draft
            form.MileageRate = 0.60m; // Default rate

            return await _repository.CreateFormAsync(form);
        }

        public async Task<MileageForm> UpdateFormAsync(MileageForm form)
        {
            return await _repository.UpdateFormAsync(form);
        }

        public async Task DeleteFormAsync(int formId)
        {
            await _repository.DeleteFormAsync(formId);
        }

        public async Task<bool> SubmitFormAsync(int formId)
        {
            var form = await _repository.GetFormByIdAsync(formId);

            if (form == null || (form.FormStatusId != FormState.Draft && form.FormStatusId != FormState.Rejected)) // Only drafts and rejections can be submitted
                return false;

            form.FormStatusId = FormState.Submitted; // Submitted
            await _repository.UpdateFormAsync(form);

            return true;
        }

        public async Task<bool> ManagerApproveAsync(int formId, string managerId, string managerEmail, string managerDisplayName)
        {
            var form = await _repository.GetFormByIdAsync(formId);

            if (form == null || form.FormStatusId != FormState.Submitted) // Only submitted forms can be approved
                return false;

            form.FormStatusId = FormState.ManagerApproved; // Manager Approved
            form.ManagerId = managerId;
            form.ManagerEmail = managerEmail;
            form.ManagerDisplayName = managerDisplayName;
            form.ApprovalDate = DateTime.Now;

            await _repository.UpdateFormAsync(form);

            return true;
        }

        public async Task<bool> ManagerRejectAsync(int formId, string managerId, string managerName, string reason)
        {
            var form = await _repository.GetFormByIdAsync(formId);

            if (form == null || form.FormStatusId != FormState.Submitted) // Only submitted forms can be rejected
                return false;

            form.FormStatusId = FormState.Rejected; // Back to User
            form.RejectionReason = reason;
            form.RejectedBy = managerId;
            form.RejectedByName = managerName;
            form.RejectionDate = DateTime.Now;
            form.RejectionPhase = "Manager";

            await _repository.UpdateFormAsync(form);

            return true;
        }

        public async Task<bool> AccountingApproveAsync(int formId, string accountantId, string accountantEmail, string accountantDisplayName)
        {
            var form = await _repository.GetFormByIdAsync(formId);

            if (form == null || form.FormStatusId != FormState.ManagerApproved) // Only manager-approved forms can be approved by accounting
                return false;

            form.FormStatusId = FormState.Completed; // Accounting Completed
            form.AccountantId = accountantId;
            form.AccountantEmail = accountantEmail;
            form.AccountantDisplayName = accountantDisplayName;
            form.AccountingApprovalDate = DateTime.Now;

            await _repository.UpdateFormAsync(form);

            return true;
        }

        public async Task<bool> AccountingRejectAsync(int formId, string accountantId, string accountantName, string reason)
        {
            var form = await _repository.GetFormByIdAsync(formId);

            if (form == null || form.FormStatusId != FormState.ManagerApproved) // Only manager-approved forms can be rejected by accounting
                return false;

            form.FormStatusId = FormState.Rejected; // Back to User
            form.RejectionReason = reason;
            form.RejectedBy = accountantId;
            form.RejectedByName = accountantName;
            form.RejectionDate = DateTime.Now;
            form.RejectionPhase = "Accounting";

            await _repository.UpdateFormAsync(form);

            return true;
        }

        public async Task<bool> DiscardFormAsync(int formId, string discardedById, string discardedByName, string reason)
        {
            var form = await _repository.GetFormByIdAsync(formId);

            // Invalid operation if form is not pending manager or accounting review (Submitted/MgrApproved)
            if (form == null || (form.FormStatusId != FormState.Submitted && form.FormStatusId != FormState.ManagerApproved))
                return false;

            // Update to Discarded status
            form.FormStatusId = FormState.Discarded; // Should be 6
            form.RejectionReason = reason;
            form.RejectedBy = discardedById;
            form.RejectedByName = discardedByName;
            form.RejectionDate = DateTime.Now;
            form.RejectionPhase = form.FormStatusId == FormState.Submitted ? "Manager" : "Accounting"; // Track which phase it was discarded in

            await _repository.UpdateFormAsync(form);

            return true;
        }

        public async Task<IEnumerable<FormStatus>> GetFormStatusesAsync()
        {
            return await _repository.GetFormStatusesAsync();
        }
    }
}
