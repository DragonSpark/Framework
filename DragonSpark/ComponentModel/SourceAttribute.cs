using DragonSpark.Aspects;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using Defaults = DragonSpark.Activation.Location.Defaults;

namespace DragonSpark.ComponentModel
{
	public sealed class SourceAttribute : ServicesValueBase
	{
		readonly static Func<Type, object> Creator = Create;
		public SourceAttribute( [OfSourceType]Type sourceType = null ) : base( new ServicesValueProviderConverter( info => sourceType ?? SourceTypes.Default.Get( info.PropertyType ) ?? info.PropertyType ), Creator ) {}

		static object Create( Type type )
		{
			var serviceSource = Defaults.ServiceSource( type );
			var coerce = SourceCoercer.Default.Get( serviceSource );
			return coerce;
		}
	}
}