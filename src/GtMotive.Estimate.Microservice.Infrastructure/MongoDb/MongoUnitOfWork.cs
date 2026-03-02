using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    /// <summary>
    /// MongoDB implementation of unit of work. With MongoDB driver, each operation is committed immediately;
    /// Save is provided for compatibility with the use case pattern.
    /// </summary>
    public class MongoUnitOfWork : IUnitOfWork
    {
        /// <inheritdoc/>
        public Task<int> Save()
        {
            return Task.FromResult(1);
        }
    }
}
