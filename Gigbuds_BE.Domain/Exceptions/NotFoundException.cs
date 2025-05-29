namespace Gigbuds_BE.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string resourceName, object key = null)
            : base($"Resource '{resourceName}'{(key != null ? $" with key '{key}'" : "")} was not found.")
        {
        }
    }
}