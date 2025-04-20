using System;

namespace FormsBoard.Domain.Forms.HR.NameChangeRequest
{
    public class NameChangeRequest
    {
        public string CurrentFirstName { get; set; }
        public string CurrentMiddleName { get; set; }
        public string CurrentLastName { get; set; }
        public string RequestedFirstName { get; set; }
        public string RequestedMiddleName { get; set; }
        public string RequestedLastName { get; set; }
        public string ApproverName { get; set; }
        public DateTime RequestedDate { get; set; }
        public string SubmittedBy { get; set; }
        public string Comments { get; set; }
    }
}
