using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Reflection;

namespace DragonSpark.Application.Compose;

sealed class ApplyNameConfiguration : ICommand<IWebHostBuilder>
{
	readonly Func<string> _name;

	public ApplyNameConfiguration(IResult<Assembly> assembly)
		: this(assembly.Then().Select(x => x.GetName().Name.Verify())) {}

	public ApplyNameConfiguration(Func<string> name) => _name = name;

	public void Execute(IWebHostBuilder parameter)
	{
		parameter.UseSetting(WebHostDefaults.ApplicationKey, _name());
	}
}