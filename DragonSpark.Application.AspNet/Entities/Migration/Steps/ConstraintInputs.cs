using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class ConstraintInputs : ReferenceStoring<DatabaseFacade, ConstraintInput>
{
	public static ConstraintInputs Default { get; } = new();

	ConstraintInputs() : base(ComposeConstraintInput.Default) {}
}