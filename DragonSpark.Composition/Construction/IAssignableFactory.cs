using DragonSpark.Model.Results;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Construction;

public interface IAssignableFactory : IMutable<IServiceProviderFactory<IServiceContainer>>,
                                      IServiceProviderFactory<IServiceContainer>;