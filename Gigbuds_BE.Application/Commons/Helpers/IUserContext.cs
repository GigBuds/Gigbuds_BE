namespace Gigbuds_BE.Application.Commons.Helpers
{
    public interface IUserContext
    {
        /// <summary>
        /// Retrieves information about the currently authenticated user from the HTTP context.
        /// Returns <c>null</c> if the user is not authenticated.
        /// </summary>
        /// <returns>
        /// A <see cref="CurrentUser"/> object containing the user's ID, email, and roles, or <c>null</c> if unauthenticated.
        /// </returns>
        CurrentUser? GetCurrentUser();
    }
}
