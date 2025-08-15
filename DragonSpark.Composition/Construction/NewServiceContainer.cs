using System;
using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using LightInject;

namespace DragonSpark.Composition.Construction;

sealed class NewServiceContainer : ISelect<ContainerOptions, ServiceContainer>
{
    public static NewServiceContainer Default { get; } = new();

    NewServiceContainer() : this(ConstructionInfoProvider.Default) {}

    readonly FieldInfo _provider;

    public NewServiceContainer(FieldInfo provider) => _provider = provider;

    public ServiceContainer Get(ContainerOptions parameter)
    {
        var result = new ServiceContainer(parameter);
        result.ConstructorSelector = new ConstructorSelector(new CanSelectDependency(result, parameter).Get);

        var current = _provider.GetValue(result).Verify().To<Lazy<IConstructionInfoProvider>>();

        _provider.SetValue(result, new Lazy<IConstructionInfoProvider>(new Construction(current.Value)));
        return result;
    }
}