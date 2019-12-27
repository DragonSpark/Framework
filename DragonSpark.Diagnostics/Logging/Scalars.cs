using DragonSpark.Model.Selection;
using Serilog.Events;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Diagnostics.Logging
{
	sealed class Scalars : IScalars
	{
		public static Scalars Default { get; } = new Scalars();

		Scalars() : this(Formats.Default) {}

		readonly ISelect<MessageTemplate, IFormats> _format;

		public Scalars(ISelect<MessageTemplate, IFormats> format) => _format = format;

		public IScalar Get(LogEvent parameter)
			=> new Scalar(Enumerate(parameter).ToDictionary(x => x.Key, x => x).AsReadOnly());

		IEnumerable<ScalarProperty> Enumerate(LogEvent parameter)
		{
			var formats    = _format.Get(parameter.MessageTemplate);
			var properties = parameter.Properties;
			foreach (var name in formats.Get().Open())
			{
				switch (properties[name])
				{
					case ScalarValue scalar:
						yield return new ScalarProperty(name, formats, scalar.Value);
						break;
				}
			}
		}
	}
}