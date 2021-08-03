using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System;
using Activator = DragonSpark.Runtime.Activation.Activator;

namespace DragonSpark.Compose.Model.Selection
{
	public class TypeSelector<T> : Selector<T, Type>
	{
		public TypeSelector(ISelect<T, Type> subject) : base(subject) {}

		public Selector<T, object> Activate() => Select(Activator.Default);

		public Selector<T, TOut> Activate<TOut>() => Activate().Cast<TOut>();

		public MetadataSelector<T> Metadata() => new MetadataSelector<T>(Select(TypeMetadata.Default).Get());
	}
}