using DragonSpark.Diagnostics;
using SerilogTimings.Extensions;
using System;
using System.Reflection;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class TimedDelegatedSource<TParameter, TResult> : DelegatedParameterizedSource<TParameter, TResult>
	{
		readonly MethodBase method;
		readonly string template;

		public TimedDelegatedSource( Func<TParameter, TResult> get, string template ) : this( get, get.GetMethodInfo(), template ) {}

		public TimedDelegatedSource( Func<TParameter, TResult> get, MethodBase method, string template ) : base( get )
		{
			this.method = method;
			this.template = template;
		}

		public override TResult Get( TParameter parameter )
		{
			using ( Logger.Default.Get( method ).TimeOperation( template, method, parameter ) )
			{
				return base.Get( parameter );
			}
		}
	}
}