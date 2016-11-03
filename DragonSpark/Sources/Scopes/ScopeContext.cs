using DragonSpark.Application;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Sources.Scopes
{
	public class ScopeContext : SuppliedSource<object>
	{
		readonly Func<object> defaultScope;

		public ScopeContext() : this( Execution.Default ) {}

		public ScopeContext( ISource<ISource> defaultScope ) : this( defaultScope.GetValue ) {}

		[UsedImplicitly]
		public ScopeContext( Func<object> defaultScope )
		{
			this.defaultScope = defaultScope;
		}

		public override object Get() => SourceCoercer.Default.Get( base.Get() ) ?? defaultScope();
	}
}