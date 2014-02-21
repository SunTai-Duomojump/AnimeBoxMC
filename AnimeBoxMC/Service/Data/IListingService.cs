using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeBoxMC.Service.Data
{
    /// <summary>
    /// I 操作主数据
    /// </summary>
    interface IListingService
    {
        /// <summary>
        /// 获取主数据列表
        /// </summary>
        /// <returns>数据内容</returns>
        IEnumerable<string> GetListing();
    }
}
