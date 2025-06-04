using Gigbuds_BE.Domain.Entities.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gigbuds_BE.Application.Specifications.JobSeekerSchedules
{
    public class JobSeekerScheduleSpecification : BaseSpecification<JobSeekerSchedule>
    {
        public JobSeekerScheduleSpecification(int jobSeekerId) : base(j => j.Id == jobSeekerId)
        {
            AddInclude(j => j.JobShifts);
        }
    }
}
