using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.Testing.TestObjects.IoC
{
	public class SpyStrategy : BuilderStrategy
	{
		public override void PostBuildUp(IBuilderContext context)
		{
			var target = context.Existing as ISpyTarget;
			if ( target != null )
			{
				var policy = context.Policies.Get<ISpyPolicy>( context.BuildKey );
				if ( policy != null && policy.Enabled )
				{
					target.WasSpiedOn = true;
				}
			}

			base.PostBuildUp(context);
		}
	}
}
