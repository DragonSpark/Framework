﻿using DragonSpark.Model.Selection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries
{
	class Count<T> : Querying<T, uint>, ICount<T>
	{
		public Count(ISelect<IQueryable<T>, ValueTask<uint>> select) : base(select) {}

		public Count(Func<IQueryable<T>, ValueTask<uint>> select) : base(select) {}
	}
}