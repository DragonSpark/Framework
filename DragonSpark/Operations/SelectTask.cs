using System.Threading.Tasks;
using DragonSpark.Model.Selection;

namespace DragonSpark.Operations
{
	public sealed class SelectTask<T> : Select<ValueTask<T>, Task<T>>
	{
		public static SelectTask<T> Default { get; } = new SelectTask<T>();

		SelectTask() : base(x => x.AsTask()) {}
	}
}