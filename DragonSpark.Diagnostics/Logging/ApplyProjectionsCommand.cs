using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Objects;
using Serilog.Events;
using System;

namespace DragonSpark.Diagnostics.Logging
{
	sealed class ApplyProjectionsCommand : ICommand<LogEvent>
	{
		readonly IProjectors             _projectors;
		readonly Func<LogEvent, IScalar> _scalars;

		public ApplyProjectionsCommand(IProjectors projectors) : this(Implementations.Scalars, projectors) {}

		public ApplyProjectionsCommand(Func<LogEvent, IScalar> scalars, IProjectors projectors)
		{
			_scalars    = scalars;
			_projectors = projectors;
		}

		public void Execute(LogEvent parameter)
		{
			foreach (var scalar in _scalars(parameter))
			{
				var instance  = scalar.Value.Instance;
				var projector = _projectors.Get(instance.GetType());
				if (projector != null)
				{
					var format     = scalar.Value.Get();
					var projection = projector(format)(instance);
					var value      = new ScalarValue(projection);
					parameter.AddOrUpdateProperty(new LogEventProperty(scalar.Key, value));
				}
			}
		}
	}
}