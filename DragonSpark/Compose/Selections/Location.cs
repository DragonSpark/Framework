using System;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Compose.Selections
{
	public sealed class Location<TIn, TOut> : Select<TIn, TOut>
	{
		public static Location<TIn, TOut> Default { get; } = new Location<TIn, TOut>();

		Location() : base(_ => DefaultComponent<TOut>.Default) {}

		public Alternatives Or => Alternatives.Instance;

		public sealed class Alternatives
		{
			public static Alternatives Instance { get; } = new Alternatives();

			Alternatives() {}

			public ISelect<TIn, TOut> Throw() => DefaultComponentLocator<TOut>.Default.ToSelect(I.A<TIn>());

			public ISelect<TIn, TOut> Default(TOut instance)
				=> new Component<TOut>(instance.Start()).ToSelect(I.A<TIn>());

			public ISelect<TIn, TOut> Default(Func<TOut> result)
				=> new Component<TOut>(result).ToSelect(I.A<TIn>());
		}
	}
}