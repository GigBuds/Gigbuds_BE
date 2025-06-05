namespace Gigbuds_BE.Application.Commons.Constants;

public static class ProjectConstant
{
    public const int Free_Tier_Job_Application = 10;
    public const int Default_Priority_Level = 0;
    public static class MembershipLevel
    {
        public const string Free_Tier_Job_Application_Title = "Gói Miễn phí";
        public const string Basic_Tier_Job_Application_Title = "Gói Cơ bản";
        public const string Premium_Tier_Job_Application_Title = "Gói Cao cấp";
    }
    public static class EmployerMembership
    {
        public const int Default_Priority_Level = 0;
        public const int Basic_Post_Priority_Level = 1;
        public const int Premium_Post_Priority_Level = 2;

        /// <summary>
        /// Returns the post priority level for a given membership level string.
        /// </summary>
        /// <param name="membershipLevel">The membership level as a string.</param>
        /// <returns>Priority level as integer.</returns>
        public static int GetPriorityLevel(string membershipLevel)
        {
            return membershipLevel switch
            {
                MembershipLevel.Premium_Tier_Job_Application_Title => Premium_Post_Priority_Level,
                MembershipLevel.Basic_Tier_Job_Application_Title => Basic_Post_Priority_Level,
                _ => Default_Priority_Level,
            };
        }
    }
    public static class UserRoles
    {
        public const string JobSeeker = "JobSeeker";
        public const string Employer = "Employer";
        public const string Admin = "Admin";
        public const string Staff = "Staff";
    }
}


