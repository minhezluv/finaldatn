using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using HrmCore.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Controllers
{
    public class DepartmentController : BasesController<Department>
    {
        private readonly IDepartmentService DepartmentService;
        private readonly IDepartmentRepository departmentRepository;
        public DepartmentController(IDepartmentService mDepartmentService, IDepartmentRepository mdepartmentRepository) : base(mDepartmentService)
        {
            DepartmentService = mDepartmentService;
            departmentRepository = mdepartmentRepository;
        }

        [EnableCors("AllowCROSPolicy")]
        [HttpGet("Filter")]
        public IActionResult GetByFilterPaging([FromQuery] string departmentFilter)
        {
            try
            {
                var serviceResult = DepartmentService.GetByFilterPaging(departmentFilter);


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
