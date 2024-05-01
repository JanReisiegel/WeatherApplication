using System;

namespace Weather.Services
{
    public class LocationException : Exception
    {
        public string Message { get; set; }
        public LocationException(string message)
        {
            Message = message;
        }
    }
}
