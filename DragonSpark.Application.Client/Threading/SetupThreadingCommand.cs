using DragonSpark.Activation.IoC;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Unity;

namespace DragonSpark.Application.Client.Threading
{
	public class SetupThreadingCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			context.Container().Resolve<IDispatchHandler>();
		}
	}
}