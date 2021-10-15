using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Components;

sealed class ClientIdentifier : Instance<Guid>, IClientIdentifier
{
	public ClientIdentifier() : base(Guid.NewGuid()) {}
}