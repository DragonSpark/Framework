using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class LocateAssignments<T, TOther> : ReferenceValueStore<string, IResult<Action<T, TOther>?>>
	{
		public static LocateAssignments<T, TOther> Default { get; } = new LocateAssignments<T, TOther>();

		LocateAssignments() : base(Start.A.Selection<string>()
		                                .By.Calling(string.Intern)
		                                .Select(x => new PrincipalPropertyByName(x))
		                                .Select(x => new LocatePrincipalProperty<T, TOther>(x))
		                                .Select(x => new LocateAssignment<T, TOther>(x))) {}
	}
}