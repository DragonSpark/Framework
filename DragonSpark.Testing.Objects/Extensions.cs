using AutoFixture;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Testing.Objects
{
	public static class Extensions
	{
		public static ISelect<None, IEnumerable<T>> Many<T>(this IResult<IFixture> @this, uint count)
			=> @this.Then().Select(new Many<T>(count)).Accept().Get();
	}
}