using HrmCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Interfaces.IServices
{
    public interface IDepartmentService : IBaseService<Department>
    {
        public ServiceResult GetByFilterPaging(string departmentFilter);
    }
}
