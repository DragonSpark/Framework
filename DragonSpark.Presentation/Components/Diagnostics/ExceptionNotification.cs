using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using Radzen;
using System;

namespace DragonSpark.Presentation.Components.Diagnostics;

sealed class ExceptionNotification : FixedResult<Exception, NotificationMessage>, IExceptionNotification
{
	[UsedImplicitly]
	public static ExceptionNotification Default { get; } = new();

	ExceptionNotification() : base(DefaultNotificationMessage.Default) {}
}