using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeBoxMC.Model
{
    /// <summary>
    /// 基本歌曲的描述
    /// </summary>
    public class SongBase
    {
        /// <summary>
        /// 歌曲艺术家
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// 歌曲标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文件存放路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 专辑
        /// </summary>
        public string Album { get; set; }
        public string Source { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public bool Like { get; set; }
    }
}
