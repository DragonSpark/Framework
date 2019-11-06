using System;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Text.Formatting
{
	public sealed class FormatterRegistration : SystemStore<IConditional<object, IFormattable>>
	{
		public static FormatterRegistration Default { get; } = new FormatterRegistration();

		FormatterRegistration() : base(DefaultSystemFormatter.Default.Self) {}
	}
}