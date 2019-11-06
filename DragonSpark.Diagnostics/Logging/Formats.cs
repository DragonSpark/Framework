using JetBrains.Annotations;
using Serilog.Events;
using Serilog.Parsing;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Diagnostics.Logging
{
	sealed class Formats : ReferenceValueStore<MessageTemplate, IFormats>
	{
		public static Formats Default { get; } = new Formats();

		Formats() : base(x => x.Tokens
		                       .OfType<PropertyToken>()
		                       .ToDictionary(y => y.PropertyName, y => y.Format)
		                       .AsReadOnly()
		                       .To(I<Selection>.Default)) {}

		sealed class Selection : Select<string, string>, IFormats, IActivateUsing<IReadOnlyDictionary<string, string>>
		{
			readonly Array<string> _names;

			[UsedImplicitly]
			public Selection(IReadOnlyDictionary<string, string> source)
				: this(source.ToStore().Get, source.Keys.Result()) {}

			public Selection(Func<string, string> source, Array<string> names) : base(source) => _names = names;

			public Array<string> Get() => _names;
		}
	}
}