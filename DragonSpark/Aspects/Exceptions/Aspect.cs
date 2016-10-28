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
			source?.Get().Execute( args.Proceed );
		}
	}
}