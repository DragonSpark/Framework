﻿using DragonSpark.Application.Entities.Queries.Materialize;

namespace DragonSpark.Application.Entities.Queries.Scoped.Materialize
{
	public class FirstOrDefault<TIn, T> : Materialize<TIn, T, T?>
	{
		public FirstOrDefault(IQuery<TIn, T> query) : this(query, FirstOrDefaultMaterializer<T>.Default) {}

		protected FirstOrDefault(IQuery<TIn, T> query, IMaterializer<T, T?> materializer)
			: base(query, materializer) {}
	}
}