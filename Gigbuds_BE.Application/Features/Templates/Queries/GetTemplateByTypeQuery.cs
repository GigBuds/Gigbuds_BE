using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Templates.Queries
{
    internal class GetTemplateByTypeQuery : IRequest<string>
    {
        public ContentType TemplateType { get; set; }
    }
}
