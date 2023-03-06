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
    public class JobService : BaseService<Job>, IJobService
    {
		#region props and constructor
		IJobRepository JobRepository;
		public JobService(IJobRepository mJobRepository) : base(mJobRepository)
		{
			JobRepository = mJobRepository;
		}
		#endregion
	}
}
