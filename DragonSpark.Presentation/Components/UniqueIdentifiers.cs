using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Components
{
	public sealed class UniqueIdentifiers : DelegatedSelection<Guid, string>
	{
		public static UniqueIdentifiers Default { get; } = new UniqueIdentifiers();

		UniqueIdentifiers() : base(UniqueIdentifier.Default.Get, Guid.NewGuid) {}
	}
}