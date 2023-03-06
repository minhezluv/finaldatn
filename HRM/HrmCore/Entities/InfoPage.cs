using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Entities
{
    public class InfoPage
    {
        #region Property

        /// <summary>
        /// tổng số trang
        /// </summary>
        /// CreatedBy: NHHoang (27/8/2021)
        public Int32 TotalPage { get; set; }

        /// <summary>
        /// tổng số bản ghi
        /// </summary>
        /// CreatedBy: NHHoang (27/8/2021)
        public Int32 TotalRecord { get; set; }

        #endregion
    }
}
