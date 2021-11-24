using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Diagnostics;

sealed class Exceptions : Operation<ExceptionInput>, IExceptions
{
	public Exceptions(IExceptionLogger select) : base(select.Then().Terminate()) {}
}