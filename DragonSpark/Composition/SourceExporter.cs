using DragonSpark.Sources.Delegates;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;
using System.Composition.Hosting.Core;

namespace DragonSpark.Composition
{
	public sealed class SourceExporter : SourceDelegateExporterBase
	{
		readonly static Func<ActivationParameter, object> DefaultResult = Factory.Implementation.Get;

		public static SourceExporter Default { get; } = new SourceExporter();
		SourceExporter() : base( DefaultResult, Delegates<CompositionContract>.Self ) {}

		sealed class Factory : ParameterizedSourceBase<ActivationParameter, object>
		{
			public static Factory Implementation { get; } = new Factory();
			Factory() {}

			public override object Get( ActivationParameter parameter ) => SourceFactory.Defaults.Get( parameter.Services ).Get( parameter.SourceType );
		}
	}
}