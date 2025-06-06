using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Gigbuds_BE.Application.Features.JobApplications.Commands
{
    public class CheckJobApplicationCommand : IRequest<bool>
    {
        public int AccountId { get; set; }
        public int JobPostId { get; set; }

        public CheckJobApplicationCommand(int accountId, int jobPostId)
        {
            AccountId = accountId;
            JobPostId = jobPostId;
        }
    }
}
