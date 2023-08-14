using System.Threading.Tasks;
using ZonyLrcTools.Models;
using ZonyLrcTools.Models.MusicTag;

namespace ZonyLrcTools.Ctrls.Downloader.Lyric.Interfaces
{
    /// <summary>
    /// 歌词下载器的接口定义。
    /// </summary>
    public interface ILyricDownloader
    {
        Task<LyricItemCollection> DownloadAsync(MusicInfo musicInfo);
    }
}
