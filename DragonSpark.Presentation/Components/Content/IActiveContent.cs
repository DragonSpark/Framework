using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Content;

public interface IActiveContent<T> : ICommand<T>, IResulting<T?>
{
	IUpdateMonitor Monitor { get; }
}