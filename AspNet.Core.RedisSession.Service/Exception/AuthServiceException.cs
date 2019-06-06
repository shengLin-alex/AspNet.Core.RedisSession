using System;

namespace AspNet.Core.RedisSession.Service
{
    /// <summary>
    /// 驗證服務發生之例外
    /// </summary>
    [Serializable]
    public class AuthServiceException : Exception
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public AuthServiceException()
        {
        }

        /// <summary>
        /// 建構子多載，包含訊息
        /// </summary>
        /// <param name="message">訊息</param>
        public AuthServiceException(string message) : base(message)
        {
        }

        /// <summary>
        /// 建構子多載，包含訊息與內部例外
        /// </summary>
        /// <param name="message">訊息</param>
        /// <param name="innerException">內部例外</param>
        public AuthServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NoColumnSpecifiedException class with a specified error message.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized object data
        /// about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual information
        /// about the source or destination.
        /// </param>
        protected AuthServiceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}