using DragonSpark.Model.Commands;
using Uno.Extensions.Hosting;

namespace DragonSpark.Application.Mobile.Design;

sealed class ConfigureDesigner : Command<IApplicationBuilder>, IConfigureDesigner
{
	public static ConfigureDesigner Default { get; } = new();

	ConfigureDesigner() : base(_ => {}) {}
}
