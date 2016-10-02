using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Windows.Markup
{
	public sealed class Incrementer : IIncrementer
	{
		readonly DecoratedSourceCache<int> count = new DecoratedSourceCache<int>();

		public int Next( object context )
		{
			var result = count.Get( context ) + 1;
			count.Set( context, result );
			return result;
		}
	}
}