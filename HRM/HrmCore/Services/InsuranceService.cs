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
    public class InsuranceService : BaseService<Insurance>, IInsuranceService
    {
		#region props and constructor
		IInsuranceRepository JobRepository;
		public InsuranceService(IInsuranceRepository mJobRepository) : base(mJobRepository)
		{
			JobRepository = mJobRepository;
		}
		#endregion
	}
}
