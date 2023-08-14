using System;

namespace ZonyLrcTools.Models.Converts
{
    public static class StringConvert
    {
        /// <summary>
        /// 针对 <see cref="string.IsNullOrEmpty"/> 方法和 <see cref="string.IsNullOrWhiteSpace"/> 的扩展，将两者结合起来。
        /// 只有当字符串满足两个方法的条件之后，结果才会返回 true。
        /// </summary>
        public static bool IsNullOrEmptyOrWhite(this string srcStr)
        {
            return string.IsNullOrEmpty(srcStr) && string.IsNullOrWhiteSpace(srcStr);
        }

        /// <summary>
        /// 针对 <see cref="string.TrimEnd"/> 方法的扩展，原方法只能使用 <see cref="char"/> 来去除字符串末尾不需要的数据。
        /// </summary>
        /// <param name="srcStr">需要去除末尾数据的源字符串。</param>
        /// <param name="trimEndStr">字符串末尾需要去除的字符串。</param>
        /// <returns>去除完成的字符串结果。</returns>
        public static string TrimEnd(this string srcStr, string trimEndStr)
        {
            if (srcStr.EndsWith(trimEndStr, StringComparison.Ordinal))
            {
                return srcStr.Substring(0, srcStr.Length - trimEndStr.Length);
            }

            return srcStr;
        }
    }
}
