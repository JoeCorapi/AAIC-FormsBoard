using System;
using System.Collections.Generic;

namespace FormsBoard.Domain.Forms.HR.SeparatedEmployee
{
    public class SeparatedEmployee
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime SeparatedDate { get; set; }
        public string DeactivationEffective { get; set; }
        public string Location { get; set; }
        public string Specialty { get; set; }
        public string Department { get; set; }
        public IEnumerable<string> RemoveFromList { get; set; }
        public string SubmittedBy { get; set; }

    }
}
