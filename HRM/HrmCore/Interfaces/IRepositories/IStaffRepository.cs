using HrmCore.Entities;
using HrmCore.Entities.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Interfaces.IRepositories
{
    public interface IStaffRepository : IBaseRepository<Staff>
    {
        public IEnumerable<StaffColumn> GetEmployeeExportColumns();
        public int Import(List<Staff> staffs);
        List<FullStaff> GetByFilterPaging(string employeeFilter, Int32 pageSize, Int32 pageIndex, string departmentID,string positionID, int Status);
        InfoPage GetInfoPage(string employeeFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionID, int Status);
        bool CheckEmployeeCodeExists(string staffCode,string staffID);
        String GetLastStaffCode();

        public int InsertStaff(Staff staff);

    }
}
