using DragonSpark.ComponentModel;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public abstract class UnityCommand : SetupCommandBase
	{
		[Locate, Required]
		public IUnityContainer Container { [return: Required]get; set; }
	}
}