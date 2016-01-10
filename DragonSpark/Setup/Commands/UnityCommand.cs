using DragonSpark.ComponentModel;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public abstract class UnityCommand : SetupCommandBase<IApplicationSetupParameter>
	{
		[Activate, Required]
		public IUnityContainer Container { get; set; }
	}
}