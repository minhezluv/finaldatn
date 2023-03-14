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
    public class AccountService: BaseService<Account>, IAccountService
    {
        #region props and constructor
        IAccountRepository JobRepository;
        public AccountService(IAccountRepository mJobRepository) : base(mJobRepository)
        {
            JobRepository = mJobRepository;
        }
        #endregion
    }
}
