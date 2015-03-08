using System;
using System.Threading;
using DragonSpark.Application.Presentation.Commands;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Launch
{
	public class LoadModulesOperation : OperationCommandBase
	{
		readonly IModuleMonitor monitor;
		readonly ManualResetEvent reset = new ManualResetEvent( false );
		
		public LoadModulesOperation( IModuleMonitor monitor )
		{
			this.monitor = monitor;
		}

		[DefaultPropertyValue( "Loading Primary Application Modules." )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		protected override void ExecuteCommand( ICommandMonitor commandMonitor )
		{
			monitor.Load().IsFalse( () => 
			{
				reset.Reset();
				monitor.Loaded += OnLoaded;
				reset.WaitOne();
			} );
		}

		void OnLoaded( object sender, EventArgs e )
		{
			monitor.Loaded -= OnLoaded;
			reset.Set();
		}
	}
}