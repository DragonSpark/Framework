using DragonSpark.Model.Selection;

namespace DragonSpark.Application
{
	public class ApplicationContexts<TIn, TContext> : Select<TIn, IApplicationContext<TIn>>
		where TContext : IApplicationContext<TIn>
	{
		protected ApplicationContexts(ISelect<TIn, IServices> services)
			: base(services.Select(ServiceSelector<TContext>.Default).Then().Cast<IApplicationContext<TIn>>().Get()) {}
	}

	public class ApplicationContexts<TContext, TIn, TOut> : Select<TIn, IApplicationContext<TIn, TOut>>
		where TContext : IApplicationContext<TIn, TOut>
	{
		protected ApplicationContexts(ISelect<TIn, IServices> services)
			: base(services.Select(ServiceSelector<TContext>.Default)
			               .Then()
			               .Cast<IApplicationContext<TIn, TOut>>()
			               .Get()) {}
	}
}