namespace Gigbuds_BE.Domain.Exceptions
{
    public class UpdateFailedException(string resourceName) : Exception($"Update resource {resourceName} failed")
    {
    }
}