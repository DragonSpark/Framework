using System;

namespace DragonSpark.Runtime
{
	public class PropertyContext<TItem>
	{
		readonly Func<TItem> resolver;
		readonly BitFlipper flipper = new BitFlipper();

		public PropertyContext( Func<TItem> resolver )
		{
			this.resolver = resolver;
		}

		public TItem Value
		{
			get
			{
				flipper.CheckWith( () => Equals( value, default( TItem ) ), () => value = resolver() );
				return value;
			}
		}	TItem value;
	}
}