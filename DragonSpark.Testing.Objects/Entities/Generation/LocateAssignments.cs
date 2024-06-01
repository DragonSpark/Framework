using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Testing.Objects.Entities.Generation;

sealed class LocateAssignments<T, TOther> : ReferenceValueStore<string, IResult<Action<T, TOther>?>>
{
	public static LocateAssignments<T, TOther> Default { get; } = new();

	LocateAssignments() : base(Text.Intern.Default.Select(x => new PrincipalPropertyByName(x))
	                               .Select(x => new LocatePrincipalProperty<T, TOther>(x))
	                               .Select(x => new LocateAssignment<T, TOther>(x))) {}
}