using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicSalaryController : BasesController<BasicSalary>
    {
        private readonly IBasicSalaryService JobService;
        private readonly IBasicSalaryRepository jobRepository;
        public BasicSalaryController(IBasicSalaryService mJobService, IBasicSalaryRepository mjobRepository) : base(mJobService)
        {
            JobService = mJobService;
            jobRepository = mjobRepository;
        }
    }
}
