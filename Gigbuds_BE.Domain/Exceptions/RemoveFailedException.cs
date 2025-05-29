namespace Gigbuds_BE.Domain.Exceptions
{
    public class RemoveFailedException : Exception
    {
        public RemoveFailedException(string resourceName)
            : base($"Remove resource {resourceName} failed")
        {
        }

        public RemoveFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}