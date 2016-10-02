using PostSharp.Aspects;

namespace DragonSpark.Aspects.Relay
{
	public sealed class CommandMethodAspect : MethodAspectBase
	{
		public override void OnInvoke( MethodInterceptionArgs args )
		{
			var invocation = args.Instance as ICommandRelay;
			if ( invocation != null )
			{
				invocation.Execute( args.Arguments[0] );
			}
			else
			{
				args.Proceed();
			}
		}
	}
}