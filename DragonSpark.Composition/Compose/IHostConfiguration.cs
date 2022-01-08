using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose;

public interface IHostConfiguration : ICommand<IHostBuilder> {}