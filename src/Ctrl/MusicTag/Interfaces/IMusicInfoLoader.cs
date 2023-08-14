using System.Threading.Tasks;
using ZonyLrcTools.Models.MusicTag;

namespace ZonyLrcTools.Ctrls.MusicTag.Interfaces
{
    public interface IMusicInfoLoader
    {
        MusicInfo Load(string musicFilePath);

        Task<MusicInfo> LoadAsync(string musicFilePath);

        void Save(MusicInfo musicInfo);

        Task SaveAsync(MusicInfo musicInfo);
    }
}
