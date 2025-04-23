using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Uno.Design;

public sealed class ApplyDesigner : ICommand<Run.Application>
{
	public static ApplyDesigner Default { get; } = new();

	ApplyDesigner() {}

	public void Execute(Run.Application parameter)
	{
		var (builder, host) = parameter;
		host.Services.GetRequiredService<IConfigureDesigner>().Execute(builder);
	}
}