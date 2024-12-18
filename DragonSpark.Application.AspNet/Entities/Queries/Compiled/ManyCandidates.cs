﻿using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled;

sealed class ManyCandidates<TIn, TOut> : Instances<ManyGeneric<TIn, TOut>>
{
	public static ManyCandidates<TIn, TOut> Default { get; } = new();

	ManyCandidates() : this(typeof(Many<,,>), typeof(Many<,,,>), typeof(Many<,,,,>),
	                    typeof(Many<,,,,,>), typeof(Many<,,,,,,>), typeof(Many<,,,,,,,>),
	                    typeof(Many<,,,,,,,,>), typeof(Many<,,,,,,,,,>), typeof(Many<,,,,,,,,,,>)) {}

	public ManyCandidates(params Type[] types)
		: base(types.AsValueEnumerable().Select(x => new ManyGeneric<TIn, TOut>(x)).ToArray()) {}
}