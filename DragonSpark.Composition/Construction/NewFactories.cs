using System.Collections.Generic;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Construction;

sealed class NewFactories : ReferenceValueStore<IDictionary<object, object>, IServiceProviderFactory<IServiceContainer>>
{
    public static NewFactories Default { get; } = new();

    NewFactories() : base(NewFactory.Default.Then().Any()) {}
}