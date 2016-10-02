using DragonSpark.Sources.Delegates;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Generic;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Composition
{
	sealed class ExportTypeExpander : ParameterizedSourceBase<Type, IEnumerable<Type>>
	{
		readonly Func<Type, Type> source;
		public static ExportTypeExpander Default { get; } = new ExportTypeExpander();

		ExportTypeExpander() : this( SourceTypes.Default.Get ) {}

		ExportTypeExpander( Func<Type, Type> source )
		{
			this.source = source;
		}

		public override IEnumerable<Type> Get( Type parameter )
		{
			yield return parameter;
			var sourceType = source( parameter );
			if ( sourceType != null )
			{
				var provider = Activator.Default;
				yield return ResultTypes.Default.Get( sourceType );
				yield return ParameterizedSourceDelegates.Sources.Get( provider ).Get( sourceType )?.GetType() ?? SourceDelegates.Sources.Get( provider ).Get( sourceType )?.GetType();
			}
		}
	}
}