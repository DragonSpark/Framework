using System;

namespace DragonSpark.Presentation.Interaction
{
	public class UniqueIdentityResult : SuccessResult<Guid>
	{
		public UniqueIdentityResult(Guid instance) : base(instance) {}
	}
}
