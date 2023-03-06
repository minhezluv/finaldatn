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
    public class PositionService : BaseService<Position>, IPositionService
    {
		#region props and constructor
		IPositionRepository JobRepository;
		public PositionService(IPositionRepository mJobRepository) : base(mJobRepository)
		{
			JobRepository = mJobRepository;
		}

        public ServiceResult GetByFilterPaging(string positionFilter)
        {
            var serviceResult = new ServiceResult();

            var tmp = string.Empty;

            if (!(positionFilter == null || positionFilter == ""))
            {
                tmp = positionFilter;
            }
                serviceResult.data = this.JobRepository.GetByFilterPaging(tmp);
            return serviceResult;
        }
        #endregion
    }
}
