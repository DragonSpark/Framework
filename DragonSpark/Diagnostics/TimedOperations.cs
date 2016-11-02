using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using System;
using System.Reflection;

namespace DragonSpark.Diagnostics
{
	public sealed class TimedOperations : DelegatedParameterizedSource<string, Func<MethodBase, IDisposable>>
	{
		public static TimedOperations Default { get; } = new TimedOperations();
		TimedOperations() : base( parameter => Configuration.Implementation.Get( parameter ).ToEqualityCache().ToDelegate() ) {}

		public sealed class Configuration : ParameterizedScope<string, IParameterizedSource<MethodBase, IDisposable>>
		{
			public static Configuration Implementation { get; } = new Configuration();
			Configuration() : base( parameter => new TimedOperationFactory( parameter ) ) {}
		}
	}
}