using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Contracts.Messaging;
using System;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class EvaluateUnsentNotifications : EvaluateToLease<DateTimeOffset, DurableMessageProperties>
{
	public EvaluateUnsentNotifications(IScopes scopes) : base(scopes, SelectUnsentNotifications.Default) {}
}