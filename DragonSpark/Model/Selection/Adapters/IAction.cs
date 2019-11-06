using DragonSpark.Runtime;

namespace DragonSpark.Model.Selection.Adapters
{
	public interface IAction<in T> : ISelect<T, None> {}
}