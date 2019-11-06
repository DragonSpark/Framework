﻿using System.Collections.Generic;
using AutoFixture;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;

namespace DragonSpark.Testing.Objects
{
	public static class Extensions
	{
		public static ISelect<None, IEnumerable<T>> Many<T>(this IResult<IFixture> @this, uint count)
			=> @this.ToSelect().Select(new Many<T>(count));
	}
}