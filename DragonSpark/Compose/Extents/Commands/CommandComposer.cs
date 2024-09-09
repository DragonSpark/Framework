using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Compose.Extents.Commands;

public sealed class CommandComposer : ICommandContext
{
	public static CommandComposer Default { get; } = new();

	CommandComposer() {}

	public CommandExtent Of => CommandExtent.Default;
}

public sealed class CommandComposer<T>
{
	public static CommandComposer<T> Instance { get; } = new();

	CommandComposer() {}

	public Model.Commands.CommandComposer<T> Empty { get; } = EmptyCommand<T>.Default.Then();

	public Model.Commands.CommandComposer<T> Calling(Action<T> body) => new(new Command<T>(body));
}