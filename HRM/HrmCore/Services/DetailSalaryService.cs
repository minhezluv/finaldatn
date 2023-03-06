﻿using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Services
{
    public class DetailSalaryService: BaseService<DetailSalary>, IDetailSalaryService
    {
		#region props and constructor
		IDetailSalaryRepository JobRepository;
		public DetailSalaryService(IDetailSalaryRepository mJobRepository) : base(mJobRepository)
		{
			JobRepository = mJobRepository;
		}
		#endregion
	}
}
