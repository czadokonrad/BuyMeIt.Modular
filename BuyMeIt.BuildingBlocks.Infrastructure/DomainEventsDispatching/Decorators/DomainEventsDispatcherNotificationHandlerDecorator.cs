using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Infrastructure.DomainEventsDispatching.Decorators
{
    public sealed class DomainEventsDispatcherNotificationHandlerDecorator<T> :
        INotificationHandler<T> where T : INotification
    {
        private readonly IDomainEventsDispatcher _dispatcher;
        private readonly INotificationHandler<T> _decorated;

        public DomainEventsDispatcherNotificationHandlerDecorator(
            IDomainEventsDispatcher dispatcher,
            INotificationHandler<T> decorated)
        {
            _dispatcher = dispatcher;
            _decorated = decorated;
        }

        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            await _decorated.Handle(notification, cancellationToken);

            await _dispatcher.DispatchEventsAsync();
        }
    }
}
