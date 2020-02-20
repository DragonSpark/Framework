using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Execution;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class Initialized<T> : IAlteration<T> where T : DbContext
	{
		public Initialized() : this(new First()) {}

		readonly ICondition _condition;

		public Initialized(ICondition condition) => _condition = condition;

		public T Get(T parameter) => _condition.Get() ? parameter.With(z => z.Database.EnsureCreated()) : parameter;
	}
}