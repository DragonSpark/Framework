using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Selection;

public sealed class AllAssemblyTypes : Instances<Type>, IActivateUsing<Assembly>, IActivateUsing<Type>
{
	public AllAssemblyTypes(Type referenceType) : this(referenceType.Assembly) {}

	public AllAssemblyTypes(Assembly assembly) : base(assembly.DefinedTypes) {}
}

public sealed class AllAssemblyTypes<T> : ArrayResult<Type>
{
	public static AllAssemblyTypes<T> Default { get; } = new AllAssemblyTypes<T>();

	AllAssemblyTypes() : base(new AllAssemblyTypes(typeof(T))) {}
}