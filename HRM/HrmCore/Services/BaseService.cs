using HrmCore.Entities;
using HrmCore.Interfaces.IRepositories;
using HrmCore.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Services
{
    public class BaseService<Entity> : IBaseService<Entity>
    {
        #region props and constructor
        protected IBaseRepository<Entity> baseRepository;
        protected ServiceResult result;
        protected string tableName;
        public BaseService(IBaseRepository<Entity> mBaseRepository) //tiêm
        {
            baseRepository = mBaseRepository;
            result = new ServiceResult();
            tableName = typeof(Entity).Name;
        }
		#endregion
		#region GET INSERT UPDATE DELETE
		/// <summary>
		/// process business CRUD then call IEntityRepository operating with DB
		/// </summary>
		public ServiceResult GetAll()
		{
			
			result.setField(true, "", baseRepository.GetAll());
			return result;
		}

		public ServiceResult GetById(Guid EntityId)
		{
			result.setField(true, "", baseRepository.GetById(EntityId));
			return result;
		}
		public Entity GetEntityByProperty(string propName, string propValue)
		{
			return baseRepository.GetEntityByProperty(propName, propValue);
		}
		public ServiceResult Insert(Entity entity)
		{
			// 0. Assign new id to entity
			SetId(entity, Guid.NewGuid());
			// 1. Validate data: 
			string errorInfo = null; // ValidateData(entity);
			if (errorInfo != null)
			{
				result.setField(false, errorInfo, null);
				return result;
			}
			// 2. Insert new record to database
			if (baseRepository.Insert(entity) > 0)
			{
				result.setField(true, "", entity);
				return result;
			}
			throw new NotImplementedException();
		}

		public ServiceResult Update(Guid EntityId, Entity entity)
		{
			// 0. Assign id to entity
			SetId(entity, EntityId);
			// 1. Validate data: 
			string errorInfo = null; //ValidateData(entity);
			if (errorInfo != null)
			{
				result.setField(false, errorInfo, null);
				return result;
			}
			// 2. Update a record to database
			if (baseRepository.Update(EntityId, entity) > 0)
			{
				result.setField(true, "", entity);
				return result;
			}
			throw new NotImplementedException();
		}

		public ServiceResult Delete(Guid EntityId)
		{
			//1. Check if there is EntityId in database
			var entity = baseRepository.GetById(EntityId);
			if (entity == null)
			{
				result.setField(false, "EntityId not found", null);
				return result;
			}

			//2. Delete record in database
			baseRepository.Delete(EntityId);
			result.setField(true, "", entity);
			return result;
			throw new NotImplementedException();
		}
		#endregion

		#region Additional methods
		public void SetId(Entity entity, Guid guid)
		{
			entity.GetType().GetProperty("guid").SetValue(entity, guid, null);
		}

        public ServiceResult MultipleDelete(IEnumerable<Guid> listId)
        {
			//var _serviceResult = new ServiceResult();
			var rowAffect = baseRepository.MultiDeleteEntity(listId);

			if (rowAffect > 0)
			{
				//_serviceResult.data = rowAffect;
				//_serviceResult.success = Int32(200);
				//_serviceResult.Message = Properties.Resources.Msg_SuccessDelete;
				result.setField(true, "delete sucess", rowAffect);
			}
			else
			{
				//_serviceResult.Code = MISACode.InvalidData;
				//_serviceResult.Message = Properties.Resources.Msg_FailedDelete;
				result.setField(false, "delete false", rowAffect);
			}

			return result;
		}
		#endregion

		#region bonus methods
		protected bool ValidateId(string id)
		{
			Guid tmp;

			return Guid.TryParse(id.ToString(), out tmp);
		}
		#endregion
	}
}
