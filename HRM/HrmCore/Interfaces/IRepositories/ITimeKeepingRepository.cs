using HrmCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Interfaces.IRepositories
{
    public interface ITimeKeepingRepository : IBaseRepository<TimeKeeping>
    {
        #region Additional Method
        public List<StaffTime> GetStaffTimeAll();
        public StaffTime GetStaffTimeByID(Guid StaffID);

        public List<FullTimeKeeping> GetByFilterPaging(string staffFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionName,int Year,int Month,int Day);
        InfoPage GetInfoPage(string staffFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionName, int Year, int Month, int Day);
        public List<FullTimeKeeping> GetByMonth(Guid guid);
        public DateTime GetMonth(Guid guid);
        public Guid getIDbyStaffCode(string staffCode);

        public List<FullTimeKeeping> GetByMonthOption(int year, int month, string departmentID, string positionID, string staffID);
        #endregion
    }
}
