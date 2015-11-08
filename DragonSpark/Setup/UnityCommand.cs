using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup
{
	public abstract class UnityCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			Configure( context.Container() );
		}

		protected abstract void Configure( IUnityContainer container );
	}
}