﻿using Fluid;
using Fluid.Values;
using Gigbuds_BE.Application.Interfaces.Services;

namespace Gigbuds_BE.Infrastructure.Services
{
    internal class TemplatingService : ITemplatingService
    {
        private readonly FluidParser _parser;

        public TemplatingService()
        {
            _parser = new FluidParser();
        }

        public string ParseTemplate<T>(string template, T model)
        {
            if (!_parser.TryParse(template, out var fluidTemplate, out var error))
            {
                throw new InvalidOperationException($"Template parsing failed: {error}");
            }

            var context = new TemplateContext(model);
            context.SetValue("model", new ObjectValue(model));

            return fluidTemplate.Render(context);
        }
    }
}
