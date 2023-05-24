namespace SelectU.Core.Exceptions
{
    public class ForgotPasswordException : Exception
    {
        public ForgotPasswordException()
        { }

        public ForgotPasswordException(string message) : base(message)
        { }
    }
}
