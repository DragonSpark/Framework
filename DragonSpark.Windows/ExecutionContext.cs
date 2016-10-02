using DragonSpark.Sources;
using System;

namespace DragonSpark.Windows
{
	[Priority( Priority.AfterNormal )]
	sealed class ExecutionContext : Source<AppDomain>
	{
		public static ISource Default { get; } = new ExecutionContext();
		ExecutionContext() : base( AppDomain.CurrentDomain ) {}
	}
}