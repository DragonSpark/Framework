using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Reflection;
using System.Windows.Markup;

namespace DragonSpark.Entity
{
	[ContentProperty( "InitializerKey" )]
	public class DatabaseInitializationConfigurationCommand : IContainerConfigurationCommand
	{
		static readonly MethodInfo InitializeMethod = typeof(DatabaseInitializationConfigurationCommand).GetMethod( "Initialize", DragonSparkBindingOptions.AllProperties );

		public System.Type ContextType { get; set; }

		static void Initialize<TContext>( IUnityContainer container ) where TContext : DbContext, new()
		{
		    var databaseInitializer = container.TryResolve<IDatabaseInitializer<TContext>>();
			databaseInitializer.NotNull( Database.SetInitializer );

			using ( var context = new TContext() )
			{
				context.Database.Initialize(true);
			}
		}

	    public void Configure( IUnityContainer container )
		{
			InitializeMethod.MakeGenericMethod( ContextType ).Invoke( null, new object[] { container } );
		}
	}
}