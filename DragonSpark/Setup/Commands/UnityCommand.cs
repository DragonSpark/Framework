using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public abstract class UnityCommand : SetupCommand
	{
		[Activate, Required]
		public IUnityContainer Container { [return: Required]get; set; }

		[Activate, Required]
		public IMessageLogger MessageLogger { [return: Required]get; set; }


		// protected override void Execute( SetupContext context )

		// protected abstract void Configure( IUnityContainer container );
	}
}