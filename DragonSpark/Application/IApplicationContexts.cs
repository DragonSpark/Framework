using DragonSpark.Model.Selection;

namespace DragonSpark.Application
{
	public interface IApplicationContexts<in TIn, out TContext> : ISelect<TIn, TContext>
		where TContext : IApplicationContext {}
}