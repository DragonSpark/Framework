using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose;

public readonly record struct HostingInput(IHostBuilder Host, HostBuilderContext Context,
                                           IConfigurationBuilder Configuration);