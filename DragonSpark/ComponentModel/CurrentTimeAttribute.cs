using DragonSpark.Sources.Parameterized;
using System;

namespace DragonSpark.ComponentModel
{
	public sealed class CurrentTimeAttribute : DefaultValueBase
	{
		readonly static Func<object, CurrentTimeValueProvider> Provider = CurrentTimeValueProvider.Default.Wrap();

		public CurrentTimeAttribute() : base( Provider ) {}
	}
}