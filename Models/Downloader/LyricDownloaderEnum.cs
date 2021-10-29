using System;

namespace ZonyLrcTools.Models
{
    [Flags]
    public enum LyricDownloaderEnum
    {
        /// <summary>
        /// 网易云音乐。
        /// </summary>
        NetEase,
        /// <summary>
        /// QQ 音乐。
        /// </summary>
        QQMusic,
        /// <summary>
        /// 酷狗音乐
        /// </summary>
        KuGouMusic
    }
}
