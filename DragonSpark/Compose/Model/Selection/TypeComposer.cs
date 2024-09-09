using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System;
using Activator = DragonSpark.Runtime.Activation.Activator;

namespace DragonSpark.Compose.Model.Selection;

public class TypeComposer<T> : Composer<T, Type>
{
	public TypeComposer(ISelect<T, Type> subject) : base(subject) {}

	public Composer<T, object> Activate() => Select(Activator.Default);

	public Composer<T, TOut> Activate<TOut>() => Activate().Cast<TOut>();

	public MetadataComposer<T> Metadata() => new(Select(TypeMetadata.Default).Get());
}