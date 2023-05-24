namespace SelectU.Core.Exceptions
{
    public class UserRegisterException : Exception
    {
        public UserRegisterException()
        { }

        public UserRegisterException(string message) : base(message)
        { }
    }
}
