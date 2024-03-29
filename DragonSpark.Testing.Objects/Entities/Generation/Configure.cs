﻿using AutoBogus;
using AutoBogus.Conventions;
using DragonSpark.Model.Commands;

namespace DragonSpark.Testing.Objects.Entities.Generation;

sealed class Configure<T> : ICommand<IAutoGenerateConfigBuilder> where T : class
{
	public static Configure<T> Default { get; } = new();

	Configure() : this(ModelBinder<T>.Default) {}

	readonly IAutoBinder _binder;

	public Configure(IAutoBinder binder) => _binder = binder;

	public void Execute(IAutoGenerateConfigBuilder parameter)
	{
		parameter.WithConventions().WithBinder(_binder);
	}
}