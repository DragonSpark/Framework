using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Runtime.Activation;

public sealed class NewInstance :Select<Type, object>
{
	public static NewInstance Default { get; } = new();

	NewInstance() : base(NewInstance<object>.Default) {}
}

public sealed class NewInstance<T> : Select<Type, T>
{
	public static NewInstance<T> Default { get; } = new();

	NewInstance() : base(Start.A.Selection(TypeMetadata.Default)
	                          .Select(ConstructorLocator.Default)
	                          .Select(Constructors<T>.Default)
	                          .Then()
	                          .Invoke()) {}
}