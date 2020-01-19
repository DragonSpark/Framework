using DragonSpark.Model.Results;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Testing.Objects
{
	sealed class Select : Instance<Func<string, int>>
	{
		public static Select Default { get; } = new Select();

		Select() : this(x => default) {}

		public Select(Expression<Func<string, int>> instance) : base(instance.Compile()) {}
	}
}