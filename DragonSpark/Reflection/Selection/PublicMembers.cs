using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Selection
{
	public sealed class PublicMembers : Select<TypeInfo, IEnumerable<MemberInfo>>
	{
		public static PublicMembers Default { get; } = new PublicMembers();

		PublicMembers() : base(x => x.DeclaredMembers) {}
	}
}