using System;

namespace Gigbuds_BE.Application.DTOs.Paging;

public class PagedResultDto<T>(int count, IReadOnlyList<T> data)
{
    public int Count { get; set; } = count;
    public IReadOnlyList<T> Data { get; set; } = data;
}
