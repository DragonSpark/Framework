using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using Exception = System.Exception;

namespace DragonSpark.Application.Diagnostics
{
	sealed class Exceptions : Operation<(Type Owner, Exception Exception)>, IExceptions
	{
		public Exceptions(IExceptionLogger select) : base(select.Then().Terminate()) {}
	}
}