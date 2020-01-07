using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose.Extents.Conditions
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

		public ICondition<T> Always => Is.Always<T>();

		public ICondition<T> Never => Is.Never<T>();

		public ICondition<T> Assigned => Is.Assigned<T>();

		public ICondition<T> Calling(Func<T, bool> condition) => new Condition<T>(condition);
	}
}