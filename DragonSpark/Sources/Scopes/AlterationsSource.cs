using DragonSpark.Sources.Parameterized;
using System.Diagnostics.CodeAnalysis;

namespace DragonSpark.Sources.Scopes
{
	[SuppressMessage( "ReSharper", "PossibleInfiniteInheritance" )]
	public class AlterationsSource<T> : SuppliedAndExportedItems<IAlteration<T>>
	{
		public AlterationsSource( params IAlteration<T>[] configurators ) : base( configurators ) {}
	}
}