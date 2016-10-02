using PostSharp.Aspects;
using System;

namespace DragonSpark.Aspects.Exceptions
{
	[AttributeUsage( AttributeTargets.Method )]
	public sealed class AppliedAspect : AspectBase
	{
		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var source = args.Instance as IPolicySource;
			if ( source != null )
			{
				source.Get().Execute( args.Proceed );
			}
			else
			{
				args.Proceed();
			}
		}
	}
}