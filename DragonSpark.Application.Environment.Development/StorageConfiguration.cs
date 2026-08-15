using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;

namespace DragonSpark.Application.Environment.Development;

public sealed class StorageConfiguration : AspNet.Entities.Configure.StorageConfiguration
{
	public static StorageConfiguration Default { get; } = new();

	StorageConfiguration()
		: base(x => x.AddInterceptors(AzureAdAuthenticationDbConnectionInterceptor.Default/*,
									  NonModelProjectionTrackingGuardInterceptor.Default*/)
					 .EnableSensitiveDataLogging()
					 .EnableDetailedErrors()
					 .ConfigureWarnings(y => y.Throw(RelationalEventId.MultipleCollectionIncludeWarning)
											  .Ignore(RelationalEventId.PendingModelChangesWarning))) {}
}

// TODO

sealed class CurrentQueryContext : Logical<QueryContext>
{
	public static CurrentQueryContext Default { get; } = new();

	CurrentQueryContext() {}
}

sealed class QueryTrackingBehaviorVisitor : ExpressionVisitor
{
	public QueryTrackingBehavior? Behavior { get; private set; }

	protected override Expression VisitMethodCall(MethodCallExpression node)
	{
		var methodName = node.Method.Name;

		if (methodName == nameof(EntityFrameworkQueryableExtensions.AsNoTracking))
		{
			Behavior = QueryTrackingBehavior.NoTracking;
			return node; // Found it!
		}

		if (methodName == nameof(EntityFrameworkQueryableExtensions.AsNoTrackingWithIdentityResolution))
		{
			Behavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
			return node;
		}

		if (methodName == nameof(EntityFrameworkQueryableExtensions.AsTracking))
		{
			Behavior = QueryTrackingBehavior.TrackAll;
			return node;
		}

		return base.VisitMethodCall(node);
	}
}

public static class QueryExpressionExtensions
{
	public static QueryTrackingBehavior? GetQueryTrackingBehavior(this Expression expression)
	{
		var visitor = new QueryTrackingBehaviorVisitor();
		visitor.Visit(expression);
		return visitor.Behavior.HasValue ? visitor.Behavior.Value : null;
	}
}
sealed class IdentityType : ICondition<Type>
{
	public static IdentityType Default { get; } = new();

	IdentityType() : this("Microsoft.AspNetCore.Identity") {}

	readonly string _namespace;

	public IdentityType(string @namespace) => _namespace = @namespace;

	public bool Get(Type parameter) => parameter.Namespace?.StartsWith(_namespace) == true;
}

public readonly record struct QueryContext(Type Type, QueryTrackingBehavior Behavior);
sealed class NonModelProjectionTrackingGuardInterceptor : IQueryExpressionInterceptor, IMaterializationInterceptor
{
	public static NonModelProjectionTrackingGuardInterceptor Default { get; } = new();

	NonModelProjectionTrackingGuardInterceptor()
		: this(CurrentQueryContext.Default, SequenceElementType.Default, IdentityType.Default) {}

	readonly IMutable<QueryContext> _type;
	readonly IAlteration<Type>       _sequence;
	readonly ICondition<Type>        _identity;

	public NonModelProjectionTrackingGuardInterceptor(IMutable<QueryContext> type, IAlteration<Type> sequence,
													  ICondition<Type> identity)
	{
		_type     = type;
		_sequence = sequence;
		_identity = identity;
	}

	public object InitializedInstance(MaterializationInterceptionData materializationData, object instance)
	{
		var context  = materializationData.Context;
		var (rootType, behavior) = _type.Get();

		if (context is not null && rootType is not null && behavior == QueryTrackingBehavior.TrackAll)
		{
			var model    = context.Model;
			var root     = model.FindEntityType(rootType) is null;
			var entity   = materializationData.EntityType is not null;
			var identity = _identity.Get(instance.GetType()) || _identity.Get(rootType);

			if (root && entity && !identity)
			{
				throw new InvalidOperationException($"[EF CORE TRACKING GUARD] Query projecting to non-model DTO '{rootType.Name}' " +
													$"is materializing and tracking domain entity '{instance.GetType().Name}'! " +
													$"Apply .AsNoTracking() or .AsNoTrackingWithIdentityResolution().");
			}
		}

		return instance;
	}

	public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData)
	{
		var behavior = queryExpression.GetQueryTrackingBehavior() ??
					   eventData.Context.Verify().ChangeTracker.QueryTrackingBehavior;
		_type.Execute(new(_sequence.Get(queryExpression.Type), behavior));
		return queryExpression;
	}
}

sealed class SequenceElementType : IAlteration<Type>
{
	public static SequenceElementType Default { get; } = new();

	SequenceElementType() : this(typeof(IEnumerable<>), typeof(IQueryable<>), typeof(IEnumerable<>)) {}

	readonly Type   _definition;
	readonly Type[] _definitions;

	public SequenceElementType(Type definition, params Type[] definitions)
	{
		_definition  = definition;
		_definitions = definitions;
	}

	public Type Get(Type parameter)
	{
		if (parameter.IsGenericType && _definitions.Contains(parameter.GetGenericTypeDefinition()))
		{
			return parameter.GetGenericArguments()[0];
		}

		var interfaces = parameter.GetInterfaces();
		for (int i = 0; i < interfaces.Length; i++)
		{
			if (interfaces[i].IsGenericType && interfaces[i].GetGenericTypeDefinition() == _definition)
			{
				return interfaces[i].GetGenericArguments()[0];
			}
		}

		return parameter;
	}
}