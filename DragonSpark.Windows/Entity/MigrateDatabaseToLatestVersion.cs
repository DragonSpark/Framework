using DragonSpark.Extensions;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data.Entity;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Windows.Entity
{
	public class MigrateDatabaseToLatestVersion<TContext, TConfiguration> : System.Data.Entity.MigrateDatabaseToLatestVersion<TContext, TConfiguration> where TContext : DbContext where TConfiguration : DbMigrationsConfiguration<TContext>, new()
	{
		public MigrateDatabaseToLatestVersion() : this( false )
		{}

		public MigrateDatabaseToLatestVersion( bool useSuppliedContext ) : base( useSuppliedContext, Activator.Create<TConfiguration>() )
		{}
	}

	public class DbMigrationsConfiguration<TContext> : System.Data.Entity.Migrations.DbMigrationsConfiguration<TContext> where TContext : DbContext
	{
		public DbMigrationsConfiguration() : this( ActivationSource.Instance )
		{}

		public DbMigrationsConfiguration( IActivationSource source )
		{
			source.Apply( this );
		}
	}

	public interface IActivationSource
	{
		void Apply( object item );
	}

	class ActivationSource : IActivationSource
	{
		public static ActivationSource Instance { get; } = new ActivationSource();

		readonly Collection<Type> watching = new Collection<Type>();

		public void Apply( object item )
		{
			var type = item.GetType();
			var canActivate = Activator.CanActivate( type );
			if ( canActivate && !watching.Contains( type ) )
			{
				using ( new Context( watching, type ) )
				{
					var instance = Activator.Create( type );
					if ( instance != item )
					{
						instance.MapInto( item, Mappings.OnlyProvidedValues() );
					}
				}
			}
		}

		class Context : IDisposable
		{
			readonly IList items;
			readonly Type item;

			public Context( IList items, Type item )
			{
				this.items = items;
				this.item = item;
				items.Add( item );
			}

			public void Dispose()
			{
				items.Remove( item );
			}
		}
	}
}