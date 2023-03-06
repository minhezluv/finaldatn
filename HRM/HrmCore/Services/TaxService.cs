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
    public class TaxService : BaseService<Tax>, ITaxService
    {
		#region props and constructor
		ITaxRepository TaxRepository;
		public TaxService(ITaxRepository mTaxRepository) : base(mTaxRepository)
		{
			TaxRepository = mTaxRepository;
		}
		#endregion
	}
}
