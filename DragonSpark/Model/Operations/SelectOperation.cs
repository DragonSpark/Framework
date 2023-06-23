using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public sealed class SelectOperation : Select<Task, ValueTask>
{
	public static SelectOperation Default { get; } = new();

	SelectOperation() : base(x => new (x)) {}
}