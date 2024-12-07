using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store.Operations.Memory;

sealed class Adapter : Command<PostEvictionInput>
{
	public Adapter(ICommand<PostEvictionInput> command) : base(command) {}

	public Adapter(Action<PostEvictionInput> command) : base(command) {}

	// ReSharper disable once TooManyArguments
	public void Execute(object key, object? value, EvictionReason reason, object? state)
	{
		Execute(new(key, value, reason, state));
	}
}