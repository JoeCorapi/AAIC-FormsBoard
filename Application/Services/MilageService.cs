using FormsBoard.Application.Interfaces;
using FormsBoard.Data.Repositories;
using FormsBoard.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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

            if (form == null || form.FormStatusId != 1) // Only drafts can be submitted
                return false;

            form.FormStatusId = 2; // Submitted
            await _repository.UpdateFormAsync(form);

            return true;
        }

        public async Task<bool> ManagerApproveAsync(int formId, string managerId, string managerEmail, string managerDisplayName)
        {
            var form = await _repository.GetFormByIdAsync(formId);

            if (form == null || form.FormStatusId != 2) // Only submitted forms can be approved
                return false;

            form.FormStatusId = 3; // Manager Approved
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

            if (form == null || form.FormStatusId != 2) // Only submitted forms can be rejected
                return false;

            form.FormStatusId = 1; // Back to Draft
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

            if (form == null || form.FormStatusId != 3) // Only manager-approved forms can be approved by accounting
                return false;

            form.FormStatusId = 4; // Accounting Approved
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

            if (form == null || form.FormStatusId != 3) // Only manager-approved forms can be rejected by accounting
                return false;

            form.FormStatusId = 1; // Back to Draft
            form.RejectionReason = reason;
            form.RejectedBy = accountantId;
            form.RejectedByName = accountantName;
            form.RejectionDate = DateTime.Now;
            form.RejectionPhase = "Accounting";

            await _repository.UpdateFormAsync(form);

            return true;
        }

        public async Task<bool> MarkAsPaidAsync(int formId, string paymentReference)
        {
            var form = await _repository.GetFormByIdAsync(formId);

            if (form == null || form.FormStatusId != 4) // Only accounting-approved forms can be marked as paid
                return false;

            form.FormStatusId = 5; // Paid
            form.PaymentDate = DateTime.Now;
            form.PaymentReference = paymentReference;

            await _repository.UpdateFormAsync(form);

            return true;
        }

        public async Task<bool> DiscardFormAsync(int formId)
        {
            var form = await _repository.GetFormByIdAsync(formId);

            if (form == null)
                return false;

            form.FormStatusId = 6; // Discarded

            await _repository.UpdateFormAsync(form);

            return true;
        }

        public async Task<IEnumerable<FormStatus>> GetFormStatusesAsync()
        {
            return await _repository.GetFormStatusesAsync();
        }
    }
}
