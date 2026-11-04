using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class PolicyAwarePage : PolicyAware<uint>
{
	public PolicyAwarePage(IStopAware<uint> previous) : base(previous, PagePolicy.Default) {}
}