using BnpTest.Core.Contracts.Repository;
using BnpTest.Core.Entities;

namespace BnpTest.Infra.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity

    {
        public Task CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
