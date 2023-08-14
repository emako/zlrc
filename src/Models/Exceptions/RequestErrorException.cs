using System;
using ZonyLrcTools.Models.MusicTag;

namespace ZonyLrcTools.Models.Exceptions
{
    public class RequestErrorException : Exception
    {
        public RequestErrorException(string message, MusicInfo musicInfo, Exception exception = null) : base(message, exception)
        {
            Data["MusicInfo"] = musicInfo;
        }
    }
}
