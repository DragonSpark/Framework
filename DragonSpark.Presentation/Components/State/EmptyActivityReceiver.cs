using DragonSpark.Model.Commands;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public sealed class EmptyActivityReceiver : IActivityReceiver
{
	public static EmptyActivityReceiver Default { get; } = new();

	EmptyActivityReceiver() : this(EmptyCommand<IRenderAware>.Default) {}

	public EmptyActivityReceiver(ICommand<IRenderAware> command) : this(command, command) {}

	public EmptyActivityReceiver(ICommand<IRenderAware> add, ICommand<IRenderAware> remove)
	{
		Add    = add;
		Remove = remove;
	}

	public ICommand<IRenderAware> Add { get; }

	public ICommand<IRenderAware> Remove { get; }

	public bool Active => true;

	public ValueTask Get(ActivityReceiverState parameter) => ValueTask.CompletedTask;

	public ValueTask<ActivityReceiverState?> Get() => ValueTask.FromResult(default(ActivityReceiverState?));
}