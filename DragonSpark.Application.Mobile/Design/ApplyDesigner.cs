using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Design;

public sealed class ApplyDesigner : ICommand<DragonSpark.Application.Mobile.Run.Application>
{
	public static ApplyDesigner Default { get; } = new();

	ApplyDesigner() {}

	public void Execute(DragonSpark.Application.Mobile.Run.Application parameter)
	{
		var (builder, host) = parameter;
		host.Services.GetRequiredService<IConfigureDesigner>().Execute(builder);
	}
}