using System;
using System.Text;
using Power.Mvc.Helper.Extensions;

namespace AspNet.Core.RedisSession.Service.Model
{
    /// <summary>
    /// 服務致行結果
    /// </summary>
    public class Result : IResult
    {
        public Exception Exception { get; set; }

        public string Message => this.MessageBuilder.ToString();

        public bool Success { get; set; } = true;

        private StringBuilder MessageBuilder { get; set; } = new StringBuilder();

        public void SetError(string errorMessage, Exception exc = null)
        {
            this.Success = false;
            this.Exception = exc;
            this.SetMessage(errorMessage);
        }

        public void SetMessage(string message)
        {
            if (message.IsNullOrEmpty()) return;
            
            if (this.MessageBuilder.Length > 0)
            {
                this.MessageBuilder.AppendLine();
            }

            this.MessageBuilder.Append(message);
        }
    }
}