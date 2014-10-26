using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Build;
using DragonSpark.Markup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using System.Windows.Markup;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Server.Entity.Installation
{
	[ContentProperty( "Steps" )]
	public abstract class CompositeInstallationStep<TContext> : InstallationStep<TContext> where TContext : DbContext
	{
		protected override void Execute( TContext context )
		{
			Steps.GetItems().Apply( x => x.Execute( context ) );
		}

		public ConfigurationCollection<IInstallationStep> Steps
		{
			get { return steps; }
		}	readonly ConfigurationCollection<IInstallationStep> steps = new ConfigurationCollection<IInstallationStep>();
	}

	public static class EntityInstallation
	{
		public static void Install<TContext>( this TContext context ) where TContext : DbContext, IEntityInstallationStorage
		{
			var installation = Activator.Create<InstallationContext>();
			installation.Install( context );
		}
	}

	public class EntityInstallationStep : EntityInstallationStep<DbContext, object>
	{}

	[ContentProperty( "Attach" )]
	public class EntityInstallationStep<TContext, TEntity> : InstallationStep<TContext> where TEntity : class where TContext : DbContext
	{
		protected override void Execute( TContext context )
		{
			Remove.Apply( y => context.Get( y ).NotNull( x => context.Remove( x ) ) );

			Attach.Apply( y => ApplyAttach( context, y ) );
		}

		protected virtual void ApplyAttach( TContext context, TEntity entity )
		{
			context.ApplyChanges( entity );
		}

		public Collection<TEntity> Attach
		{
			get { return attach ?? ( attach = new Collection<TEntity>() ); }
		}	Collection<TEntity> attach;

		public Collection<TEntity> Remove
		{
			get { return remove ?? ( remove = new Collection<TEntity>() ); }
		}	Collection<TEntity> remove;
	}

	public interface IEntityInstallationStorage
	{
		IDbSet<InstallationEntry> GetInstallations();
	}

	public interface IInstallationStep
	{
		void Execute( DbContext context );
	}

	[InheritedExport]
	public interface IInstaller
	{
		Guid Id { get; }

		Version Version { get; }

		Type ContextType { get; }

		IEnumerable<IInstallationStep> Steps { get; }
	}

	public class InstallationContext
	{
		readonly IOperationMonitor monitor;
		readonly IEnumerable<IInstaller> installers;

		public InstallationContext( IOperationMonitor monitor, IEnumerable<IInstaller> installers )
		{
			this.monitor = monitor;
			this.installers = installers;
		}

		public void Install<TContext>( TContext context ) where TContext : DbContext, IEntityInstallationStorage
		{
			var items = installers.OrderBy( x => x.Version ).Where( x => x.ContextType == typeof(TContext) && context.GetInstallations().Find( x.Id, x.Version.ToString() ) == null ).ToArray();
			items.Apply( x =>
			{
				monitor.OnUpdate( string.Format( "Installing Installer '{0}', Version '{1}' with {2} steps", x.Id, x.Version, x.Steps.Count() ) );
				x.Steps.Apply( y =>
				{
					y.Execute( context );
					context.Save();
					monitor.OnUpdate( string.Format( "Executed Installer '{0}', Version '{1}' Step #{2}", x.Id, x.Version, x.Steps.ToList().IndexOf( y ) + 1 ) );
				} );
				context.Create<InstallationEntry>( y => y.SynchronizeFrom( x ) );
				context.Save();
				monitor.OnUpdate( string.Format( "Completed Installer '{0}', Version '{1}'", x.Id, x.Version ) );
			} );
		}
	}

	public interface IOperationMonitor
	{
		[OperationContract( IsOneWay = true )]
		void OnUpdate( string message );

		[OperationContract( IsOneWay = true )]
		void OnComplete();

		[OperationContract( IsOneWay = true )]
		void OnError( string message );
	}

	[AttributeUsage( AttributeTargets.Method )]
	public class MonitorAttribute : Attribute
	{}

	public class OperationMonitorInterceptionBehavior : InterceptionBehaviorBase
	{
		readonly IOperationMonitor monitor;

		public OperationMonitorInterceptionBehavior( IOperationMonitor monitor )
		{
			this.monitor = monitor;
		}

		protected override IMethodReturn Invoke( IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext )
		{
			var candidates = new[] { input.MethodBase, input.Target.GetType().GetMethod( input.MethodBase.Name, input.MethodBase.GetParameters().Select( x => x.ParameterType ).ToArray() ) }.NotNull().ToArray();
			var result = getNext()( input, getNext );
			
			var enable = candidates.Any( x => x.IsDecoratedWith<MonitorAttribute>() );
			if ( enable )
			{
				if ( result.Exception != null )
				{
					var message = ServiceFaultException.DetermineMessage( result.Exception.Message, result.Exception );
					monitor.OnError( message );
				}
				monitor.OnComplete();
			}
			return result;
		}
	}

	public class InstallationEntry
	{
		[Key, Column( Order = 0 )]
		public Guid Id { get; set; }

		[CurrentTimeOffsetDefault]
		public DateTimeOffset Installed { get; set; }

		[NotMapped]
		public Version Version
		{
			get { return VersionStorage.Transform( x => new Version( x ) ); }
			set { VersionStorage = value.Transform( x => x.ToString() ); }
		}

		[Key, Column( Order = 1 ), LocalStorage]
		public string VersionStorage { get; set; }
	}

	public abstract class InstallationStep<TContext> : IInstallationStep where TContext : DbContext
	{
		void IInstallationStep.Execute( DbContext context )
		{
			this.BuildUpOnce();

			context.As<TContext>( Execute );
		}

		protected abstract void Execute( TContext context );
	}

	[ContentProperty( "Steps" )]
	public abstract class Installer : IInstaller
	{
		public Guid Id { get; set; }

		[TypeConverter( typeof(VersionConverter) )]
		public Version Version { get; set; }

		public Type ContextType { get; set; }

		IEnumerable<IInstallationStep> IInstaller.Steps
		{
			get { return Steps.GetItems(); }
		}

		public ConfigurationCollection<IInstallationStep> Steps
		{
			get { return steps; }
		}	readonly ConfigurationCollection<IInstallationStep> steps = new ConfigurationCollection<IInstallationStep>();
	}
}
