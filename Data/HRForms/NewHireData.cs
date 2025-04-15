using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormsBoard.Data.HRForms
{
    public static class NewHireData
    {
        public static List<string> DistributionLists
        {
            get
            {
                return
                    new List<string>()
                    {
                        "AASC_Clinical",
                        "AASC_CustServ",
                        "AASC_NursePractitioners",
                        "AASC_Physicians",
                        "AASC_Scheduling",
                        "AASC_Staff",
                        "BaumNotifications",
                        "MessageCenterStaff",
                        "PME_AR",
                        "PME_Credentialing",
                        "PME_Staff",
                        "Remote_Staff",
                        "WebSchedule",
                        "Weisgarber_Vaccine_Lab",
                    };
            }
        }
    }
}
