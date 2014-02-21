using AnimeBoxMC.Service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeBoxMC.Service
{
    /// <summary>
    /// 全局逻辑工厂
    /// </summary>
    class ServiceFactory
    {
        private IListingService _ListingService;
        /// <summary>
        /// 获取主列表数据逻辑
        /// </summary>
        public IListingService ListingService
        {
            get { return _ListingService ?? new XmlListingService(); }
        }

    }
}
