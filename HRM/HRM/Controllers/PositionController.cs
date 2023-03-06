using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using Microsoft.AspNetCore.Cors;
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
    public class PositionController : BasesController<Position>
    {
        private readonly IPositionService JobService;
        private readonly IPositionRepository jobRepository;
        public PositionController(IPositionService mJobService, IPositionRepository mjobRepository) : base(mJobService)
        {
            JobService = mJobService;
            jobRepository = mjobRepository;
        }


        [EnableCors("AllowCROSPolicy")]
        [HttpGet("Filter")]
        public IActionResult GetByFilterPaging([FromQuery] string positionFilter)
        {
            try
            {
                var serviceResult = JobService.GetByFilterPaging(positionFilter);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
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
