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
    public class TerminationcontractService : BaseService<Terminationcontract>, ITerminationcontractService
    {
		#region props and constructor
		ITerminationcontractRepository JobRepository;
		public TerminationcontractService(ITerminationcontractRepository mJobRepository) : base(mJobRepository)
		{
			JobRepository = mJobRepository;
		}
		#endregion
	}
}
