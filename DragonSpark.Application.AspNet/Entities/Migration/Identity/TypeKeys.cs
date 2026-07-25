using DragonSpark.Model.Selection;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class TypeKeys : Select<Type, ImmutableHashSet<object>>, ITypeKeys
{
	public TypeKeys(ISelect<Type, ImmutableHashSet<object>> select) : base(select) {}
}