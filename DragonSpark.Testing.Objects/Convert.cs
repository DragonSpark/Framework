using DragonSpark.Model.Results;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Testing.Objects
{
	sealed class Convert : Instance<Converter<string, int>>
	{
		public static Convert Default { get; } = new Convert();

		Convert() : this(x => default) {}

		public Convert(Expression<Converter<string, int>> instance) : base(instance.Compile()) {}
	}
}