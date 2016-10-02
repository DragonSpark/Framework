using DragonSpark.Aspects;
using DragonSpark.Sources;
using System;
using Defaults = DragonSpark.Activation.Location.Defaults;

namespace DragonSpark.ComponentModel
{
	public sealed class SourceAttribute : ServicesValueBase
	{
		readonly static Func<Type, object> Creator = Create;
		public SourceAttribute( [OfSourceType]Type sourceType = null ) : base( new ServicesValueProviderConverter( info => sourceType ?? info.PropertyType ), Creator ) {}

		static object Create( Type type ) => Defaults.ServiceSource( type ).Value();
	}
}