using Gigbuds_BE.Domain.Entities.Identity;

namespace Gigbuds_BE.Application.Interfaces.Services
{
    public interface IApplicationUserService<T> where T : ApplicationUser
    {
        #region Core methods
        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A read-only list of all users.</returns>
        Task<IReadOnlyList<T>> GetAllUsersAsync();

        /// <summary>
        /// Retrieves users by their role asynchronously.
        /// </summary>
        /// <param name="role">The role of the users to retrieve.</param>
        /// <returns>A list of users with the specified role.</returns>
        Task<IList<ApplicationUser>> GetUsersByRoleAsync(string role);

        /// <summary>
        /// Retrieves all users that match a given specification asynchronously.
        /// </summary>
        /// <param name="spec">The specification to filter users.</param>
        /// <returns>A read-only list of users that match the specification.</returns>
        Task<IReadOnlyList<T>> GetAllUsersWithSpecAsync(ISpecification<T> spec);

        /// <summary>
        /// Retrieves a single user that matches a given specification asynchronously.
        /// </summary>
        /// <param name="spec">The specification to filter the user.</param>
        /// <returns>The user that matches the specification, or null if no user matches.</returns>
        Task<T?> GetUserWithSpec(ISpecification<T> spec);

        /// <summary>
        /// Counts the number of users that match a given specification asynchronously.
        /// </summary>
        /// <param name="spec">The specification to filter users.</param>
        /// <returns>The count of users that match the specification.</returns>
        Task<int> CountAsync(ISpecification<T> spec);

        /// <summary>
        /// Inserts a new Job Seeker with the specified password asynchronously.
        /// </summary>
        /// <param name="user">The user to insert.</param>
        /// <param name="password">The password for the new user.</param>
        Task InsertJobSeekerAsync(ApplicationUser user, string password);

        /// <summary>
        /// Inserts a new Employer with the specified password asynchronously.
        /// </summary>
        /// <param name="user">The user to insert.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="companyEmail">The company email for the new user.</param>
        Task InsertEmployerAsync(ApplicationUser user, string password, string companyEmail);

        /// <summary>
        /// Assigns a role to a user asynchronously.
        /// </summary>
        /// <param name="user">The user to assign the role to.</param>
        /// <param name="role">The role to assign.</param>
        Task AssignRoleAsync(T user, string role);

        /// <summary>
        /// Updates an existing user's information asynchronously.
        /// </summary>
        /// <param name="user">The user with updated information.</param>
        Task UpdateAsync(ApplicationUser user);

        /// <summary>
        /// Retrieves a user by their email address asynchronously.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>The user with the specified email, or null if not found.</returns>
        Task<ApplicationUser?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Gets the role of a specified user asynchronously.
        /// </summary>
        /// <param name="user">The user whose role to retrieve.</param>
        /// <returns>The role of the user.</returns>
        Task<string> GetUserRoleAsync(T user);

        /// <summary>
        /// Retrieves a user by their ID asynchronously, with optional navigation property includes.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <param name="includes">A list of navigation property names to include, or null to disable includes.</param>
        /// <param name="isTracking">Whether to use tracking or not.</param>
        /// <returns>The user with the specified ID, or null if not found.</returns>
        Task<ApplicationUser?> GetByIdAsync(int userId, List<string>? includes = null, bool isTracking = false);

        /// <summary>
        /// Inserts a new user with the specified password asynchronously.
        /// </summary>
        /// <param name="user">The user to insert.</param>
        /// <param name="password">The password for the new user.</param>
        Task InsertAsync(ApplicationUser user, string password);
        #endregion

        Task<string> GetJobSeekerMembershipLevelAsync(int userId);
        Task<string> GetEmployerMembershipLevelAsync(int userId);
    }
}
