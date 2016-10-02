using System.Composition;
using System.Reflection;
using DragonSpark.Specifications;

namespace DragonSpark.Composition
{
	public sealed class IsExportSpecification : SpecificationBase<MemberInfo>
	{
		public static IsExportSpecification Default { get; } = new IsExportSpecification();
		IsExportSpecification() {}

		public override bool IsSatisfiedBy( MemberInfo parameter ) => parameter.IsDefined( typeof(ExportAttribute) );
	}
}