namespace Weather.MyExceptions
{
    public class HistoryException(string text) : Exception
    {
        public override string Message { get; } = text;
    }
}
