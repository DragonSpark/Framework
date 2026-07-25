using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Workers;
using DragonSpark.Contracts.Messaging;
using System;
using System.Linq;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class SelectUnsentNotifications
	: StartWhereSelection<DateTimeOffset, ProcessNotification, DurableMessageProperties>
{
	public static SelectUnsentNotifications Default { get; } = new();

	SelectUnsentNotifications()
		: base((p, x) => x.AvailableAt <= p,
		       (d, p, q) => q.OrderBy(x => x.AvailableAt)
		                     .Take(100)
		                     .Select(x => new DurableMessageProperties(x.Id, x.Destination,
		                                                               x.AvailableAt.HasValue
			                                                               ? p - x.AvailableAt
			                                                               : null, 
		                                                               x.Lifetime))) {}
}