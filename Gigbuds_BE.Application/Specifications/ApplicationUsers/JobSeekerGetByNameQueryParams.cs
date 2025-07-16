namespace Gigbuds_BE.Application.Specifications.ApplicationUsers
{
    public class JobSeekerGetByNameQueryParams : BasePagingParams
    {
        public string? Name { get; set; } = string.Empty;
    }
}
