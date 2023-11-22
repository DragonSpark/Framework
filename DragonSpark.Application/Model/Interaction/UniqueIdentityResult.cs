using System;

namespace DragonSpark.Application.Model.Interaction;

public class UniqueIdentityResult : SuccessResult<Guid>
{
	public UniqueIdentityResult(Guid instance) : base(instance) {}
}