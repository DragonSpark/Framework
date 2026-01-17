using System;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct ForwardedType(Type Previous, Type Current);