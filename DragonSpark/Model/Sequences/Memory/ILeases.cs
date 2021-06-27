using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Memory
{
	public interface ILeases<T> : ISelect<uint, Lease<T>> {}
}