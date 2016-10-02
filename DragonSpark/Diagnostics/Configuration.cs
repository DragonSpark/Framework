using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Reflection;

namespace DragonSpark.Diagnostics
{
	public static class Configuration
	{
		public static IParameterizedScope<string, Func<MethodBase, IDisposable>> TimedOperationFactory { get; } = new ParameterizedScope<string, Func<MethodBase, IDisposable>>( TimedOperationFactorySource.Default.ToSourceDelegate().GlobalCache() );
	}
}