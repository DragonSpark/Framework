using DragonSpark.Compose;
using DragonSpark.Reflection.Collections;
using System;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class MemberAssembly : DragonSpark.Model.Selection.Coalesce<Type, Assembly>
	{
		public static MemberAssembly Default { get; } = new MemberAssembly();

		MemberAssembly() : base(Start.A.Selection.Of.System.Type.By.Self.Then()
		                             .Metadata()
		                             .Select(CollectionInnerType.Default)
		                             .Select(x => x?.Assembly), x => x.Assembly) {}
	}
}