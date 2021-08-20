﻿using DragonSpark.Application.Entities.Queries.Materialize;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Scoped.Materialize
{
	public class ToList<TIn, TEntity> : Materialize<TIn, TEntity, List<TEntity>>
	{
		public ToList(IQuery<TIn, TEntity> query) : this(query, DefaultToList<TEntity>.Default) {}

		protected ToList(IQuery<TIn, TEntity> query, IMaterializer<TEntity, List<TEntity>> materializer)
			: base(query, materializer) {}
	}
}
