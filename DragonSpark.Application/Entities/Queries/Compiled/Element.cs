using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class Element<TIn, T> : Allocating<In<TIn>, T>, IElement<TIn, T>
{
	public Element(IInstance<TIn, T> instance) : this(instance.Get()) {}

	public Element(Expression<Func<DbContext, TIn, T>> expression) : base(Compiler<TIn, T>.Default.Get(expression)) {}
}