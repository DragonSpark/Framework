using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using System.ComponentModel;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class DefaultValuePropertySpecification : SpecificationBase<PropertyInfo>
	{
		public static DefaultValuePropertySpecification Instance { get; } = new DefaultValuePropertySpecification();

		protected override bool Verify( PropertyInfo parameter ) => parameter.Has<DefaultValueAttribute>() || parameter.Has<DefaultValueBase>();
	}
}