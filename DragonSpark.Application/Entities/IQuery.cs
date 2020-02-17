using DragonSpark.Model.Selection;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities {
	public interface IQuery<in TKey, TEntity> : ISelect<TKey, Expression<Func<TEntity, bool>>> {}
}