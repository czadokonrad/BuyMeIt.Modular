using BuyMeIt.BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Infrastructure
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        public UnitOfWork(
            DbContext context,
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            this._context = context;
            this._domainEventsDispatcher = domainEventsDispatcher;
        }

        public async Task<int> CommitAsync(
            CancellationToken cancellationToken = default,
            Guid? internalCommandId = null)
        {
            await _domainEventsDispatcher.DispatchEventsAsync();

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
