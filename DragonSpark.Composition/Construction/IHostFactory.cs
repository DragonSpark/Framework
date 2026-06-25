using DragonSpark.Model.Selection;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Construction;

public interface IHostFactory : ISelect<IHostBuilder, IServiceProviderFactory<IServiceContainer>>;