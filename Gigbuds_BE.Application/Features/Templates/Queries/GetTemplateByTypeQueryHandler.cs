
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Templates;
using Gigbuds_BE.Domain.Entities.Notifications;
using MediatR;

namespace Gigbuds_BE.Application.Features.Templates.Queries
{
    internal class GetTemplateByTypeQueryHandler : IRequestHandler<GetTemplateByTypeQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTemplateByTypeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(GetTemplateByTypeQuery request, CancellationToken cancellationToken)
        {
            var template = await _unitOfWork.Repository<Template>().GetBySpecificationAsync(new GetByTemplateTypeSpecification(request.TemplateType));
            return template.TemplateBody ?? string.Empty;
        }
    }
}
