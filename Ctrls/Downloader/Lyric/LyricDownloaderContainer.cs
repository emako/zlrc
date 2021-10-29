using System.Collections.Generic;
using System.Linq;
using ZonyLrcTools.Ctrls.Downloader.Lyric.Interfaces;
using ZonyLrcTools.Ctrls.Downloader.Lyric.NetEase;
using ZonyLrcTools.Models;

namespace ZonyLrcTools.Ctrls.Downloader.Lyric
{
    /// <summary>
    /// 歌词下载器的存储容器。
    /// </summary>
    // TODO: 后续应该增加下载优先级的配置。
    public class LyricDownloaderContainer
    {
        public LyricDownloaderContainer()
        {
            Downloader = new List<ILyricDownloader>
            {
                new NetEaseCloudMusicLyricDownloader(),
                //new QQMusicCloudMusicLyricDownloader(),
                //new KuGouMusicLyricDownloader()
            };
        }

        /// <summary>
        /// 获得所有可用的歌词下载器 (<see cref="ILyricDownloader"/>) 。
        /// </summary>
        public IList<ILyricDownloader> Downloader { get; }

        /// <summary>
        /// 根据设置页面里面的歌词源，下载歌词数据。
        /// </summary>
        public ILyricDownloader GetDownloader()
        {
            // TODO:添加参数支持修改下载源
            switch (LyricDownloaderEnum.NetEase)
            {
                case LyricDownloaderEnum.NetEase:
                    return Downloader.FirstOrDefault(type => type.GetType().FullName.Contains("NetEase"));
                //case LyricDownloaderEnum.QQMusic:
                //    return Downloader.FirstOrDefault(type => type.GetType().FullName.Contains("QQMusic"));
                //case LyricDownloaderEnum.KuGouMusic:
                //    return Downloader.FirstOrDefault(type => type.GetType().FullName.Contains("KuGou"));
                //default:
                //    return Downloader.First();
            }
        }
    }
}
