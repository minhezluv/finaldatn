using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Controllers
{
    public class JobController : BasesController<Job>
    {
        private readonly IJobService JobService;
        private readonly IJobRepository jobRepository;
        public JobController(IJobService mJobService, IJobRepository mjobRepository) : base(mJobService)
        {
            JobService = mJobService;
            jobRepository = mjobRepository;
        }
    }
}
