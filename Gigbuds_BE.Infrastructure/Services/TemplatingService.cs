using Fluid;
using Fluid.Values;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.Templates;
using Gigbuds_BE.Domain.Entities.Notifications;
using Gigbuds_BE.Domain.Exceptions;

namespace Gigbuds_BE.Infrastructure.Services
{
    internal class TemplatingService : ITemplatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FluidParser _parser;

        public TemplatingService(IUnitOfWork unitOfWork)
        {
            _parser = new FluidParser();
            _unitOfWork = unitOfWork;
        }

        public async Task<string> ParseTemplate<T>(ContentType templateType, T model)
        {
            var template = await GetTemplateString(templateType);
            if (!_parser.TryParse(template, out var fluidTemplate, out var error))
            {
                throw new InvalidOperationException($"Template parsing failed: {error}");
            }

            var context = new TemplateContext(model);
            context.SetValue("model", new ObjectValue(model));

            return await fluidTemplate.RenderAsync(context);
        }

        private async Task<string> GetTemplateString(ContentType templateType)
        {
            var template = await _unitOfWork.Repository<Template>().GetBySpecificationAsync(new GetByTemplateTypeSpecification(templateType));
            if (template == null)
            {
                throw new NotFoundException(nameof(Template), templateType.ToString());
            }
            return template.TemplateBody;
        }
    }
}
