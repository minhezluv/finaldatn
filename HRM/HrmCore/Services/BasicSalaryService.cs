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
    public class BasicSalaryService : BaseService<BasicSalary>, IBasicSalaryService
    {
		#region props and constructor
		IBasicSalaryRepository BasicSalaryRepository;
		public BasicSalaryService(IBasicSalaryRepository mBasicSalaryRepository) : base(mBasicSalaryRepository)
		{
			BasicSalaryRepository = mBasicSalaryRepository;
		}
		#endregion
	}
}
