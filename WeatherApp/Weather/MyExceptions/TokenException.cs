namespace Weather.MyExceptions
{
    public class TokenException(string message) : Exception
    {
        public override string Message { get; } = message;
    }
}
