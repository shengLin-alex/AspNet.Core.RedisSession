using System;

namespace AspNet.Core.RedisSession.Service.Model
{
    public interface IResult
    {
        Exception Exception { get; set; }
        
        string Message { get; }

        bool Success { get; set; }

        void SetError(string errorMessage, Exception exc = null);

        void SetMessage(string message);
    }
}