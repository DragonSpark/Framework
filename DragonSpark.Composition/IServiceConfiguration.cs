using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

public interface IServiceConfiguration : ICommand<IServiceCollection> {}