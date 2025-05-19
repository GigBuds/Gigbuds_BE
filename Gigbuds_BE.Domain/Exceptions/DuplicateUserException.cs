namespace Gigbuds_BE.Domain.Exceptions
{
    public class DuplicateUserException(string message) : Exception(message)
    {
    }
}
