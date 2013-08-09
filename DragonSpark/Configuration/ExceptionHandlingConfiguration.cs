using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using DragonSpark.Objects;
using DragonSpark.Runtime;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;
using IExceptionHandler = DragonSpark.Runtime.IExceptionHandler;

namespace DragonSpark.Configuration
{
	[ContentProperty( "Policies" )]
	public class ExceptionHandlingConfiguration : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			var manager = new ExceptionManager( Policies.Select( x => x.Instance ).ToArray() );
			container.RegisterInstance( manager );
			ExceptionPolicy.SetExceptionManager( manager );

			container.TryResolve<IExceptionHandler>().NotNull( ConfigureExceptionHandling );
		}
		
		protected virtual void ConfigureExceptionHandling( IExceptionHandler handler )
		{
			AppDomain.CurrentDomain.With( x => x.UnhandledException += ( s, args ) => args.ExceptionObject.As<Exception>( handler.Process ) );
		}

		public Collection<ExceptionPolicyDefinition> Policies
		{
			get { return policies; }
		}	readonly Collection<ExceptionPolicyDefinition> policies = new Collection<ExceptionPolicyDefinition>();
	}

	[ContentProperty( "Entries" )]
	public class ExceptionPolicyDefinition : Singleton<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition>
	{
		public string PolicyName { get; set; }

		public Collection<ExceptionPolicyEntry> Entries
		{
			get { return entries; }
		}	readonly Collection<ExceptionPolicyEntry> entries = new Collection<ExceptionPolicyEntry>();

		protected override Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition Create()
		{
			var result = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition( PolicyName, Entries.Select( x => x.Instance ).ToArray() );
			return result;
		}
	}

	[ContentProperty( "Handlers" )]
	public class ExceptionPolicyEntry : Singleton<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry>
	{
		public Type ExceptionType { get; set; }

		public PostHandlingAction Action { get; set; }

		public ConfigurationCollection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler> Handlers
		{
			get { return handlers; }
		}	readonly ConfigurationCollection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler> handlers = new ConfigurationCollection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler>();

		protected override Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry Create()
		{
			var items = Handlers.GetItems( () => ServiceLocation.Locate<IUnityContainer>() );
			var result = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry( ExceptionType, Action, items );
			return result;
		}
	}

	public class ConfigurationCollection<TItem> : List<object>
	{
		public IEnumerable<TItem> GetItems( Func<object> source = null )
		{
			var result = this.Select( x => x.ResolvedAs<TItem>( source ) ).NotNull().ToArray();
			return result;
		}
	}

	public class ReplaceHandler : Singleton<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ReplaceHandler>
	{
		public string Message { get; set; }

		public Type ReplacementType { get; set; }

		protected override Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ReplaceHandler Create()
		{
			var result = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ReplaceHandler( Message, ReplacementType );
			return result;
		}
	}

	public class WrapHandler : Singleton<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WrapHandler>
	{
		public string Message { get; set; }

		public Type WrapExceptionType { get; set; }

		protected override Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WrapHandler Create()
		{
			var result = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WrapHandler( Message, WrapExceptionType );
			return result;
		}
	}
}