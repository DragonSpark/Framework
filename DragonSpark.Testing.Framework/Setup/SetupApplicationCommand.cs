using DragonSpark.Activation.FactoryModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework.Extensions;
using DragonSpark.TypeSystem;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class SetupApplicationCommand<TLogger, TAssemblyProvider> : DragonSpark.Setup.Commands.SetupApplicationCommand<TLogger, TAssemblyProvider> 
		where TLogger : IMessageLogger
		where TAssemblyProvider : IAssemblyProvider
	{
		[ComponentModel.Singleton( typeof(AmbientKeyFactory<IServiceLocator>) )]
		public IFactory<MethodInfo, IAmbientKey> Factory { get; set; }

		protected override void Execute( SetupContext context )
		{
			var key = Factory.Create( context.Method() );
			Locator.Register( key );

			base.Execute( context );
		}
	}

	/*public class RegisterAmbientObjectBuilder : RegisterAmbientItem<IObjectBuilder>
	{ }*/

	/*public abstract class RegisterAmbientItem<T> : SetupCommand
	{
		[Activate]
		public AmbientKeyFactory<T> Factory { get; set; }

		public T Instance { get; set; }

		protected override void Execute( SetupContext context )
		{
			var key = Factory.Create( context.Method() );
			AmbientValues.Register( key, Instance );
		}
	}*/
}