using Microsoft.Extensions.Hosting;
using Uno.Extensions.Hosting;

namespace DragonSpark.Application.Mobile.Run;

public sealed record Application(IApplicationBuilder Builder, IHost Host);