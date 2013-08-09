using Microsoft.Practices.ObjectBuilder2;

namespace DragonSpark.IoC
{
	public interface IBehaviorPolicy : IBuilderPolicy
	{
		void Apply( object target );
	}
}