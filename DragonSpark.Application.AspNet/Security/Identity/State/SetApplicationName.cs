using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Security.Identity.State;

public class SetApplicationName : IAlteration<IDataProtectionBuilder>
{
	readonly IFormatter<IHostEnvironment> _name;

	public SetApplicationName(IFormatter<IHostEnvironment> name) => _name = name;

	public IDataProtectionBuilder Get(IDataProtectionBuilder parameter)
		=> parameter.SetApplicationName(_name.Get(parameter.Services.GetRequiredInstance<IHostEnvironment>()))
		            .Return(parameter);
}