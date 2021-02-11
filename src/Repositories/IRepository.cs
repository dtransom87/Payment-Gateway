using System;
using System.Threading.Tasks;
using payment_gateway.Models;

namespace payment_gateway.Repositories
{
    public interface IRepository<EntityBase>
    {
        Task<EntityBase> Create(EntityBase entityBase);
        
        Task<EntityBase> Get(string id);

        Task<EntityBase> Update(EntityBase entityBase);

        Task Delete(Guid id);
    }
}