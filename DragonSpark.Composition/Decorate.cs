using DragonSpark.Model.Commands;
using LightInject;
using System;

namespace DragonSpark.Composition;

sealed class Decorate<T> : ICommand<IServiceContainer>
{
	readonly Func<IServiceFactory, T, T> _configure;

	public Decorate(Func<IServiceFactory, T, T> configure) => _configure = configure;

	public void Execute(IServiceContainer parameter)
	{
		parameter.Decorate(_configure);
	}
}

sealed class Decorate<TFrom, TTo> : ICommand<IServiceContainer> where TTo : TFrom
{
	public static Decorate<TFrom,TTo> Default { get; } = new();

	Decorate() {}

	public void Execute(IServiceContainer parameter)
	{
		parameter.Decorate<TFrom, TTo>();
	}
}