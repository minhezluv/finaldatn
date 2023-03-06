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
    public class DetailSalaryController : BasesController<DetailSalary>
    {
        private readonly IDetailSalaryService JobService;
        private readonly IDetailSalaryRepository jobRepository;
        public DetailSalaryController(IDetailSalaryService mJobService, IDetailSalaryRepository mjobRepository) : base(mJobService)
        {
            JobService = mJobService;
            jobRepository = mjobRepository;
        }
    }
}
