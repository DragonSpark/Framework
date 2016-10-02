using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Diagnostics
{
	public sealed class TimedOperationFactory : DragonSpark.Diagnostics.TimedOperationFactory
	{
		public const string ExecutedTestMethodMethod = "Executed Test Method '{@Method}'";

		public static IParameterizedSource<MethodBase, IDisposable> Default { get; } = new TimedOperationFactory().Apply( AssignedSpecification<Action<string>>.Default.Fixed( Output.Default.Get ) );
		TimedOperationFactory() : base( ExecutedTestMethodMethod ) {}
	}
}