using System.Threading;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Windows
{
	public class TaskLocalValue<T> : WritableValue<T>
	{
		readonly AsyncLocal<T> local;

		public TaskLocalValue() : this( new AsyncLocal<T>() )
		{}

		public TaskLocalValue( AsyncLocal<T> local )
		{
			this.local = local;
		}

		public override void Assign( T item )
		{
			local.Value = item;
		}

		public override T Item => local.Value;
	}
}