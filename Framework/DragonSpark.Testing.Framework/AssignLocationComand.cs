using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using System.Reflection;

namespace DragonSpark.Testing.Framework
{
	public static class SetupContextExtensions
	{
		public static IFixture Fixture( this SetupContext @this )
		{
			var result = @this.Item<SetupAutoDataContext>().Fixture;
			return result;
		}

		public static MethodInfo Method( this SetupContext @this )
		{
			var result = @this.Item<SetupAutoDataContext>().Method;
			return result;
		}
	}

	public class AssignServiceLocatorCommand : DragonSpark.Setup.AssignServiceLocatorCommand
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