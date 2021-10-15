using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

sealed class ProviderAwareAmbientContext : Maybe<DbContext>, IAmbientContext
{
	public ProviderAwareAmbientContext(IAmbientContext first) : this(first, AmbientProvidedContext.Default) {}

	public ProviderAwareAmbientContext(IAmbientContext first, IResult<DbContext?> second) : base(first, second) {}
}