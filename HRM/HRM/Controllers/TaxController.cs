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
    public class TaxController : BasesController<Tax>
    {
        private readonly ITaxService TaxService;
        private readonly ITaxRepository TaxRepository;
        public TaxController(ITaxService mJobService, ITaxRepository mjobRepository) : base(mJobService)
        {
            TaxService = mJobService;
            TaxRepository = mjobRepository;
        }
    }
}
