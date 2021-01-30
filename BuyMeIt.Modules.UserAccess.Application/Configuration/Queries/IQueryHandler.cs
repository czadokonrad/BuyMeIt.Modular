using BuyMeIt.Modules.UserAccess.Application.Contracts;
using MediatR;

namespace BuyMeIt.Modules.UserAccess.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}