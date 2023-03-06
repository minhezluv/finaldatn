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
    public class TerminationContractController : BasesController<Terminationcontract>
    {
        private readonly ITerminationcontractService JobService;
        private readonly ITerminationcontractRepository jobRepository;
        public TerminationContractController(ITerminationcontractService mJobService, ITerminationcontractRepository mjobRepository) : base(mJobService)
        {
            JobService = mJobService;
            jobRepository = mjobRepository;
        }
    }
}
