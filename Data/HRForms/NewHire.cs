using System;
using System.Collections.Generic;

namespace FormsBoard.Data.HRForms
{
    public class NewHire
    {
        public string EmployeeName { get; set; }
        public string Title { get; set; }
        public string Specialty { get; set; }
        public string Department { get; set; }
        public string HomeLocation { get; set; }
        public string OfficeName { get; set; }
        public string Company { get; set; }


        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public string StandardApplication { get; set; }
        public bool MilageGrid { get; set; }
        public string EmployeeHomeAddress { get; set; }
        public string LocationsNeeded { get; set; }


        public IEnumerable<string> DistributionLists { get; set; }


        public string SubmittedBy { get; set; }
        public string Comments { get; set; }
    }
}
