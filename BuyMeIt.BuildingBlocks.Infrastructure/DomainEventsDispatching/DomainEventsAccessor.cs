using BuyMeIt.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;

namespace BuyMeIt.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
#nullable enable
    public sealed class DomainEventsAccessor : IDomainEventsAccessor
    {
        private readonly DbContext _context;
        private IReadOnlyCollection<EntityEntry<Entity>> _domainEntities;

        public DomainEventsAccessor(DbContext context)
        {
            _context = context;
            _domainEntities = new List<EntityEntry<Entity>>();
        }

        private void FindDomainEntitiesWithDomainEvents()
        {
            _domainEntities = _context.ChangeTracker.Entries<Entity>()
                .Where(e => e.Entity.DomainEvents?.Count > 0)
                .ToList();
        }

        public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
        {
            FindDomainEntitiesWithDomainEvents();

            return _domainEntities.SelectMany(e => e.Entity.DomainEvents).ToList();
        }

        public void ClearAllDomainEvents()
        {
            FindDomainEntitiesWithDomainEvents();

            foreach(var de in _domainEntities)
            {
                de.Entity.ClearDomainEvents();
            }
        }
    }
#nullable restore
}
