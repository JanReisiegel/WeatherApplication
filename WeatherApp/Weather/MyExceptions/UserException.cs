namespace Weather.MyExceptions
{
    public class UserException(string message) : Exception
    {
        public override string Message { get; } = message;
    }
}
