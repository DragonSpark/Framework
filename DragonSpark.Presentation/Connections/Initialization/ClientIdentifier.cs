using DragonSpark.Application.Components;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Connections.Initialization
{
	sealed class ClientIdentifier : CoalesceStructure<Guid>, IClientIdentifier
	{
		public ClientIdentifier(IClientIdentifier previous, ClientIdentifierAccessor accessor)
			: base(accessor, previous) {}
	}
}