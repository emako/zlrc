using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ZonyLrcTools.Models.Converts;

namespace ZonyLrcTools.Models
{
    /// <summary>
    /// 歌词条目集合，准确来说就是一首歌的全部歌词。当调用本类的 <see cref="ToString"/> 时，
    /// 就会构建具体的歌词数据。
    /// </summary>
    public class LyricItemCollection : List<LyricItem>
    {
        /// <summary>
        /// 歌曲是否是没有歌词的纯音乐。
        /// </summary>
        public bool IsPureMusic { get; private set; }

        /// <summary>
        /// 构造一个新的 <see cref="LyricItemCollection"/> 对象实例。
        /// </summary>
        public LyricItemCollection()
        {

        }

        public LyricItemCollection(string srcLyricText)
        {
            if (srcLyricText.IsNullOrEmptyOrWhite())
            {
                IsPureMusic = true;
                return;
            }

            // 根据正则获取所有的歌词条目，并将其加入到集合当中。
            var regex = new Regex(@"\[\d+:\d+.\d+\].+\n?");
            foreach (Match match in regex.Matches(srcLyricText))
            {
                Add(new LyricItem(match.Value));
            }
        }

        /// <summary>
        /// 尝试与另一个歌词集合进行合并，常用于双语歌词合并动作。
        /// </summary>
        /// <param name="otherCollection">待合并的歌词条目集合。</param>
        /// <param name="intoOneLine">双语歌词是否合并为一行。</param>
        /// <remarks>
        /// <see cref="intoOneLine"/>参数只能保证时间轴一致的情况能够合并为一行，如果不一致
        /// 则可能仍会分为两行进行处理。合并处理依据以源歌词为 <c>基准</c> 进行，一旦出现源歌词与翻译歌词数目不匹配
        /// 的情况会造成分行处理，但最终的结果仍然以升序排列。
        /// </remarks>
        /// <returns> 合并成功的歌词结果 </returns>
        public LyricItemCollection Merge(LyricItemCollection otherCollection, bool intoOneLine = true)
        {
            if (otherCollection.IsPureMusic) return this;
            var lyricItemCollection = new LyricItemCollection();
            var indexDiff = this.Count - otherCollection.Count;

            if (intoOneLine)
            {
                // 如果索引数相等，直接根据索引快速构建。
                if (indexDiff == 0)
                {
                    for (int index = 0; index < this.Count; index++)
                    {
                        lyricItemCollection.Add(this[index] + otherCollection[index]);
                    }

                    return lyricItemCollection;
                }

                // 处理存在两种情况的歌词，一种是时间轴匹配进行合并，剩余的则按照未处理的顺序进行添加。
                var srcMarkDict = new Dictionary<int, bool>();
                var distMarkDict = new Dictionary<int, bool>();
                for (int srcIndex = 0; srcIndex < this.Count; srcIndex++)
                {
                    srcMarkDict.Add(srcIndex, false);
                }

                for (int distIndex = 0; distIndex < otherCollection.Count; distIndex++)
                {
                    distMarkDict.Add(distIndex, false);
                }

                // 优先处理匹配项
                for (int srcIndex = 0; srcIndex < this.Count; srcIndex++)
                {
                    var otherItem = otherCollection.Find(x => x.SortTimeLine == this[srcIndex].SortTimeLine);
                    if (otherItem != null)
                    {
                        lyricItemCollection.Add(this[srcIndex] + otherItem);

                        var distIndex = otherCollection.FindIndex(x => x == otherItem);
                        distMarkDict[distIndex] = true;
                    }
                    else
                    {
                        lyricItemCollection.Add(this[srcIndex]);
                    }

                    srcMarkDict[srcIndex] = true;
                }

                // 遍历处理未匹配条目
                var srcWaitProcessIndex = srcMarkDict.Where(item => item.Value == false).Select(pair => pair.Key).ToList();
                var distWaitProcessIndex = distMarkDict.Where(item => item.Value == false).Select(pair => pair.Key).ToList();

                srcWaitProcessIndex.ForEach(index => lyricItemCollection.Add(this[index]));
                distWaitProcessIndex.ForEach(index => lyricItemCollection.Add(otherCollection[index]));

                lyricItemCollection.Sort();
                return lyricItemCollection;
            }

            ForEach(item => lyricItemCollection.Add(item));
            otherCollection.ForEach(item => lyricItemCollection.Add(item));

            lyricItemCollection.Sort();
            return lyricItemCollection;
        }

        /// <summary>
        /// 基于所有歌词条目，来生成具体的歌词数据。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var lineBreak = Environment.NewLine;

            var lyricBuilder = new StringBuilder();
            ForEach(item => lyricBuilder.Append(item).Append(lineBreak));
            return lyricBuilder.ToString().TrimEnd(lineBreak);
        }
    }
}
