using System;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;

namespace DragonSpark.Compose.Conditions
{
	public sealed class Context
	{
		public static Context Default { get; } = new Context();

		Context() {}

		public Extent Of => Extent.Default;
	}

	public sealed class Context<T>
	{
		public static Context<T> Instance { get; } = new Context<T>();

		Context() {}

		public ICondition<T> Always => Always<T>.Default;

		public ICondition<T> Never => Never<T>.Default;

		public ICondition<T> Assigned => IsAssigned<T>.Default;

		public ICondition<T> Calling(Func<T, bool> condition) => new Condition<T>(condition);
	}
}