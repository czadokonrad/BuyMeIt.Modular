using System;

namespace BuyMeIt.BuildingBlocks.Domain
{
    public class DomainException: Exception
    {
        public Exception Exception { get; }
        public string ErrorMessage { get; }
        
        public DomainException(string errorMessage, Exception exception = null) : base(errorMessage)
        {
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public override string ToString()
        {
            return $"{ErrorMessage}";
        }

        public string ToDetailedString() => $"{ToString()} {Exception?.Message}";
    }
}