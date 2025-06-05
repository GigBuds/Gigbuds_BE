using System;
using AutoMapper;
using Gigbuds_BE.Application.DTOs.Memberships;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Interfaces.Services;
using Gigbuds_BE.Application.Specifications.Memberships;
using Gigbuds_BE.Domain.Entities.Memberships;
using Gigbuds_BE.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Gigbuds_BE.Application.Features.Memberships.Commands;

public class RegisterMembershipCommandHandler : IRequestHandler<RegisterMembershipCommand, MembershipResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMembershipsService _membershipsService;

    public RegisterMembershipCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMembershipsService membershipsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _membershipsService = membershipsService;
    }
    public async Task<MembershipResponseDto> Handle(RegisterMembershipCommand request, CancellationToken cancellationToken)
    {
        var spec = new GetMembershipByIdSpecification(request.MembershipId);
        var membership = await _unitOfWork.Repository<Membership>().GetBySpecificationAsync(spec);
        if (membership == null)
        {
            throw new NotFoundException("Membership not found");
        }
        
        var accountMembership = await _membershipsService.CreateMemberShipBenefitsAsync(request.UserId, membership);

        await _membershipsService.ScheduleMembershipExpirationAsync(request.UserId, membership);

        return accountMembership;
    }
}
