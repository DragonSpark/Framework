using System;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct ChangeTypeInput(object Value, Type SourceType, Type TargetType);