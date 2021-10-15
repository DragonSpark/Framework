using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System.Reflection;

namespace DragonSpark.Compose.Model.Selection;

public class MetadataSelector<T> : Selector<T, TypeInfo>
{
	public MetadataSelector(ISelect<T, TypeInfo> subject) : base(subject) {}

	public TypeSelector<T> AsType() => new TypeSelector<T>(Select(TypeSelector.Default).Get());
}