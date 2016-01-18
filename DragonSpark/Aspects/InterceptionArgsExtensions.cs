using DragonSpark.Extensions;
using PostSharp.Aspects;

namespace DragonSpark.Aspects
{
	public static class InterceptionArgsExtensions
	{
		public static object GetReturnValue( this MethodInterceptionArgs @this ) => @this.With( x => x.Proceed() ).ReturnValue;
		// public static object GetTarget( this InterceptionArgs @this ) => @this.With( x => x.Proceed() ).ReturnValue;
	}
}