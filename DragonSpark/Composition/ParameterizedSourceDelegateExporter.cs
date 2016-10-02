using DragonSpark.Sources.Delegates;
using DragonSpark.Sources.Parameterized;
using System;
using System.Composition.Hosting.Core;

namespace DragonSpark.Composition
{
	public sealed class ParameterizedSourceDelegateExporter : SourceDelegateExporterBase
	{
		readonly static Func<CompositionContract, CompositionContract> Resolver = SourceDelegateContractResolver.Parameterized.ToSourceDelegate();
		readonly static Func<ActivationParameter, object> DelegateSource = Factory.DefaultNested.Get;

		public static ParameterizedSourceDelegateExporter Default { get; } = new ParameterizedSourceDelegateExporter();
		ParameterizedSourceDelegateExporter() : base( DelegateSource, Resolver ) {}

		sealed class Factory : ParameterizedSourceBase<ActivationParameter, object>
		{
			public static Factory DefaultNested { get; } = new Factory();
			Factory() {}

			public override object Get( ActivationParameter parameter ) => 
				ParameterizedSourceDelegates.Sources.Get( parameter.Services ).Get( parameter.SourceType );
		}
	}
}