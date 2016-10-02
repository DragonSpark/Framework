using DragonSpark.Specifications;
using System.Windows.Input;

namespace DragonSpark.Commands
{
	public interface ICommand<in T> : ICommand, ISpecification<T>
	{
		void Execute( T parameter );

		void Update();
	}
}