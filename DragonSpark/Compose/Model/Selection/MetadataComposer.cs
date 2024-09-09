using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System.Reflection;

namespace DragonSpark.Compose.Model.Selection;

public class MetadataComposer<T> : Composer<T, TypeInfo>
{
	public MetadataComposer(ISelect<T, TypeInfo> subject) : base(subject) {}

	public TypeComposer<T> AsType() => new(Select(TypeSelector.Default).Get());
}