using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Windows.Markup
{
	class Incrementer : IIncrementer
	{
		public int Next( object context )
		{
			var count = new Count( context );
			var result = count.Item + 1;
			count.Assign( result );
			return result;
		}

		class Count : AssociatedValue<int>
		{
			public Count( object instance ) : base( instance, typeof(Count) ) {}
		}
	}
}