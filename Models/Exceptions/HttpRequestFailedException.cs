using System;

namespace ZonyLrcTools.Models.Exceptions
{
    /// <summary>
    /// Http 请求基类异常。
    /// </summary>
    public class HttpRequestFailedException : Exception
    {
        public HttpRequestFailedException(string message) : base(message)
        {

        }

        /// <summary>
        /// 构造一个新的 <see cref="HttpRequestFailedException"/> 对象实例。
        /// </summary>
        /// <param name="message">异常的具体信息。</param>
        /// <param name="requestParams">执行 Http 请求时传递的参数。</param>
        /// <param name="responseContent">服务器返回的数据。</param>
        public HttpRequestFailedException(string message, string requestParams, string responseContent) : this(message)
        {
            Data.Add("RequestParams", requestParams);
            Data.Add("Response", responseContent);
        }
    }
}
