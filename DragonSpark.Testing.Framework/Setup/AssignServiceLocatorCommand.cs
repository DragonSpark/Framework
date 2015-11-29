using System.Reflection;
using DragonSpark.Activation.Build;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AssignServiceLocatorCommand : DragonSpark.Setup.Commands.AssignServiceLocatorCommand
	{
		[ComponentModel.Singleton( typeof(AmbientLocatorKeyFactory) )]
		public IFactory<MethodInfo, IAmbientKey> Factory { get; set; }

		protected override void Assign( SetupContext context, IServiceLocator locator )
		{
			var key = Factory.Create( context.Method() );
			locator.Register( key );

			base.Assign( context, locator );
		}
	}
}