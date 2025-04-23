using Microsoft.Extensions.Hosting;
using Uno.Extensions.Hosting;

namespace DragonSpark.Application.Mobile.Uno.Run;

public sealed record Application(IApplicationBuilder Builder, IHost Host);
