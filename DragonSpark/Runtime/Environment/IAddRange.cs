using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Environment
{
	public interface IAddRange<T> : ICommand<Store<T>> {}
}