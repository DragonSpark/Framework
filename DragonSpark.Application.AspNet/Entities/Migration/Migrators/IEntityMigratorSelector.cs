using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public interface IEntityMigratorSelector : ISelect<EntityMigratorSelectorInput, IEntityMigrator?> {}