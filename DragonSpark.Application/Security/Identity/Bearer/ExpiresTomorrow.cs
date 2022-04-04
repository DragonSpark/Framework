using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class ExpiresTomorrow : Result<DateTime>
{
	public static ExpiresTomorrow Default { get; } = new();

	ExpiresTomorrow() : base(Expires.Default.Then().Bind(1)) {}
}