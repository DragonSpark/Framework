using System.Threading.Tasks;
using DragonSpark.Model.Selection;

namespace DragonSpark.Operations
{
	public sealed class SelectOperation<T> : Select<Task<T>, ValueTask<T>>
	{
		public static SelectOperation<T> Default { get; } = new SelectOperation<T>();

		SelectOperation() : base(x => new ValueTask<T>(x)) {}
	}
}