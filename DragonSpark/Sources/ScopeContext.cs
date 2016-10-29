using DragonSpark.Application;
using System;

namespace DragonSpark.Sources
{
	public class ScopeContext : SuppliedSource<object>
	{
		readonly Func<object> defaultScope;

		public ScopeContext() : this( Execution.Context ) {}

		public ScopeContext( ISource<ISource> defaultScope ) : this( defaultScope.GetValue ) {}

		public ScopeContext( Func<object> defaultScope )
		{
			this.defaultScope = defaultScope;
		}

		public override object Get() => SourceCoercer.Default.Coerce( base.Get() ) ?? defaultScope();
	}
}