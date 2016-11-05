using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Windows.Legacy.Markup
{
	public sealed class Incrementer : IIncrementer
	{
		readonly DecoratedSourceCache<int> count = new DecoratedSourceCache<int>();

		public int NextCount( object context )
		{
			var result = count.Get( context ) + 1;
			count.Set( context, result );
			return result;
		}
	}
}