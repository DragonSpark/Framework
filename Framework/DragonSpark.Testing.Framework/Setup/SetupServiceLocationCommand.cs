using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Extensions;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class SetupServiceLocationCommand : DragonSpark.Setup.Commands.SetupServiceLocationCommand
	{
		[ComponentModel.Singleton( typeof(AmbientLocatorKeyFactory) )]
		public IFactory<MethodInfo, IAmbientKey> Factory { get; set; }

		protected override void Execute( SetupContext context )
		{
			var key = Factory.Create( context.Method() );
			Locator.Register( key );

			base.Execute( context );
		}
	}
}