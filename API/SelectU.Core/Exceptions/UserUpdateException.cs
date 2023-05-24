namespace SelectU.Core.Exceptions
{
    public class UserUpdateException : Exception
    {
        public UserUpdateException()
        { }

        public UserUpdateException(string message) : base(message)
        { }
    }
}
