using System.Collections.Generic;

namespace DragonSpark
{
	public sealed class PriorityComparer : IComparer<IPriorityAware>
	{
		public static PriorityComparer Default { get; } = new PriorityComparer();

		public int Compare( IPriorityAware x, IPriorityAware y ) => Comparer<Priority>.Default.Compare( x.Priority, y.Priority );
	}
}