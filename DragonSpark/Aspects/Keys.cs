using DragonSpark.Runtime;
using PostSharp.Aspects;

namespace DragonSpark.Aspects
{
	public static class Keys
	{
		public static int For( MethodExecutionArgs args ) => KeyFactory.CreateUsing( args.Instance ?? args.Method.DeclaringType, args.Method, args.Arguments );

		// public static int For( MethodInterceptionArgs args ) => KeyFactory.Default.CreateUsing( args.Default ?? args.Method.DeclaringType, args.Method, args.Arguments );
	}
}