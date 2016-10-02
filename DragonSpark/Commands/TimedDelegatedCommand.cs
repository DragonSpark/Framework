using DragonSpark.Diagnostics;
using SerilogTimings.Extensions;
using System;
using System.Reflection;

namespace DragonSpark.Commands
{
	public sealed class TimedDelegatedCommand<T> : DelegatedCommand<T>
	{
		readonly MethodBase method;
		readonly string template;

		public TimedDelegatedCommand( Action<T> action, string template ) : this( action, action.GetMethodInfo(), template ) {}

		public TimedDelegatedCommand( Action<T> action, MethodBase method, string template ) : base( action )
		{
			this.method = method;
			this.template = template;
		}

		public override void Execute( T parameter )
		{
			using ( Logger.Default.Get( method ).TimeOperation( template, method, parameter ) )
			{
				base.Execute( parameter );
			}
		}
	}
}