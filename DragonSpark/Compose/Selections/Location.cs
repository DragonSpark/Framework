using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Environment;
using System;

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

			public ISelect<TIn, TOut> Throw()
				=> Start.A.Selection<TIn>().By.Returning(A.Result(DefaultComponentLocator<TOut>.Default));

			public ISelect<TIn, TOut> Default(TOut instance) => Default(instance.Start().ToDelegate());

			public ISelect<TIn, TOut> Default(Func<TOut> result)
				=> Start.A.Selection<TIn>().By.Returning(A.Result(new Component<TOut>(result)));
		}
	}
}