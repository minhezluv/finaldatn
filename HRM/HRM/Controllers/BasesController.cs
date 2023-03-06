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
	/**
 * Một API được phân biệt qua: 
 *		route (api/Bases)
 *		method (HttpGet,...)
 *		endpoint (nếu không có {} thì không yêu cầu truyền tham số)
 */

	/**
	 * THIET KE CHUAN RESTFUL
	 */

	/**
	 * http response:
	 * 200 OK
	 * 201 CREATED
	 * 400 BAD REQUEST: INPUT INVALID FROM USER
	 * 401 UNAUTHORIZED
	 * 403 FORBIDDEN, even AUTHORIZED
	 * 404 NOT FOUND
	 * 500 INTERNAL SERVER ERROR (code logic error)
	 */

	/**
	 * TECHNIQUE: filter, sort, paging and offset
	 */

	/**
	 * Format return data: JSON
	 */

	[Route("api/v1/[controller]")]
	[ApiController]
	public class BasesController<Entity> : ControllerBase
	{
		protected readonly IBaseService<Entity> baseService;
		//protected readonly IBaseRepository<Entity> baseRepository;
		protected ServiceResult response;
       

        public BasesController(IBaseService<Entity> mBaseService)
		{
			baseService = mBaseService;
			//baseRepository = mBaseRepository;
			response = new ServiceResult();
		}


		#region GET POST PUT DELETE PROTOCOL
		[EnableCors("AllowCROSPolicy")]
		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				response = new ServiceResult();
		
				baseService.GetAll();
				response = baseService.GetAll();
				return StatusCode(200, response.data);
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			};
		}

		[HttpGet("{EntityId}")]
		public IActionResult GetById(Guid EntityId)
		{
			try
			{
				response = baseService.GetById(EntityId);
				return StatusCode(200, response);
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			};
		}

		/// <summary>
		/// Insert new entity to database
		/// </summary>
		/// <approach>
		/// 1. Use store procedure to make code shorter
		/// (Ex: Proc_Insert is a procedure stored in database)
		/// 2. Build sqlCmd by for loop as below (see code of infrastructure package)
		/// <approach>
		[EnableCors("AllowCROSPolicy")]
		[HttpPost]
		public IActionResult Post([FromBody] Entity entity)
		{
			try
			{
				response = baseService.Insert(entity);
				if (response.success) return StatusCode(201, response.data);
				else return StatusCode(400, response);
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			}
		}

		[EnableCors("AllowCROSPolicy")]
		[HttpPut("{EntityId}")]
		public IActionResult Put(Guid EntityId, [FromBody] Entity entity)
		{
			try
			{
				response = baseService.Update(EntityId, entity);
				if (response.success) return StatusCode(200, response.data);
				else return StatusCode(400, response);
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			}
		}

		[EnableCors("AllowCROSPolicy")]
		[HttpDelete("{EntityId}")]
		public IActionResult Delete(Guid EntityId)
		{
			try
			{
				response = baseService.Delete(EntityId);
				if (response.success) return StatusCode(200, response.data);
				else return StatusCode(400, response);
			}
			catch (Exception ex)
			{
				return HandleException(ex);
			}
		}

		[EnableCors("AllowCROSPolicy")]
		[HttpGet("Property")]
		public IActionResult GetByProperty([FromQuery] string propName, [FromQuery] string propValue)
		{
			var entity = baseService.GetEntityByProperty(propName, propValue);

			if (entity != null)
			{
				return Ok(entity);
			}
			else
			{
				return NoContent();
			}
		}

		[EnableCors("AllowCROSPolicy")]
		[HttpDelete]
		public IActionResult MultipleDelete([FromBody] IEnumerable<Guid> listId)
		{
			var serviceResult = baseService.MultipleDelete(listId);

			if (serviceResult.success == true)
			{
				return StatusCode(200,serviceResult);
			}
			else
			{
				return NoContent();
			}
		}
		#endregion


		#region Additional methods
		protected IActionResult HandleException(Exception ex)
		{
			response.setField(false, ex.Message, null);
			return StatusCode(500, response);
		}
		#endregion
	}
}
