using DragonSpark.Extensions;

namespace DragonSpark.IoC
{
	public abstract class BehaviorPolicyBase<TTarget> : IBehaviorPolicy where TTarget : class 
	{
		protected abstract void Apply( TTarget target );

		public void Apply( object target )
		{
			target.As<TTarget>( Apply );
		}
	}
}