using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System;
using System.Reflection;

namespace DragonSpark.Diagnostics
{
	public static class Configuration
	{
		public static IParameterizedScope<string, Func<MethodBase, IDisposable>> TimedOperationFactory { get; } = new ParameterizedScope<string, Func<MethodBase, IDisposable>>( TimedOperationFactorySource.Default.ToDelegate().GlobalCache() );
	}
}