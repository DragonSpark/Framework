using System;
using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Values" )]
	public class ResolvedArrayParameter : InjectionParameterValue
	{
		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ElementType { get; set; }

		public InjectionParameterValueCollection Values
		{
			get { return values ?? ( values = new InjectionParameterValueCollection() ); }
		}	InjectionParameterValueCollection values;

		public override Microsoft.Practices.Unity.InjectionParameterValue Create( Type targetType )
		{
			var injectionParameterValues = Values.Resolve( ElementType ?? targetType );
			var result = new Microsoft.Practices.Unity.ResolvedArrayParameter( ElementType, injectionParameterValues );
			return result;
		}
	}
}