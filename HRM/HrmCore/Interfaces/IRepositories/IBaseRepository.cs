using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Interfaces.IRepositories
{
    public interface IBaseRepository<Entity>
    {
		public List<Entity> GetAll();
		public Entity GetById(Guid EntityId);
		public int Insert(Entity entity);
		public int Update(Guid EntityId, Entity entity);
		public int Delete(Guid EntityId);
		public bool checkDuplicateCode(string EntityCode);
		int MultiDeleteEntity(IEnumerable<Guid> listId);
		Entity GetEntityByProperty(Entity entity, string propName);
		Entity GetEntityByProperty(string propName, string propValue);
	}
}
