using Gigbuds_BE.Application.Specifications;
using Gigbuds_BE.Domain.Entities.Transactions;

namespace Gigbuds_BE.Application.Specifications.Transactions;

public class TransactionByReferenceCodeSpecification : BaseSpecification<TransactionRecord>
{
    public TransactionByReferenceCodeSpecification(long referenceCode) 
        : base(t => t.ReferenceCode == referenceCode)
    {
        AddInclude(t => t.Membership);
        AddInclude(t => t.Account);
    }
    
} 

public class TransactionRecordSpecification : BaseSpecification<TransactionRecord>
{
    public TransactionRecordSpecification() 
        : base(t => t.IsEnabled == true)
    {
        AddInclude(t => t.Membership);
    }
}