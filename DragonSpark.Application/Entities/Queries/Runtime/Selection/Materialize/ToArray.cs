﻿using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection.Materialize
{
	public class ToArray<TIn, TEntity> : Materialize<TIn, TEntity, Array<TEntity>>
	{
		public ToArray(IQuery<TIn, TEntity> query) : this(query, DefaultToArray<TEntity>.Default) {}

		protected ToArray(IQuery<TIn, TEntity> query, IMaterializer<TEntity, Array<TEntity>> materializer)
			: base(query, materializer) {}
	}
}