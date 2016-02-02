using DragonSpark.Runtime;
using System.Windows.Input;

namespace DragonSpark.Setup
{
	public interface ISetup : ICommand {}

	public interface ISetup<in T> : ISetup, ICommand<T> {}
}