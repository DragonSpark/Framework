using DragonSpark.Model.Selection;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

public interface IEntityComparison : ISelect<EntityDefinitionInput, ModifiedEntityComparisonResult>;