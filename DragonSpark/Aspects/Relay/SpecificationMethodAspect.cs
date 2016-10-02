using PostSharp.Aspects;

namespace DragonSpark.Aspects.Relay
{
	public sealed class SpecificationMethodAspect : MethodAspectBase
	{
		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var invocation = args.Instance as ISpecificationRelay;
			if ( invocation != null )
			{
				args.ReturnValue = invocation.IsSatisfiedBy( args.Arguments[0] );
			}
			else
			{
				args.Proceed();
			}
		}
	}
}