using DragonSpark.Activation.FactoryModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Extensions;
using DragonSpark.TypeSystem;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class SetupApplicationCommand<TLogger, TAssemblyProvider> : DragonSpark.Setup.Commands.SetupApplicationCommand<TLogger, TAssemblyProvider> 
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
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