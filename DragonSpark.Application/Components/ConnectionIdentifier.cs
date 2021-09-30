using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Components
{
	public sealed class ConnectionIdentifier : Instance<Guid>
	{
		public ConnectionIdentifier() : base(Guid.NewGuid()) {}
	}
}