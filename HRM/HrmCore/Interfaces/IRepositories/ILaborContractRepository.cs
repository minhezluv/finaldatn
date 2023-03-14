using HrmCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Interfaces.IRepositories
{
    public interface ILaborContractRepository : IBaseRepository<LaborContract>
    {
        //public IEnumerable<StaffColumn> GetEmployeeExportColumns();
        public int Import(List<LaborContract> laborContracts);
        List<FullLaborContract> GetByFilterPaging(string LCFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionID,int Status, int TypeLaborContractID);
        InfoPage GetInfoPage(string LCFilter, Int32 pageSize, Int32 pageIndex, string departmentID, string positionID, int Status, int TypeLaborContractID);
        bool CheckLCCodeExists(string lcCode, string lcID);
        String GetLastLCCode();
        int activeStaff(Guid staffID);
        public int InsertLC(LaborContract LC);
    }
}
