using System.Threading.Tasks;
using ZonyLrcTools.Ctrls.Downloader.Lyric.Interfaces;
using ZonyLrcTools.Ctrls.Networks;
using ZonyLrcTools.Models;
using ZonyLrcTools.Models.Exceptions;
using ZonyLrcTools.Models.MusicTag;

namespace ZonyLrcTools.Ctrls.Downloader.Lyric.NetEase
{
    /// <summary>
    /// 网易云音乐歌词下载器的具体实现，可以通过网易云音乐下载指定的歌词数据。
    /// </summary>
    public class NetEaseCloudMusicLyricDownloader : ILyricDownloader
    {
        private readonly WrappedHttpClient _wrappedHttpClient = new WrappedHttpClient();

        public async Task<LyricItemCollection> DownloadAsync(MusicInfo musicInfo)
        {
            if (string.IsNullOrEmpty(musicInfo.Name) && string.IsNullOrEmpty(musicInfo.Artist))
            {
                throw new NotFoundSongException("没有搜索到指定的歌曲。", musicInfo);
            }

            var requestParameter = new SongSearchRequestModel(musicInfo.Name, musicInfo.Artist);
            var searchResult = await _wrappedHttpClient.PostAsync<MusicSearchResponseModel>(
                url: @"https://music.163.com/api/search/get/web",
                parameters: requestParameter,
                isBuildForm: true,
                refererUrl: @"https://music.163.com",
                mediaTypeValue: "application/x-www-form-urlencoded");

            ValidateResponse(searchResult, musicInfo);

            var lyricJsonObj = await _wrappedHttpClient.GetAsync<MusicGetLyricResponse>(
                url: @"https://music.163.com/api/song/lyric",
                parameters: new MusicGetLyricRequest(searchResult.GetFirstSongId()),
                refererUrl: @"https://music.163.com");

            if (lyricJsonObj?.OriginalLyric == null) return new LyricItemCollection(string.Empty);

            // TODO
            // 确认歌词的构建方式，根据不同的方式返回不同的歌词结果。
            //if (AppConfiguration.Instance.Configuration.LyricContentType == LyricContentTypes.Original)
            //{
            //    return new LyricItemCollection(lyricJsonObj.OriginalLyric.Text);
            //}

            //if (AppConfiguration.Instance.Configuration.LyricContentType == LyricContentTypes.Translation && lyricJsonObj.TranslationLyric != null)
            //{
            //    return new LyricItemCollection(lyricJsonObj.TranslationLyric.Text);
            //}

            return new LyricItemCollection(lyricJsonObj.OriginalLyric.Text).Merge(new LyricItemCollection(lyricJsonObj.TranslationLyric?.Text));
        }

        protected virtual void ValidateResponse(MusicSearchResponseModel searchResult, MusicInfo musicInfo)
        {
            if (searchResult == null || searchResult.StatusCode != 200 || searchResult.Items == null)
            {
                throw new RequestErrorException("网易云音乐接口没有正常返回结果...", musicInfo);
            }

            if (searchResult.Items.SongItems == null || (searchResult.Items.SongItems?.Count <= 0 && searchResult.Items.SongCount <= 0))
            {
                throw new NotFoundSongException("没有搜索到指定的歌曲。", musicInfo);
            }
        }
    }
}
