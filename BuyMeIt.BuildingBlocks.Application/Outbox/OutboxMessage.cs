using System;

namespace BuyMeIt.BuildingBlocks.Application.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }

        public DateTimeOffset OccurredOn { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public DateTimeOffset? ProcessedDate { get; set; }

        public OutboxMessage(Guid id, DateTimeOffset occurredOn, string type, string data)
        {
            this.Id = id;
            this.OccurredOn = occurredOn;
            this.Type = type;
            this.Data = data;
        }
    }
}
