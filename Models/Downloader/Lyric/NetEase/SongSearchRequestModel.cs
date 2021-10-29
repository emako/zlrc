using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace ZonyLrcTools.Models
{
    public class SongSearchRequestModel
    {
        /// <summary>
        /// CSRF 标识，一般为空即可，接口不会进行校验。
        /// </summary>
        [JsonProperty("csrf_token")]
        public string CSRFToken { get; set; }

        /// <summary>
        /// 需要搜索的内容，一般是歌曲名 + 歌手的格式。
        /// </summary>
        [JsonProperty("s")]
        public string SearchKey { get; set; }

        /// <summary>
        /// 页偏移量。
        /// </summary>
        [JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// 搜索类型。
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// 是否获取全部的搜索结果。
        /// </summary>
        [JsonProperty("total")]
        public bool IsTotal { get; set; }

        /// <summary>
        /// 每页的最大结果容量。
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        public SongSearchRequestModel()
        {
            CSRFToken = string.Empty;
            Type = 1;
            Offset = 0;
            IsTotal = true;
            Limit = 5;
        }

        public SongSearchRequestModel(string musicName, string artistName) : this()
        {
            SearchKey = HttpUtility.UrlEncode($"{musicName}+{artistName}", Encoding.UTF8);
        }
    }
}
