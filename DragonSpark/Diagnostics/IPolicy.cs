using DragonSpark.Model.Selection;
using Polly;

namespace DragonSpark.Diagnostics
{
	public interface IPolicy<T> : ISelect<PolicyBuilder<T>, IAsyncPolicy<T>> {}

	public interface IPolicy : ISelect<PolicyBuilder, IAsyncPolicy> {}
}