using System;
using System.ComponentModel.DataAnnotations;
using Gigbuds_BE.Domain.Entities.Jobs;
namespace Gigbuds_BE.Application.Specifications.JobPosts;

public class JobPostSearchParams : BasePagingParams
{
    public string? EmployerName { get; set; }
    public string? JobName { get; set; }
    public decimal? SalaryFrom { get; set; }
    public decimal? SalaryTo { get; set; }
    public bool? IsMale { get; set; }
    public DateTime? JobTimeFrom { get; set; }
    public DateTime? JobTimeTo { get; set; }
    public SalaryUnit? SalaryUnit { get; set; }
    public int? JobPositionId { get; set; }
    public int? AgeFrom { get; set; }
    public int? AgeTo { get; set; }
    public IEnumerable<string> DistrictCodeList { get; set; } = new List<string>();
}
