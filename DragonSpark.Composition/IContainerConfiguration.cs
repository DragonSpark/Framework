using DragonSpark.Model.Commands;
using LightInject;

namespace DragonSpark.Composition;

public interface IContainerConfiguration : ICommand<IServiceContainer> {}