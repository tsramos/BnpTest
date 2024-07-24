using BnpTest.Core.Entities;

namespace BnpTest.Core.Contracts.Repository
{
    public interface IBaseRepository<in T> where T : Entity
    {
        Task CreateAsync(T entity);
    }
}
