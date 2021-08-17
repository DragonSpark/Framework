using DragonSpark.Application.Entities;
using DragonSpark.Model.Commands;
using LightInject;

namespace DragonSpark.Application
{
	sealed class CompositionRegistrations : ICommand<IServiceContainer>
	{
		public static CompositionRegistrations Default { get; } = new CompositionRegistrations();

		CompositionRegistrations() {}

		public void Execute(IServiceContainer parameter)
		{
			parameter.Decorate(typeof(ISave<>), typeof(StateAwareSave<>));
		}
	}
}
