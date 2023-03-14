using HrmCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Interfaces.IRepositories
{
    public interface IAccountRepository: IBaseRepository<Account>
    {
        bool checkAccount(string username, string password,string role);
        int insertAccount(Account account);
    }
}
