using DragonSpark.Model.Commands;
using LightInject;
using System;

namespace DragonSpark.Composition
{
	sealed class Decorate<T> : ICommand<IServiceContainer>
	{
		readonly Func<IServiceFactory, T, T> _configure;

		public Decorate(Func<IServiceFactory, T, T> configure) => _configure = configure;

		public void Execute(IServiceContainer parameter)
		{
			parameter.Decorate(_configure);
		}
	}
}