using DragonSpark.Extensions;
using DragonSpark.Sources;
using System.Threading;

namespace DragonSpark.Windows.Runtime
{
	public class TaskLocalStore<T> : AssignableSourceBase<T>
	{
		readonly AsyncLocal<T> local;

		public TaskLocalStore() : this( new AsyncLocal<T>() ) {}

		public TaskLocalStore( AsyncLocal<T> local )
		{
			this.local = local;
		}

		public override void Assign( T item )
		{
			local.Value = item;
		}

		public override T Get() => local.Value;

		protected override void OnDispose()
		{
			local.Value?.TryDispose();
			local.Value = default(T);
		}
	}
}