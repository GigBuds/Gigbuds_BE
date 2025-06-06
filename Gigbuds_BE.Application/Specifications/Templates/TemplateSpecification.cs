using Gigbuds_BE.Domain.Entities.Notifications;
using System.Linq.Expressions;

namespace Gigbuds_BE.Application.Specifications.Templates
{
    internal class GetByTemplateTypeSpecification : BaseSpecification<Template>
    {
        public GetByTemplateTypeSpecification(ContentType templateType) : base(
            x => x.ContentType == templateType && x.IsEnabled
        )
        {
        }
    }
}
