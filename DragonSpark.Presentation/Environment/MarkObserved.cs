using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Environment;

public sealed class MarkObserved : ICommand
{
	public static MarkObserved Default { get; } = new();

	MarkObserved() : this(ShouldObserve.Default) {}

	readonly ICondition<Exception> _condition;

	public MarkObserved(ICondition<Exception> condition) => _condition = condition;

	public void Execute(None parameter)
	{
		TaskScheduler.UnobservedTaskException
			+= (_, args) =>
			   {
				   if (args.Exception.InnerException is {} e && _condition.Get(e))
				   {
					   args.SetObserved();
				   }
			   };
	}
}