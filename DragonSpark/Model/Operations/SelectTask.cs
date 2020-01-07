using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public sealed class SelectTask<T> : Select<ValueTask<T>, Task<T>>
	{
		public static SelectTask<T> Default { get; } = new SelectTask<T>();

		SelectTask() : base(x => x.AsTask()) {}
	}
}