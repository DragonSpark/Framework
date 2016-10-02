using PostSharp.Aspects;

namespace DragonSpark.Aspects.Relay
{
	public sealed class ParameterizedSourceMethodAspect : MethodAspectBase
	{
		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var invocation = args.Instance as IParameterizedSourceRelay;
			if ( invocation != null )
			{
				args.ReturnValue = invocation.Get( args.Arguments[0] );
			}
			else
			{
				args.Proceed();
			}
		}
	}
}