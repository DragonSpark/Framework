using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Components;

public sealed class UniqueIdentifiers : SelectedResult<Guid, string>
{
	public static UniqueIdentifiers Default { get; } = new();

	UniqueIdentifiers() : base(Guid.NewGuid, GenerateElementIdentifier.Default.Get) {}
}