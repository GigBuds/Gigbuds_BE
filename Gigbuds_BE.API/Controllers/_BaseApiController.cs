﻿using Asp.Versioning;
using Gigbuds_BE.API.Helpers.RequestHelpers;
using Microsoft.AspNetCore.Mvc;

namespace Gigbuds_BE.API.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class _BaseApiController : ControllerBase
    {
        /// <summary>
        /// Returns an <see cref="ActionResult"/> containing paginated data and pagination metadata.
        /// </summary>
        /// <typeparam name="T">The type of items in the paginated list.</typeparam>
        /// <param name="items">The list of items for the current page.</param>
        /// <param name="count">The total number of items available.</param>
        /// <param name="pageIndex">The current page index (one-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>An <see cref="OkObjectResult"/> containing a <see cref="Pagination{T}"/> object.</returns>
        protected ActionResult ResultWithPagination<T>(IReadOnlyList<T> items, int count, int pageIndex, int pageSize)
        {
            var pagination = new Pagination<T>(items, count, pageIndex, pageSize);
            return Ok(pagination);
        }
    }

}
