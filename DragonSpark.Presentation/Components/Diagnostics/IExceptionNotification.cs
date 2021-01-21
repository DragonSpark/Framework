using DragonSpark.Model.Selection;
using Radzen;
using System;

namespace DragonSpark.Presentation.Components.Diagnostics
{
	public interface IExceptionNotification : ISelect<Exception, NotificationMessage> {}
}