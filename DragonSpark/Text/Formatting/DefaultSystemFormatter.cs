using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Text.Formatting
{
	sealed class DefaultSystemFormatter : Conditional<object, IFormattable>, IFormatter
	{
		public static DefaultSystemFormatter Default { get; } = new DefaultSystemFormatter();

		DefaultSystemFormatter() : base(Always<object>.Default,
		                                Start.A.Selection.Of.Any.By.StoredActivation<DefaultFormatter>()) {}
	}
}