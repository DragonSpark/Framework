using DragonSpark.Commands;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Testing.Framework.Runtime;
using System;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application
{
	public sealed class ApplicationInitializer : CommandBase<MethodBase>
	{
		readonly IComposable<IDisposable> disposables;
		public static IScope<ApplicationInitializer> Default { get; } = new Scope<ApplicationInitializer>( Factory.GlobalCache( () => new ApplicationInitializer() ) );
		ApplicationInitializer() : this( Disposables.Default ) {}
		
		ApplicationInitializer( IComposable<IDisposable> disposables )
		{
			this.disposables = disposables;
		}

		public override void Execute( MethodBase parameter )
		{
			MethodContext.Default.Assign( parameter );
			disposables.Add( ExecutionContext.Default.Get() );
		}
	}
}