using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace DragonSpark.Application.Environment.Development;

sealed class NonModelProjectionTrackingGuardInterceptor : IQueryExpressionInterceptor, IMaterializationInterceptor
{
	public static NonModelProjectionTrackingGuardInterceptor Default { get; } = new();

	NonModelProjectionTrackingGuardInterceptor()
		: this(CurrentQueryContext.Default, SequenceElementType.Default, IdentityType.Default) {}

	readonly IMutable<QueryContext?> _type;
	readonly IAlteration<Type>       _sequence;
	readonly ICondition<Type>        _identity;

	public NonModelProjectionTrackingGuardInterceptor(IMutable<QueryContext?> type, IAlteration<Type> sequence,
	                                                  ICondition<Type> identity)
	{
		_type     = type;
		_sequence = sequence;
		_identity = identity;
	}

	public object InitializedInstance(MaterializationInterceptionData materializationData, object instance)
	{
		var type = _type.Get();
		if (type is not null)
		{
			var (rootType, behavior) = type.Value;
			switch (behavior)
			{
				case QueryTrackingBehavior.TrackAll:
					var model    = materializationData.Context.Model;
					var track    = ShouldTrack.Default.Get(new(model, rootType));
					var entity   = materializationData.EntityType is not null;
					var identity = _identity.Get(instance.GetType()) || _identity.Get(rootType);

					if (!track && entity && !identity)
					{
						throw new
							InvalidOperationException($"[EF CORE TRACKING GUARD] Query projecting to non-model DTO '{rootType.Name}' " +
							                          $"is materializing and tracking domain entity '{instance.GetType().Name}'! " +
							                          $"Apply .AsNoTracking() or .AsNoTrackingWithIdentityResolution().");
					}

					break;
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

public readonly record struct ShouldTrackInput(IModel Model, Type Type);

sealed class ShouldTrack : ICondition<ShouldTrackInput>
{
	public static ShouldTrack Default { get; } = new();

	ShouldTrack() {}

	public bool Get(ShouldTrackInput parameter)
	{
		var (model, type) = parameter;
		var constructors = type.GetConstructors().SelectMany(c => c.GetParameters().Select(x => x.ParameterType));
		var properties   = type.GetProperties().Select(x => x.PropertyType);
		foreach (var candidate in constructors.Prepend(type).Union(properties).Distinct())
		{
			if (model.FindEntityType(candidate) is not null)
			{
				return true;
			}
		}

		return false;
	}
}