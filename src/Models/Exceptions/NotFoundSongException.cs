using System;
using ZonyLrcTools.Models.MusicTag;

namespace ZonyLrcTools.Models.Exceptions
{
    public class NotFoundSongException : Exception
    {
        public NotFoundSongException(string message, MusicInfo musicInfo) : base(message)
        {
            Data["MusicInfo"] = musicInfo;
        }
    }
}
