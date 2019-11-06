using System.Threading;
using JetBrains.Annotations;
using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Execution
{
	public class Logical<T> : IMutable<T>
	{
		readonly AsyncLocal<T> _local;

		[UsedImplicitly]
		public Logical() : this(new AsyncLocal<T>()) {}

		public Logical(AsyncLocal<T> local) => _local = local;

		public T Get() => _local.Value;

		public void Execute(T parameter)
		{
			_local.Value = parameter;
		}
	}
}