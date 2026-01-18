using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public static class Extensions
{
	public static IMigration WithConstraintManagement(this IMigration @this, DbContext destination)
		=> new ConstraintAwareMigration(@this, destination.Database);
}