﻿using System;
using System.Text.RegularExpressions;

namespace ZonyLrcTools.Models
{
    /// <summary>
    /// 歌词条目的定义，歌词条目是歌词信息的最小单位，一个完整的
    /// 歌词数据是由诸多歌词条目组合而成。
    /// </summary>
    public class LyricItem : IComparable<LyricItem>
    {
        /// <summary>
        /// 原始时间轴
        /// </summary>
        public string OriginalTimeline { get; private set; }

        /// <summary>
        /// 排序时间轴
        /// </summary>
        public double SortTimeLine { get; private set; }

        /// <summary>
        /// 歌词文本数据
        /// </summary>
        public string LyricText { get; private set; }

        /// <summary>
        /// 歌词条目时间轴 - 分
        /// </summary>
        public int Minute { get; private set; }

        /// <summary>
        /// 歌词条目时间轴 - 秒
        /// </summary>
        public double Second { get; private set; }

        /// <summary>
        /// 构造歌词条目
        /// </summary>
        /// <param name="originalData"></param>
        public LyricItem(string originalData)
        {
            ResolveTimeLine(originalData);
        }

        /// <summary>
        /// 构造歌词条目
        /// </summary>
        public LyricItem(int minute, double second, string lyricText)
        {
            Minute = minute;
            Second = second;
            LyricText = lyricText;
            SortTimeLine = Minute * 60.00 + Second;
            OriginalTimeline = $"[{Minute:00}:{Second:00.00}]";
        }

        public static LyricItem operator +(LyricItem src, LyricItem dist)
        {
            return new LyricItem(src.Minute, src.Second, $"{src.LyricText}  {dist.LyricText}");
        }

        private void ResolveTimeLine(string originalData)
        {
            var timeline = new Regex(@"\[\d+:\d+.\d+\]").Match(originalData)
                .Value.Replace("]", string.Empty)
                .Replace("[", string.Empty)
                .Split(':');

            if (int.TryParse(timeline[0], out int minute)) Minute = minute;
            if (double.TryParse(timeline[1], out double second)) Second = second;

            SortTimeLine = Minute * 60.00 + Second;
            OriginalTimeline = $"[{Minute:00}:{Second:00.00}]";
            LyricText = new Regex(@"(?<=\[\d+:\d+.\d+\]).+").Match(originalData).Value;
        }

        public int CompareTo(LyricItem other)
        {
            if (SortTimeLine > other.SortTimeLine)
            {
                return 1;
            }

            if (SortTimeLine < other.SortTimeLine)
            {
                return -1;
            }

            return 0;
        }

        public override string ToString()
        {
            return $"[{Minute:00}:{Second:00.00}]{LyricText}";
        }
    }
}
