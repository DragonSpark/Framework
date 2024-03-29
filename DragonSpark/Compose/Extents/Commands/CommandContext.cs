﻿using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Compose.Extents.Commands;

public sealed class CommandContext : ICommandContext
{
	public static CommandContext Default { get; } = new();

	CommandContext() {}

	public CommandExtent Of => CommandExtent.Default;
}

public sealed class CommandContext<T>
{
	public static CommandContext<T> Instance { get; } = new();

	CommandContext() {}

	public Model.Commands.CommandContext<T> Empty { get; } = EmptyCommand<T>.Default.Then();

	public Model.Commands.CommandContext<T> Calling(Action<T> body) => new(new Command<T>(body));
}