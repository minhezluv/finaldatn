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
    public class LaborContractController : BasesController<LaborContract>
    {
        private readonly ILaborContractService JobService;
        private readonly ILaborContractRepository jobRepository;
        public LaborContractController(ILaborContractService mJobService, ILaborContractRepository mjobRepository) : base(mJobService)
        {
            JobService = mJobService;
            jobRepository = mjobRepository;
        }
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("Filter")]
        public IActionResult GetByFilterPaging([FromQuery] string LCFilter, [FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string departmentID, [FromQuery] string positionID, [FromQuery] int Status, [FromQuery] int TypeLaborContractID)
        {
            try
            {
                var serviceResult = JobService.GetByFilterPaging(LCFilter, pageSize, pageIndex, departmentID, positionID,Status,TypeLaborContractID);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    // userMsg = MISA.Test.Core.Resources.ErrorMsg.ServerError_ErrorMsg,
                };

                return StatusCode(500, errObj);
            }
        }
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("InfoPage")]
        public IActionResult GetInfoPage([FromQuery] string LCFilter, [FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string departmentID, [FromQuery] string positionID)
        {
            try
            {
                var serviceResult = JobService.GetInfoPage(LCFilter, pageSize, pageIndex, departmentID, positionID);


                return StatusCode(200, serviceResult.data);


            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    // userMsg = MISA.Test.Core.Resources.ErrorMsg.ServerError_ErrorMsg,
                };

                return StatusCode(500, errObj);
            }

        }
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("LastLCCode")]
        public IActionResult GetLastLCCode()
        {
            try
            {
                var serviceResult = this.JobService.GetLastLCCode();

                return StatusCode(200, serviceResult.data);
            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "không thể kết nối",
                };

                return StatusCode(500, errObj);
            }
        }
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("CheckStaffCodeExists/{lcCode}/{lcID}")]
        public IActionResult CheckLCCodeExists(string lcCode, string lcID)
        {
            try
            {
                var serviceResult = this.JobService.CheckLCCodeExists(lcCode, lcID);
                if (!serviceResult.success)
                {
                    var errObj = new
                    {
                        devMsg = serviceResult.devMsg,
                        userMsg = serviceResult.userMsg,
                    };

                    return BadRequest(errObj);
                }

                return StatusCode(200, serviceResult.data);
            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "không thể kết nối server",
                };

                return StatusCode(500, errObj);
            }
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpPost("InsertLC")]

        public IActionResult InsertStaff(LaborContract LC)
        {
            try
            {
                var serviceResult = this.JobService.InsertLC(LC);
                
                return StatusCode(200, serviceResult);
            }
            catch (Exception e)
            {
                var errObj = new
                {
                    devMsg = e.Message,
                    userMsg = "không thể kết nối server",
                };

                return StatusCode(500, errObj);
            }
        }

    }
}
