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
    public interface ILaborContractService : IBaseService<LaborContract>
    {
        public MemoryStream Export(CancellationToken cancellationToken, string filterValue);
        public int Import(Staff staff);
        ServiceResult GetByFilterPaging(string lcFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionID,int Status, int TypeLaborContract);
        ServiceResult GetInfoPage(string lcFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionID);
        ServiceResult CheckLCCodeExists(string staffCode, string staffID);
        ServiceResult GetLastLCCode();
        bool checkActiveStaff(Guid staffID);
        ServiceResult InsertLC(LaborContract LC);
    }
}
