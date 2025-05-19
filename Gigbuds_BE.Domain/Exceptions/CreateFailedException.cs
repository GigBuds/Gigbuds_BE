namespace Gigbuds_BE.Domain.Exceptions
{
    public class CreateFailedException(string resourceName) : Exception($"Create resource {resourceName} failed")
    {
    }
}
