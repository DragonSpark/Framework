using System;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Commands
{
	public class ReportExceptionCommand : OperationCommandBase<Exception>
	{
		readonly IApplicationService applicationService;
		readonly BitFlipper execute = new BitFlipper();

		public ReportExceptionCommand( IApplicationService applicationService )
		{
			this.applicationService = applicationService;
		}

		protected override bool CanExecute( ICommandMonitor monitor )
		{
			return !execute.Flipped && base.CanExecute( monitor );
		}

		protected override void ExecuteCommand( ICommandMonitor monitor )
		{
			execute.Check( () =>
			{
				var detail = new ClientExceptionDetail( ContextChecked );
				applicationService.ReportException( detail );
			} );
		}

		[DefaultPropertyValue( "Reporting Exception to Service" )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}
	}
}