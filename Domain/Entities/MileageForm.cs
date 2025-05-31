using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FormsBoard.Domain.Entities
{
    [Table("MileageForm")]
    public class MileageForm
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string UserDisplayName { get; set; }

        [Required]
        public DateTime DateSubmitted { get; set; }

        [Required]
        public int FormStatusId { get; set; }
        public virtual FormStatus Status { get; set; }

        // Manager approval tracking
        public string ManagerId { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerDisplayName { get; set; }
        public DateTime? ApprovalDate { get; set; }

        // Accounting approval tracking
        public string AccountantId { get; set; }
        public string AccountantEmail { get; set; }
        public string AccountantDisplayName { get; set; }
        public DateTime? AccountingApprovalDate { get; set; }

        // Rejection tracking
        public string RejectionReason { get; set; }
        public string RejectedBy { get; set; }
        public string RejectedByName { get; set; }
        public DateTime? RejectionDate { get; set; }
        public string RejectionPhase { get; set; }

        [Required]
        public decimal MileageRate { get; set; }

        // Location checkboxes
        public bool LocationCorporate { get; set; }
        public bool LocationLenoirCity { get; set; }
        public bool LocationClarksville { get; set; }
        public bool LocationMaryville { get; set; }
        public bool LocationCookeville { get; set; }
        public bool LocationMtJuliet { get; set; }
        public bool LocationDickson { get; set; }
        public bool LocationWeisgarber { get; set; }

        // Specialty checkboxes
        public bool SpecialtyCorporate { get; set; }
        public bool SpecialtyInfusion { get; set; }
        public bool SpecialtyNursePractitioners { get; set; }
        public bool SpecialtyPhysicians { get; set; }
        public bool SpecialtySpecialtyMeds { get; set; }
        public bool SpecialtyAllergy { get; set; }
        public bool SpecialtyMarketing { get; set; }
        public bool SpecialtyPharmacy { get; set; }
        public bool SpecialtyScheduling { get; set; }
        public bool SpecialtyVaccine { get; set; }

        public virtual IList<MileageLineItem> LineItems { get; set; }

        // Calculated properties
        [Display(Name = "Total Mileage")]
        public decimal TotalMileage => CalculateTotalMileage();

        [Display(Name = "Total Reimbursement")]
        public decimal TotalReimbursement => TotalMileage * MileageRate;

        private int CalculateTotalMileage()
        {
            int totalMiles = 0;
            if (LineItems != null)
            {
                foreach (var item in LineItems)
                {
                    totalMiles += item.TotalMiles;
                }
            }
            return totalMiles;
        }
    }
}
