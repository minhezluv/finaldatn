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
    public class CoefficientssalaryController : BasesController<Coefficientssalary>
    {
        private readonly ICoefficientssalaryService JobService;
        private readonly ICoefficientssalaryRepository jobRepository;
        public CoefficientssalaryController(ICoefficientssalaryService mJobService, ICoefficientssalaryRepository mjobRepository) : base(mJobService)
        {
            JobService = mJobService;
            jobRepository = mjobRepository;
        }
    }
}
