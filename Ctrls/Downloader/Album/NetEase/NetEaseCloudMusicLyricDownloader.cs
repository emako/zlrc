using Newtonsoft.Json.Linq;
using Nito.AsyncEx;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZonyLrcTools.Ctrls.Downloader.Album.NetEase.Interfaces;
using ZonyLrcTools.Ctrls.Networks;
using ZonyLrcTools.Models;
using ZonyLrcTools.Models.Downloader.Album.NetEase;
using ZonyLrcTools.Models.Exceptions;
using ZonyLrcTools.Models.MusicTag;

namespace ZonyLrcTools.Ctrls.Downloader.Album.NetEase
{
    /// <summary>
    /// 基于网易云音乐实现的专辑下载器，用于下载歌曲的专辑封面。
    /// </summary>
    public class NetEaseCloudMusicAlbumDownloader : IAlbumDownloader
    {
        private readonly WrappedHttpClient _wrappedHttpClient;

        public NetEaseCloudMusicAlbumDownloader()
        {
            _wrappedHttpClient = new WrappedHttpClient();
        }

        public async Task<byte[]> DownloadAsync(MusicInfo musicInfo)
        {
            var requestParameter = new SongSearchRequestModel(musicInfo.Name, musicInfo.Artist);
            var searchResult = await _wrappedHttpClient.PostAsync<MusicSearchResponseModel>(
                url: @"https://music.163.com/api/search/get/web",
                parameters: requestParameter,
                isBuildForm: true,
                refererUrl: @"https://music.163.com",
                mediaTypeValue: "application/x-www-form-urlencoded");

            if (searchResult == null || searchResult.StatusCode != 200 || searchResult.Items == null)
            {
                throw new RequestErrorException("网易云音乐接口没有正常返回结果...", musicInfo);
            }

            if (searchResult.Items.SongCount <= 0) throw new NotFoundSongException("没有搜索到指定的歌曲。", musicInfo);

            var songDetailJsonStr = await _wrappedHttpClient.GetAsync(
                @"https://music.163.com/api/song/detail",
                new MusicGetSongDetailsRequest(searchResult.GetFirstSongId()),
                @"https://music.163.com");

            var url = JObject.Parse(songDetailJsonStr).SelectToken("$.songs[0].album.picUrl")?.Value<string>();
            if (string.IsNullOrEmpty(url))
            {
                throw new RequestErrorException("网易云音乐接口没有正常返回结果...", musicInfo);
            }

            return await new HttpClient().GetByteArrayAsync(new Uri(url));
        }

        public byte[] Download(MusicInfo musicInfo) => AsyncContext.Run(() => DownloadAsync(musicInfo));
    }
}
