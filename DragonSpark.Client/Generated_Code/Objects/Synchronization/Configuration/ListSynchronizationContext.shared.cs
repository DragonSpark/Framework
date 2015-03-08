using DragonSpark.Objects.Configuration;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	public class ListSynchronizationContext : ObjectResolvingSynchronizationContext
	{
		protected override Synchronization.SynchronizationContext Create()
		{
			var result = new Synchronization.ListSynchronizationContext( ObjectResolver.Instance, FirstExpression ?? Expression, SecondExpression ?? Expression, MappingName, IncludeBasePolicies );
			return result;
		}
	}
}