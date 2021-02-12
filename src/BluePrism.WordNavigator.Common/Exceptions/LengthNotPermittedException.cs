using System;

namespace BluePrism.WordNavigator.Common.Exceptions
{
    public class LengthNotPermittedException : Exception
    {
        public LengthNotPermittedException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}
