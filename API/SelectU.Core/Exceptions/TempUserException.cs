namespace SelectU.Core.Exceptions
{
    public class TempUserException : Exception
    {
        public TempUserException()
        { }

        public TempUserException(string message) : base(message)
        { }
    }
}
