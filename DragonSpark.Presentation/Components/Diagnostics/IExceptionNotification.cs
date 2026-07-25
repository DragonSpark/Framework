using DragonSpark.Model.Selection;
using Radzen;

namespace DragonSpark.Presentation.Components.Diagnostics;

public interface IExceptionNotification : ISelect<Exception, NotificationMessage?>;