using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BookConsumerService.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();

        
    }
}
