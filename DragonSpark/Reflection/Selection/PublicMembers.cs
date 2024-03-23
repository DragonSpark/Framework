using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Reflection.Selection;

[UsedImplicitly]
public sealed class PublicMembers : Select<TypeInfo, IEnumerable<MemberInfo>>
{
	public static PublicMembers Default { get; } = new();

	PublicMembers() : base(x => x.DeclaredMembers) {}
}