using Gigbuds_BE.Application.DTOs.ApplicationUsers;
using MediatR;

namespace Gigbuds_BE.Application.Features.Utils
{
    public class JobSeekerEmbeddingRequest : IRequest
    {
        public required JobSeekerDto JobSeeker { get; set; }
    }
}
