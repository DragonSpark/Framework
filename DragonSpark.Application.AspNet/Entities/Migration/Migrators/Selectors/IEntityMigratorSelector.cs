using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

public interface IEntityMigratorSelector : ISelect<EntityMigratorSelectorInput, IEntityMigrator?> {}