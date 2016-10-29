using PostSharp.Aspects;
using System;

namespace DragonSpark.Aspects.Exceptions
{
	[AttributeUsage( AttributeTargets.Method )]
	public sealed class Aspect : AspectBase
	{
		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var source = args.Instance as IPolicySource;
			var policy = source?.Get();
			policy.Execute( args.Proceed );
		}
	}
}