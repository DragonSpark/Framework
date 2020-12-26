﻿using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Compose.Extents.Commands
{
	public interface ICommandContext
	{
		CommandExtent Of { get; }
	}

	public sealed class CommandContext : ICommandContext
	{
		public static CommandContext Default { get; } = new CommandContext();

		CommandContext() {}

		public CommandExtent Of => CommandExtent.Default;
	}

	public sealed class CommandContext<T>
	{
		public static CommandContext<T> Instance { get; } = new CommandContext<T>();

		CommandContext() {}

		public Model.CommandContext<T> Empty { get; } = EmptyCommand<T>.Default.Then();

		public Model.CommandContext<T> Calling(Action<T> body) => new Model.CommandContext<T>(new Command<T>(body));
	}
}