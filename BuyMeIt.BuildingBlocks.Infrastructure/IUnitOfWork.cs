﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace BuyMeIt.BuildingBlocks.Infrastructure
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(
            CancellationToken cancellationToken = default,
            Guid? internalCommandId = null);
    }
}
