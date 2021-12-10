using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Components;

public sealed class UniqueIdentifiers : SelectedResult<Guid, string>
{
	public static UniqueIdentifiers Default { get; } = new UniqueIdentifiers();

	UniqueIdentifiers() : base(Guid.NewGuid, UniqueIdentifier.Default.Get) {}
}