using DragonSpark.Model.Commands;
using LightInject;
using System;

namespace DragonSpark.Composition;

public class ContainerConfiguration : Command<IServiceContainer>, IContainerConfiguration
{
	public ContainerConfiguration(Action<IServiceContainer> command) : base(command) {}
}