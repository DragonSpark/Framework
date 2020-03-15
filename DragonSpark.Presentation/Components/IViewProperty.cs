using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components
{
	public interface IViewProperty : IOperation, ISource {}

	public interface ISource<out T> : ISource
	{
		T Value { get; }
	}
}