using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public readonly record struct ComposeContainsInput(IEntityType Metadada, Array<object> Keys);