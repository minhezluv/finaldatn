using HrmCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Interfaces.IServices
{
    public interface ITimeKeepingService : IBaseService<TimeKeeping>
    {
        #region Additional method
        public ServiceResult GetStaffTimeAll();
        public ServiceResult GetStaffTimeByID(Guid StaffID);

        public ServiceResult GetByFilterPaging(string staffFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionName, int Year, int Month, int Day);
        public ServiceResult GetInfoPage(string staffFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionName, int Year, int Month, int Day);
        public ServiceResult GetByMonth(Guid guid);
        public ServiceResult GetIDbyStaffCode(string staffCode);
        public ServiceResult GetByWeekAll();

        public ServiceResult GetByYearAll(int year);

        public ServiceResult GetByWeekOption(int year, int week);
        #endregion
    }
}
