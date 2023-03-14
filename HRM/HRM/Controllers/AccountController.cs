using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using HrmCore.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Controllers
{

    public class AccountController : BasesController<Account>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IAccountService accountService;
        public AccountController(IAccountRepository maccountRepository, IAccountService maccountService): base(maccountService)
        {
            accountRepository = maccountRepository;
            accountService = maccountService;
        }
        [EnableCors("AllowCROSPolicy")]
        [HttpPost("CheckAccount")]
        public IActionResult CheckAccount(Account account)
        {
            var serviceResult = new ServiceResult();
            try
            {
                serviceResult.data = accountRepository.checkAccount(account.Username, account.Password, account.RoleID);
                return StatusCode(200, serviceResult);
            }
            catch (System.Exception e)
            {

                var errObj = new
                {
                    devMsg = e.Message,

                };

                return StatusCode(500, errObj);
            }

        }






    }
}
