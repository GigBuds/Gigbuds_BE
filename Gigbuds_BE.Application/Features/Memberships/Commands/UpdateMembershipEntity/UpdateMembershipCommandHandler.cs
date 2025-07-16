using System;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.Memberships;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;

namespace Gigbuds_BE.Application.Features.Memberships.Commands.UpdateMembershipEntity;

public class UpdateMembershipCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateMembershipCommand, bool>
{
    public async Task<bool> Handle(UpdateMembershipCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetMembershipByIdSpecification(request.Id);
        var membership = await unitOfWork.Repository<Membership>().GetBySpecificationAsync(spec);
        if (membership == null) throw new NotFoundException("Membership not found");
        membership.Title = request.Title;
        membership.Description = request.Description;
        membership.Price = request.Price;
        membership.Duration = request.Duration;
        unitOfWork.Repository<Membership>().Update(membership);
        await unitOfWork.CompleteAsync();
        return true;
    }
}
