using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Environment
{
	public interface IRegistry<T> : IArray<T>, IAddRange<T>, ICommand<T> {}
}