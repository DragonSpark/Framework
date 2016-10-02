using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Runtime;
using System;
using System.Reflection;
using TimedOperationFactory = DragonSpark.Testing.Framework.Diagnostics.TimedOperationFactory;

namespace DragonSpark.Testing.Framework.Runtime
{
	public sealed class MethodOperationContext : InitializedDisposableAction
	{
		readonly static Action Run = PurgeLoggerMessageHistoryCommand.Default.Fixed( Output.Default.Get ).ToRunDelegate();

		readonly IDisposable disposable;

		public MethodOperationContext( MethodBase method ) : this( TimedOperationFactory.Default.Get( method ) ?? new DisposableAction( () => {} ) ) {}

		public MethodOperationContext( IDisposable disposable ) : base( Run )
		{
			this.disposable = disposable;
		}

		protected override void OnDispose( bool disposing )
		{
			disposable.Dispose();
			base.OnDispose( disposing );
		}
	}
}