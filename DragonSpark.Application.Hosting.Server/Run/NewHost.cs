using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Server.Run;

public abstract class NewHost(Func<IHostBuilder, IHostBuilder> alteration) : Alteration<IHostBuilder>(alteration)
{
	public static implicit operator Func<IHostBuilder, IHostBuilder>(NewHost instance) => instance.Get;
}