using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DragonSpark.Application.Presentation.Commands
{
	public interface ICommandMonitor : INotifyPropertyChanged
	{
		event EventHandler Started, Completed;

		void Monitor( CommandMonitorContext context );

		IEnumerable<IMonitoredCommand> Commands { get; }
	}
}