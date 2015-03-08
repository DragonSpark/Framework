using System;

namespace DragonSpark.IoC.Configuration
{
	public class GenericResolvedArrayParameter : GenericParameterBase
	{
		public InjectionParameterValueCollection Values
		{
			get { return values ?? ( values = new InjectionParameterValueCollection() ); }
		}	InjectionParameterValueCollection values;

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var result = new Microsoft.Practices.Unity.GenericResolvedArrayParameter( GenericParameterName, Values.Resolve( targetType ) );
			return result;
		}
	}
}