namespace SelectU.Core.Exceptions
{
    public class ChangePasswordException : Exception
    {
        public ChangePasswordException()
        { }

        public ChangePasswordException(string message) : base(message)
        { }
    }
}
