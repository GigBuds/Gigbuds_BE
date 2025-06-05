using Gigbuds_BE.Application.DTOs.JobPosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gigbuds_BE.Application.DTOs.JobRecommendations
{
    public class JobRecommendationScheduleDto
    {
        public required int ShiftCount { get; set; }
        public required int MinimumShift { get; set; }
        public required IReadOnlyList<JobPostShiftForRecommendDto> JobShifts { get; set; }
    }
}
