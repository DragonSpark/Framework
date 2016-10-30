using DragonSpark.Aspects;
using System;
using DragonSpark.Sources;
using Defaults = DragonSpark.Activation.Location.Defaults;

namespace DragonSpark.ComponentModel
{
	public sealed class SourceAttribute : ServicesValueBase
	{
		readonly static Func<Type, object> Creator = Create;
		public SourceAttribute( [OfSourceType]Type sourceType = null ) : base( new ServicesValueProviderConverter( info => sourceType ?? info.PropertyType ), Creator ) {}

		static object Create( Type type ) => SourceCoercer.Default.Coerce( Defaults.ServiceSource( type ) );
	}
}