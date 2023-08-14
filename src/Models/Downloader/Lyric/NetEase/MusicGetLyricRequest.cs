namespace ZonyLrcTools.Models
{
    public class MusicGetLyricRequest
    {
        public MusicGetLyricRequest(int songId)
        {
            os = "osx";
            id = songId;
            lv = kv = tv = -1;
        }

        /// <summary>
        /// 请求的操作系统。
        /// </summary>
        public string os { get; }
        /// <summary>
        /// 歌曲的 SID 值。
        /// </summary>
        public int id { get; }

        public int lv { get; }
        public int kv { get; }
        public int tv { get; }
    }
}
