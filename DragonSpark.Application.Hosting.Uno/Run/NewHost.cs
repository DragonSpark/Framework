using System;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Uno.Run;

public abstract class NewHost(Func<IHostBuilder, IHostBuilder> alteration) : Alteration<IHostBuilder>(alteration)
{
	public static implicit operator Func<IHostBuilder, IHostBuilder>(NewHost instance) => instance.Get;
}
