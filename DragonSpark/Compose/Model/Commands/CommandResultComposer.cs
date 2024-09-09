using DragonSpark.Compose.Model.Results;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;

namespace DragonSpark.Compose.Model.Commands;

public class CommandResultComposer : ResultComposer<ICommand>
{
	public CommandResultComposer(IResult<ICommand> instance) : base(instance) {}

	public CommandComposer Assume() => new(new Assume(Get()));
}

public class CommandResultComposer<T> : ResultComposer<ICommand<T>>
{
	public CommandResultComposer(IResult<ICommand<T>> instance) : base(instance) {}

	public CommandComposer<T> Assume() => new(new DragonSpark.Model.Commands.Assume<T>(Get()));
}