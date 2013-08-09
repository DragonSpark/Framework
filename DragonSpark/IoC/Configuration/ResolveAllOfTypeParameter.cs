using System;

namespace DragonSpark.IoC.Configuration
{
	public class ResolveAllOfTypeParameter : InjectionParameterValue
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ElementType { get; set; }

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var result = new IoC.ResolveAllOfTypeParameter( ElementType );
			return result;
		}
	}
}