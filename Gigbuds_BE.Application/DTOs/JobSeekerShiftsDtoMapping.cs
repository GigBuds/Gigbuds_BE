using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gigbuds_BE.Application.DTOs
{
    public class JobSeekerShiftsDtoMapping
    {
        public required int JobSeekerId { get; set; }
        public required int DayOfWeek { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
    }
}
