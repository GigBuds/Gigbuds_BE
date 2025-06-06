namespace Gigbuds_BE.Application.Interfaces.Services
{
    public interface ITemplatingService
    {
        /// <summary>
        /// Replaces placeholders in a template string with their corresponding values.
        /// </summary>
        /// <param name="template">The template string containing placeholders to be replaced.</param>
        /// <param name="placeholders">A dictionary mapping placeholder keys to their replacement values.</param>
        /// <returns>The template string with all placeholders replaced by their corresponding values.</returns>
        string ParseTemplate<T>(string template, T model);
    }
}
