using AutoMapper;
using Gigbuds_BE.Application.DTOs;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications.JobShifts;
using Gigbuds_BE.Domain.Entities.Schedule;
using MediatR;

namespace Gigbuds_BE.Application.Features.Schedules.JobShifts.Queries.GetJobSeekerShifts
{
    internal class GetJobSeekerShiftsQueryHandler : IRequestHandler<GetJobSeekerShiftsQuery, IReadOnlyList<JobSeekerShiftsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetJobSeekerShiftsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<JobSeekerShiftsDto>> Handle(GetJobSeekerShiftsQuery request, CancellationToken cancellationToken)
        {
            var shiftDtos = await _unitOfWork.Repository<JobSeekerShift>()
                                    .GetAllWithSpecificationProjectedAsync<JobSeekerShiftsDto>
                                    (new JobSeekerShiftsSpecification(request.AccountId),
                                    _mapper.ConfigurationProvider);
            return shiftDtos;
        }
    }
}
