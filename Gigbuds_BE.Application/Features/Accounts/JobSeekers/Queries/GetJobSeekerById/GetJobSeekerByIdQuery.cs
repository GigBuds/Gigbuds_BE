using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using MediatR;

namespace Gigbuds_BE.Application.Features.Accounts.JobSeekers.Queries.GetJobSeekerById
{
    public class GetJobSeekerByIdQuery : IRequest
    {
        public int Id {  get; set; }
    }
}
