using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Services
{
    public class DepartmentService : BaseService<Department>, IDepartmentService
    {
		#region props and constructor
		IDepartmentRepository DepartmentRepository;
		public DepartmentService(IDepartmentRepository mDepartmentRepository) : base(mDepartmentRepository)
		{
			DepartmentRepository = mDepartmentRepository;
		}

        public ServiceResult GetByFilterPaging(string departmentFilter)
        {
            var serviceResult = new ServiceResult();

            var tmp = string.Empty;

            if (!(departmentFilter == null || departmentFilter == ""))
            {
                tmp = departmentFilter;
            }
            serviceResult.data = this.DepartmentRepository.GetByFilterPaging(tmp);
            return serviceResult;
        }
        #endregion
    }
}
