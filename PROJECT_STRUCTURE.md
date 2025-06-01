# PROJECT_STRUCTURE.md

## Persistence Layer: UnitOfWork

- **Class:** `UnitOfWork` (in `Gigbuds_BE.Infrastructure.Persistence`)
- **Method:** `TryCompleteAsync()`
  - [HYP] Hypothesis: A 'try' variant of the commit method should attempt to save changes and return 0 on failure, logging the exception, to provide a safe, non-throwing alternative to `CompleteAsync()`.
  - [VER] Verification: Confirmed via Microsoft Docs ([source](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)) and CodeProject ([source](https://www.codeproject.com/Articles/5377618/Unit-of-Work-Pattern-in-Csharp-for-Clean-Architect)).
  - [DAT] Implementation: Catches exceptions from `_dbContext.SaveChangesAsync()`, logs the error, and returns 0 if an error occurs.
  - [RES] Result: Provides a robust, non-throwing save method for transactional operations, with error logging for diagnostics.

---

## Observations

- [DAT] The method is not part of the IUnitOfWork interface, indicating it is an internal convenience for error-tolerant save operations.
- [DAT] Logging is performed using the injected `ILoggerFactory` for traceability.

// Dry technical comment: The universe remains deterministic, but exceptions are now less catastrophic.

## Application Layer: CreateJobShiftsCommandHandler

- **Class:** `CreateJobShiftsCommandHandler` (in `Gigbuds_BE.Application.Features.Schedules.JobShifts.Commands.CreateJobShift`)
- **Method:** `Handle(CreateJobShiftsCommand, ILogger, IUnitOfWork)`
  - [HYP] Hypothesis: Using a foreach loop for inserting job shifts is more idiomatic and avoids side effects compared to LINQ Select. Fetching the repository once before the loop is more efficient.
  - [VER] Verification: Confirmed via C# best practices and Microsoft documentation ([source](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/when-to-use-foreach-vs-select)).
  - [DAT] Implementation: Refactored to use foreach, repository fetched once, and dry technical comment added for scientific rigor.
  - [RES] Result: Improved code clarity, maintainability, and efficiency. The universe remains deterministic, and job shifts are inserted with maximum scientific decorum.

// Dry technical comment: Side effects in LINQ are now a thing of the past. Order is restored to the cosmos.

## Domain Layer: NotFoundException

- **Class:** `NotFoundException` (in `Gigbuds_BE.Domain.Exceptions`)
  - [HYP] Hypothesis: A NotFoundException is required to standardize error handling for missing resources across the application. It should accept a resource name and an optional key, formatting a clear error message.
  - [VER] Verification: Confirmed by reviewing exception usage in the application layer and by referencing Microsoft Docs on custom exceptions ([source](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-user-defined-exceptions)).
  - [DAT] Implementation: `NotFoundException` created, inheriting from `Exception`, with a constructor accepting a resource name and optional key, formatting the message as: "Resource '{resourceName}' with key '{key}' was not found."
  - [RES] Result: Exception is now available for consistent not-found error handling. Linter error resolved in UpdateJobPostCommandHandler. The universe remains deterministic, and missing resources are now scientifically accounted for.

// Dry technical comment: The void of missing resources is now filled with meaningful exceptions.

## Domain Layer: UpdateFailedException

- **Class:** `UpdateFailedException` (in `Gigbuds_BE.Domain.Exceptions`)
  - [HYP] Hypothesis: A custom UpdateFailedException is required to standardize error handling for update failures across the application. It should accept a resource name and format a clear error message.
  - [VER] Verification: Confirmed by reviewing exception usage in the application layer, Microsoft Docs on custom exceptions ([source](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions)), and DDD best practices ([source](https://medium.com/@roccolangeweg/domain-driven-challenges-how-to-handle-exceptions-9c115a8cb1c9)).
  - [DAT] Implementation: `UpdateFailedException` created, inheriting from `Exception`, with a constructor accepting a resource name, formatting the message as: "Update resource {resourceName} failed."
  - [RES] Result: Exception is now available for consistent update-failure error handling. The universe remains deterministic, and failed updates are now scientifically accounted for.

// Dry technical comment: The entropy of failed updates is now contained within a single exception class.

## Domain Layer: RemoveFailedException

- **Class:** `RemoveFailedException` (in `Gigbuds_BE.Domain.Exceptions`)
  - [HYP] Hypothesis: A custom RemoveFailedException is required to standardize error handling for remove/delete failures across the application. It should accept a resource name and format a clear error message, and optionally support error chaining via an inner exception.
  - [VER] Verification: Confirmed by reviewing exception usage in the application layer, Microsoft Docs on custom exceptions ([source](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-user-defined-exceptions)), and DDD best practices ([source](https://medium.com/@roccolangeweg/domain-driven-challenges-how-to-handle-exceptions-9c115a8cb1c9)).
  - [DAT] Implementation: `RemoveFailedException` created, inheriting from `Exception`, with constructors accepting a resource name or a message and inner exception, formatting the message as: "Remove resource {resourceName} failed."
  - [RES] Result: Exception is now available for consistent remove-failure error handling. Linter errors resolved in RemoveJobPostCommandHandler. The universe remains deterministic, and failed removals are now scientifically accounted for.

// Dry technical comment: The entropy of failed removals is now contained within a single exception class. The cosmos frowns upon unhandled deletions.

## API Layer: JobPostsController - GET /api/jobposts

- **Endpoint:** `GET /api/jobposts`
- **Action:** `GetAllJobPosts`
  - [HYP] Hypothesis: Exposing a GET endpoint that accepts paging and search parameters will allow clients to retrieve a paged, filterable list of job posts. The endpoint will use MediatR to send a `GetAllJobPostsQuery` and return a `PagedResultDto<JobPostDto>`.
  - [VER] Verification: Confirmed by reviewing the controller implementation and MediatR handler logic ([source](https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0), [source](https://github.com/jbogard/MediatR)).
  - [DAT] Implementation: The controller action maps query parameters (`pageIndex`, `pageSize`, `searchTerm`) to `GetAllJobPostsQueryParams`, sends the query via MediatR, and returns the result as an HTTP 200 response. The response format is:
    ```json
    {
      "count": int,
      "data": [
        {
          "id": int,
          "jobTitle": string,
          "jobDescription": string,
          "jobRequirement": string,
          "experienceRequirement": string,
          "salary": int,
          "salaryUnit": string,
          "jobLocation": string,
          "expireTime": string,
          "benefit": string,
          "vacancyCount": int,
          "isOutstandingPost": bool,
          "jobSchedule": { /* JobScheduleDto */ }
        }
      ]
    }
    ```
  - [RES] Result: Endpoint is available for paged, filterable job post retrieval. The universe remains deterministic, and job posts are now observable in discrete, paged quanta.

// Dry technical comment: The uncertainty of job post enumeration has collapsed into a single, observable endpoint.
