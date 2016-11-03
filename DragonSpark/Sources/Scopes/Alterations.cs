using DragonSpark.Sources.Parameterized;
using System.Diagnostics.CodeAnalysis;

namespace DragonSpark.Sources.Scopes
{
	[SuppressMessage( "ReSharper", "PossibleInfiniteInheritance" )]
	public class Alterations<T> : SuppliedAndExportedItems<IAlteration<T>>, IAlterations<T>
	{
		public Alterations( params IAlteration<T>[] alterations ) : base( alterations ) {}
	}
}