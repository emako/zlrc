using System.IO;
using System.Threading.Tasks;
using TagLib;
using ZonyLrcTools.Ctrls.MusicTag.Interfaces;
using ZonyLrcTools.Models.MusicTag;
using TagFile = TagLib.File;
using TagPicture = TagLib.Picture;

namespace ZonyLrcTools.Ctrls.MusicTag
{
    public class MusicInfoLoaderByTagLib : IMusicInfoLoader
    {
        public MusicInfo Load(string musicFilePath)
        {
            var result = new MusicInfo();
            var fileName = Path.GetFileNameWithoutExtension(musicFilePath);

            TagFile tagFile = null;
            try
            {
                tagFile = TagFile.Create(musicFilePath);
                result.Name = tagFile.Tag.Title;
                result.Artist = tagFile.Tag.FirstPerformer;
                if (!string.IsNullOrEmpty(tagFile.Tag.FirstAlbumArtist)) result.Artist = tagFile.Tag.FirstAlbumArtist;
                result.AlbumName = tagFile.Tag.Album;
                result.Duration = (int)tagFile.Properties.Duration.TotalMilliseconds;
            }
            catch (CorruptFileException)
            {
                // 可能存在标签文件损坏的情况，忽略处理。
                result.Status = MusicInfoStatus.MusicTagInvalid;
            }
            finally
            {
                // 如果都没有标签信息，则使用文件名作为歌手名和歌曲名。
                if (string.IsNullOrEmpty(result.Name)) result.Name = fileName;
                if (string.IsNullOrEmpty(result.Artist)) result.Artist = fileName;

                if (tagFile?.Tag.Pictures.Length > 0)
                {
                    result.AlbumImage = tagFile.Tag.Pictures[0].Data.Data;
                }

                result.FilePath = musicFilePath;
                tagFile?.Dispose();
            }

            return result;
        }

        public Task<MusicInfo> LoadAsync(string musicFilePath)
        {
            return Task.FromResult(Load(musicFilePath));
        }

        public void Save(MusicInfo musicInfo)
        {
            var tagFile = TagFile.Create(musicInfo.FilePath);

            tagFile.Tag.Title = musicInfo.Name;
            tagFile.Tag.Performers = new[] { musicInfo.Artist };
            tagFile.Tag.Album = musicInfo.AlbumName;
            tagFile.Tag.Pictures = new IPicture[] { new TagPicture(musicInfo.AlbumImage) };

            tagFile.Save();
        }

        public Task SaveAsync(MusicInfo musicInfo)
        {
            Save(musicInfo);

            return Task.FromResult(0);
        }
    }
}
