using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Environment.Browser.Document;

public interface IFocusedElement : IAsyncDisposable
{
	IOperation Store { get; }
	IOperation Restore { get; }
}