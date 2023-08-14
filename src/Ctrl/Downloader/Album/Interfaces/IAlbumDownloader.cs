using System.Threading.Tasks;
using ZonyLrcTools.Models.MusicTag;

namespace ZonyLrcTools.Ctrls.Downloader.Album.NetEase.Interfaces
{
    public interface IAlbumDownloader
    {
        Task<byte[]> DownloadAsync(MusicInfo musicInfo);

        byte[] Download(MusicInfo musicInfo);
    }
}
