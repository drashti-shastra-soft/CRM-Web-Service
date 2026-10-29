using System;
using System.Linq;

namespace CRMWebService.Infrastructure.CustomException
{
    [Serializable]
    public class APIRequestFailedException : Exception
    {
        public APIRequestFailedException(string message)
            : base(message) { }
    }
}
