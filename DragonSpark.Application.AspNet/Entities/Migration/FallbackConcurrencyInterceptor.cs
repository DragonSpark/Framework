using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public sealed class FallbackConcurrencyInterceptor : SaveChangesInterceptor
{
	public static FallbackConcurrencyInterceptor Default { get; } = new();

	FallbackConcurrencyInterceptor() : this(ApplyChanges.Default) {}

	readonly ICommand<DbContext> _apply;

	public FallbackConcurrencyInterceptor(ICommand<DbContext> apply) => _apply = apply;

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		if (eventData.Context is {} context)
		{
			_apply.Execute(context);
		}

		return base.SavingChanges(eventData, result);
	}

	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		if (eventData.Context is {} context)
		{
			_apply.Execute(context);
		}

		return await base.SavingChangesAsync(eventData, result, cancellationToken).Off();
	}
}