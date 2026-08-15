using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;

namespace DragonSpark.Application.Environment.Development;

sealed class NonModelProjectionTrackingGuardInterceptor : IQueryExpressionInterceptor, IMaterializationInterceptor
{
	public static NonModelProjectionTrackingGuardInterceptor Default { get; } = new();

	NonModelProjectionTrackingGuardInterceptor()
		: this(CurrentQueryContext.Default, SequenceElementType.Default, IdentityType.Default) {}

	readonly IMutable<QueryContext> _type;
	readonly IAlteration<Type>      _sequence;
	readonly ICondition<Type>       _identity;

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