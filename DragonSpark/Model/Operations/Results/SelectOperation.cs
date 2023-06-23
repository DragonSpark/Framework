using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Results;

public sealed class SelectOperation<T> : Select<Task<T>, ValueTask<T>>
{
	public static SelectOperation<T> Default { get; } = new();

	SelectOperation() : base(x => new ValueTask<T>(x)) {}
}