using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gigbuds_BE.Application.Specifications.ApplicationUsers
{
    public class EmployerGetByNameQueryParams : BasePagingParams
    {
        public string? Name { get; set; } = string.Empty;
    }
}