using Serilog.Core;
using Serilog.Events;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Diagnostics.Logging
{
	sealed class ProjectionLogEvents : IAlteration<LogEvent>
	{
		public static ProjectionLogEvents Default { get; } = new ProjectionLogEvents();

		ProjectionLogEvents() : this(Implementations.Scalars, PropertyFactories.Default.Get) {}

		readonly Func<LogEvent, ILogEventPropertyFactory> _factories;

		readonly Func<LogEvent, IScalar> _scalars;

		public ProjectionLogEvents(Func<LogEvent, IScalar> scalars, Func<LogEvent, ILogEventPropertyFactory> factories)
		{
			_scalars   = scalars;
			_factories = factories;
		}

		public LogEvent Get(LogEvent parameter)
		{
			var properties = parameter.Properties;
			var structures = Structures(parameter, properties).ToDictionary(x => x.Name);
			var result = structures.Count > 0
				             ? new LogEvent(parameter.Timestamp, parameter.Level, parameter.Exception, parameter.MessageTemplate,
				                            Properties(properties, structures))
				             : parameter;
			return result;
		}

		static IEnumerable<LogEventProperty> Properties(IReadOnlyDictionary<string, LogEventPropertyValue> source,
		                                                IReadOnlyDictionary<string, LogEventProperty> structures)
		{
			var keys   = source.Keys.ToArray();
			var length = keys.Length;
			var result = new LogEventProperty[length];
			for (var i = 0; i < length; i++)
			{
				var key = keys[i];
				result[i] = structures.ContainsKey(key) ? structures[key] : new LogEventProperty(key, source[key]);
			}

			return result;
		}

		IEnumerable<LogEventProperty> Structures(LogEvent parameter,
		                                         IReadOnlyDictionary<string, LogEventPropertyValue> dictionary)
		{
			var factory = _factories(parameter);
			foreach (var scalar in _scalars(parameter))
			{
				if (dictionary[scalar.Key] is ScalarValue value && value.Value is Projection projection)
				{
					yield return new LogEventProperty(scalar.Key,
					                                  new StructureValue(Properties(projection, factory),
					                                                     projection.InstanceType.Name));
				}
			}
		}

		static IEnumerable<LogEventProperty> Properties(Projection projection, ILogEventPropertyFactory factory)
		{
			foreach (var name in projection)
			{
				yield return factory.CreateProperty(name.Key, name.Value, true);
			}
		}
	}
}