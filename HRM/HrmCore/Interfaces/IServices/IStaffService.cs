using HrmCore.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HrmCore.Interfaces.IServices
{
    public interface IStaffService : IBaseService<Staff>
    {
        public MemoryStream Export(CancellationToken cancellationToken, string filterValue);
        public int Import(Staff staff);
        ServiceResult GetByFilterPaging(string employeeFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionID, int Status);
        ServiceResult GetInfoPage(string employeeFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionID, int Status);
        ServiceResult CheckEmployeeCodeExists(string staffCode, string staffID);
        ServiceResult GetLastStaffCode();
        ServiceResult GetAllStaffPerMonth(int year);
        ServiceResult InsertStaff(Staff staff);

        ServiceResult StaffNumberInfo(int year);
        
    }
}
