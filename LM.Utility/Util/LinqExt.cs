using System;
using System.Collections.Generic;

namespace LM.Utility.Util
{
    public static class LinqExt
    {
        /// <summary>
        ///  集合的 ForEach 实现。
        ///  eg:
        ///  list.ForEach(e => { if (e > 0) e++; });
        /// </summary>
        /// <typeparam name="T">集合中的元素类型</typeparam>
        /// <param name="source">集合</param>
        /// <param name="action">对每个元素执行的操作</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            foreach (var item in source)
            {
                action(item);
            }
            var list = new List<object>();
            for (var i = 0; i < 10; i++)
            {
                list.Add(new { Name = "a" + 1 });
            }
        }


    }
}
