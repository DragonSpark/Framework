using System;

namespace DragonSpark.Composition.Scopes.Hierarchy;

public interface IScopedServiceProvider : IServiceProvider, IDisposable, IAsyncDisposable {}