using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Runtime;
using JetBrains.Annotations;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class Members : ILease<Expression, MemberInfo>
{
	public static Members Default { get; } = new();

	Members() {}

	[MustDisposeResource]
	public Leasing<MemberInfo> Get(Expression parameter)
	{
		using var builder = ArrayBuilder.New<MemberInfo>(32);
		while (parameter is MemberExpression m)
		{
			builder.Add(m.Member);
			parameter = m.Expression!;
		}

		builder.AsSpan().Reverse();
		return builder.AsLease();
	}
}