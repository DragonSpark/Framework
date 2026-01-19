using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public static class Extensions
{
	public static IMigrationSteps WithConstraintManagement(this IMigrationSteps @this, DbContext destination)
		=> new ConstraintAwareMigrationSteps(@this, destination.Database);
}