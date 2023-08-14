using System;
using System.Text;
using System.Web;

namespace ZonyLrcTools.Models.MusicTag
{
    /// <summary>
    /// 歌曲信息定义，存放了软件所需要使用的歌曲元数据。
    /// </summary>
    [Serializable]
    public class MusicInfo
    {
        /// <summary>
        /// 歌曲的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 歌手，或者说是艺术家。
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// 歌曲的专辑名称。
        /// </summary>
        public string AlbumName { get; set; }

        /// <summary>
        /// 歌曲的专辑图像。
        /// </summary>
        public byte[] AlbumImage { get; set; }

        /// <summary>
        /// 歌曲对应的物理文件路径。
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 歌曲时常，单位是毫秒，主要用于酷狗音乐 API 进行搜索。
        /// </summary>
        public int Duration { get; set; }

        public MusicInfoStatus Status { get; set; }

        public MusicInfo()
        {
            Status = MusicInfoStatus.WaitingDownload;
        }

        public MusicInfo(string name, string artist) : this()
        {
            Name = name;
            Artist = artist;
        }

        public MusicInfo(string name, string artist, string albumName, byte[] albumImage, string filePath) : this(name,
            artist)
        {
            AlbumName = albumName;
            AlbumImage = albumImage;
            FilePath = filePath;
        }

        //public ListViewItem ToListViewItem()
        //{
        //    var newItem = new ListViewItem();

        //    newItem.Text = Name;
        //    newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, Artist));
        //    newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, AlbumName));
        //    newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, GetStatusString()));

        //    newItem.Tag = this;

        //    return newItem;
        //}

        public string GetStatusString()
        {
            if (Status == MusicInfoStatus.DownloadCompleted) return "下载完成";
            if (Status == MusicInfoStatus.WaitingDownload) return "等待下载";
            if (Status == MusicInfoStatus.MusicTagInvalid) return "标签信息无效";

            return "未知状态";
        }

        public string GetSearchKeyword()
        {
            return HttpUtility.UrlEncode($"{Name}+{Artist}", Encoding.UTF8);
        }
    }

    /// <summary>
    /// 歌曲当前的状态。
    /// </summary>
    [Flags]
    public enum MusicInfoStatus
    {
        /// <summary>
        /// 正在等待下载歌词/专辑图像。
        /// </summary>
        WaitingDownload,
        /// <summary>
        /// 歌词/专辑图像，已经下载完成。
        /// </summary>
        DownloadCompleted,
        /// <summary>
        /// 歌曲的标签信息无效。
        /// </summary>
        MusicTagInvalid
    }
}
