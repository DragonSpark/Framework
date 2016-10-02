using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Aspects
{
	public static class Defaults
	{
		public static Func<Type, object> ArgumentSource { get; } = Activator.Default.GetService;
	}
}