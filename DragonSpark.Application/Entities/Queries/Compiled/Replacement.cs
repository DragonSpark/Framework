using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

readonly record struct Replacement(Type ResultType, Delegate Delegate, ParameterExpression Parameter);