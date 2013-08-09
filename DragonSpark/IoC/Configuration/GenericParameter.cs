using System;

namespace DragonSpark.IoC.Configuration
{
	public class GenericParameter : GenericParameterBase
	{
		public string ResolutionKeyName { get; set; }

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var result = new Microsoft.Practices.Unity.GenericParameter( GenericParameterName, ResolutionKeyName );
			return result;
		}
	}
}