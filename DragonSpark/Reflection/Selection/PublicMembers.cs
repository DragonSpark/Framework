using DragonSpark.Model.Selection;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Reflection.Selection
{
	public sealed class PublicMembers : Select<TypeInfo, IEnumerable<MemberInfo>>
	{
		public static PublicMembers Default { get; } = new PublicMembers();

		PublicMembers() : base(x => x.DeclaredMembers) {}
	}
}