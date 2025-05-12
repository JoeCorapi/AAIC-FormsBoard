namespace FormsBoard.Domain.FormModels
{

    public static class FormState
    {
        public const int Draft = 1;
        public const int Submitted = 2;
        public const int ManagerApproved = 3;
        public const int Completed = 4;
        public const int Discarded = 5;

        // You can also include name constants if useful
        public const string DraftName = "Draft";
        public const string SubmittedName = "Submitted";
        public const string ManagerApprovedName = "Manager Approved";
        public const string CompletedName = "Completed";
        public const string DiscardedName = "Discarded";

        // Helper methods if needed
        public static string GetName(int statusId)
        {
            return statusId switch
            {
                Draft => DraftName,
                Submitted => SubmittedName,
                ManagerApproved => ManagerApprovedName,
                Completed => CompletedName,
                Discarded => DiscardedName,
                _ => "Unknown"
            };
        }
    }
}
