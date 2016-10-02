using DragonSpark.Sources.Parameterized;
using System;
using System.Reflection;

namespace DragonSpark.Diagnostics
{
	public sealed class TimedOperationFactorySource : ParameterizedSourceBase<string, Func<MethodBase, IDisposable>>
	{
		public static IParameterizedSource<string, Func<MethodBase, IDisposable>> Default { get; } = new TimedOperationFactorySource().ToEqualityCache();
		TimedOperationFactorySource() {}

		public override Func<MethodBase, IDisposable> Get( string parameter ) => new TimedOperationFactory( parameter ).ToSourceDelegate();
	}
}