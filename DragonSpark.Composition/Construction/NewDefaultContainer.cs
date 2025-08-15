using DragonSpark.Model.Results;
using LightInject;

namespace DragonSpark.Composition.Construction;

sealed class NewDefaultContainer : SelectedResult<ContainerOptions, ServiceContainer>
{
    public static NewDefaultContainer Default { get; } = new();

    NewDefaultContainer() : base(DefaultOptions.Default, NewServiceContainer.Default) {}
}