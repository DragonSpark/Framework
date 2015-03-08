using System;
using DragonSpark.Application;
using DragonSpark.Application.Presentation.Commands;
using DragonSpark.Objects;
using Microsoft.Practices.Unity;

namespace DragonSpark.Features.Modules.Exceptions
{
	public class Module : ApplicationModule<Configuration>
	{
		public Module( IUnityContainer container, IModuleMonitor monitor ) : base( container, monitor )
		{}
	}

	public class ThrowClientExceptionCommand : CommandBase
	{
		protected override void Execute( object parameter )
		{
			throw new InvalidOperationException( "This is an exception from the client." );
		}
	}

	public class ThrowServiceExceptionCommand : OperationCommandBase
	{
		readonly IFeaturesApplicationService service;

		public ThrowServiceExceptionCommand( IFeaturesApplicationService service )
		{
			this.service = service;
		}

		protected override void ExecuteCommand( ICommandMonitor monitor )
		{
			service.ThrowException();
		}

		[DefaultPropertyValue( "Throwing..." )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}
	}
}
