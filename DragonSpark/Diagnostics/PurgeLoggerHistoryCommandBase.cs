using DragonSpark.Commands;
using DragonSpark.Extensions;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Diagnostics
{
	public abstract class PurgeLoggerHistoryCommandBase<T> : CommandBase<Action<T>>
	{
		readonly Func<ILoggerHistory> historySource;
		readonly Func<IEnumerable<LogEvent>, ImmutableArray<T>> factory;

		protected PurgeLoggerHistoryCommandBase( Func<ILoggerHistory> historySource, Func<IEnumerable<LogEvent>, ImmutableArray<T>> factory )
		{
			this.historySource = historySource;
			this.factory = factory;
		}

		public override void Execute( Action<T> parameter )
		{
			var history = historySource();
			factory( history.Events ).Each( parameter );
			history.Clear();
		}
	}
}