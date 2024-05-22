using System;

namespace Weather.MyExceptions
{
    public class LocationException(string message) : Exception
    {
        public override string Message { get; } = message;
    }
}
