using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Reflection.Types;

public class TypedTable<T> : DecoratedTable<TypeInfo, T>, ITypedTable<T>
{
	public TypedTable() : this(new Dictionary<TypeInfo,T>()) {}

	public TypedTable(IDictionary<TypeInfo, T> source) : base(source.ToTable()) {}

	public TypedTable(ITable<TypeInfo, T> source) : base(source) {}
}