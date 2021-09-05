using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities
{
	class Class5 {}

	sealed class FixedInstanceContexts<T> : Instance<T>, IContexts<T> where T : DbContext
	{
		public FixedInstanceContexts(T instance) : base(instance) {}
	}
}