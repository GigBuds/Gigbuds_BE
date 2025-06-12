using FluentValidation;

namespace Gigbuds_BE.Application.Features.Memberships.Commands.CreateMembershipPayment;

public class CreateMembershipPaymentCommandValidator : AbstractValidator<CreateMembershipPaymentCommand>
{
    public CreateMembershipPaymentCommandValidator()
    {
        RuleFor(x => x.MembershipId)
            .GreaterThan(0)
            .WithMessage("Membership ID must be greater than 0");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0");

        RuleFor(x => x.BuyerEmail)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.BuyerEmail))
            .WithMessage("Invalid email format");

        RuleFor(x => x.BuyerPhone)
            .Matches(@"^(\+84|84|0)[3|5|7|8|9][0-9]{8}$")
            .When(x => !string.IsNullOrEmpty(x.BuyerPhone))
            .WithMessage("Invalid Vietnamese phone number format");
    }
} 