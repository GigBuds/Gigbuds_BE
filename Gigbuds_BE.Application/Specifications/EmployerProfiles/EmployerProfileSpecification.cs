using Gigbuds_BE.Domain.Entities.Accounts;

namespace Gigbuds_BE.Application.Specifications.EmployerProfiles
{
    public class EmployerProfileSpecification : BaseSpecification<EmployerProfile>
    {
        public EmployerProfileSpecification(string companyEmail) 
            : base(ep => ep.CompanyEmail == companyEmail && !string.IsNullOrEmpty(ep.CompanyEmail))
        {
        }
    }

    public class GetEmployerProfleByAccountIdSpecification : BaseSpecification<EmployerProfile>
    {
        public GetEmployerProfleByAccountIdSpecification(int accountId) 
            : base(ep => ep.Account.Id == accountId && ep.IsEnabled)
        {
            AddInclude(ep => ep.Account);
        }
    }
} 