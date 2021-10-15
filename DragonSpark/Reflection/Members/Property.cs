using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public readonly record struct Property<T>(PropertyInfo Metadata, T Attribute) where T : Attribute;