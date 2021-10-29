namespace ZonyLrcTools.Models.Exceptions
{
    /// <summary>
    /// 当服务器返回 503 错误时，抛出本异常，代表当前用户 IP 已经被限制。
    /// </summary>
    public class ServerUnavailableException : HttpRequestFailedException
    {
        public ServerUnavailableException(string message) : base(message)
        {
        }

        public ServerUnavailableException(string message, string requestParams, string responseContent) : base(message, requestParams, responseContent)
        {
        }
    }
}
