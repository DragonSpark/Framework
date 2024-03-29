﻿using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Selection;

[UsedImplicitly]
public sealed class PublicAssemblyTypes<T> : ArrayResult<Type>
{
	public static PublicAssemblyTypes<T> Default { get; } = new();

	PublicAssemblyTypes() : base(new PublicAssemblyTypes(typeof(T))) {}
}

public sealed class PublicAssemblyTypes : Instances<Type>, IActivateUsing<Assembly>, IActivateUsing<Type>
{
	public PublicAssemblyTypes(Type referenceType) : this(referenceType.Assembly) {}

	public PublicAssemblyTypes(Assembly assembly) : base(assembly.ExportedTypes) {}
}