﻿using System;
using RabbitMQ.Client;

namespace BuyMeIt.BuildingBlocks.EventBus.RabbitMQ
{
    public interface IRabbitMqPersistentConnection : IDisposable
    {
        bool IsConnected { get; }
        
        bool TryConnect();
        
        IModel CreateModel();
    }
}