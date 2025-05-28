using System;
using System.Data;
using FluentValidation;
using Gigbuds_BE.Application.Specifications.JobPosts;

namespace Gigbuds_BE.Application.Validators;

public class JobPostSearchParamsValidator : AbstractValidator<JobPostSearchParams>
{
    public JobPostSearchParamsValidator()
    {
        When(x => x.SalaryUnit.HasValue, () =>
        {
            RuleFor(x => x.SalaryUnit.Value).IsInEnum().WithMessage("Invalid salary unit. Valid options are: Shift, Hour, Day");
        });
    }
}
