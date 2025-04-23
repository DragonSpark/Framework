using DragonSpark.Model.Commands;
using Uno.Extensions.Hosting;

namespace DragonSpark.Application.Mobile.Uno.Design;

sealed class ConfigureDesigner : Command<IApplicationBuilder>, IConfigureDesigner
{
	public static ConfigureDesigner Default { get; } = new();

	ConfigureDesigner() : base(_ => {}) {}
}
