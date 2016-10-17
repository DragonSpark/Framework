using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;

namespace DragonSpark.Windows.Legacy.Markup
{
	public sealed class StringDesignerValueFactory : SpecificationParameterizedSource<Type, object>
	{
		public static StringDesignerValueFactory Default { get; } = new StringDesignerValueFactory();
		StringDesignerValueFactory() : base( TypeAssignableSpecification<string>.Default, type => type.AssemblyQualifiedName ) {}
	}
}