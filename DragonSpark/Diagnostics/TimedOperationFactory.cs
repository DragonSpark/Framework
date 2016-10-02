using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using SerilogTimings.Extensions;
using System;
using System.Reflection;

namespace DragonSpark.Diagnostics
{
	public class TimedOperationFactory : ParameterizedSourceBase<MethodBase, IDisposable>
	{
		readonly string template;

		public TimedOperationFactory( string template )
		{
			this.template = template;
		}

		public override IDisposable Get( MethodBase parameter ) => Logger.Default.Get( parameter ).TimeOperation( template, parameter.ToItem() );
	}
}
