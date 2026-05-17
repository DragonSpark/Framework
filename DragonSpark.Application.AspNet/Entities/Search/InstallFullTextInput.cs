using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Search;

public readonly record struct InstallFullTextInput(IEntityType Type, string Table);