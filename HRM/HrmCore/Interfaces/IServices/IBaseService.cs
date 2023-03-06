using HrmCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrmCore.Interfaces.IServices
{
    public interface IBaseService<Entity>
    {
        Entity GetEntityByProperty(string propName, string propValue);
        public ServiceResult GetAll();
        public ServiceResult GetById(Guid EntityId);
        public ServiceResult Insert(Entity entity);
        public ServiceResult Update(Guid EntityId, Entity entity);
        public ServiceResult Delete(Guid EntityId);
        ServiceResult MultipleDelete(IEnumerable<Guid> listId);
    }
}
