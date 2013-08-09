using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace DragonSpark.IoC.Configuration
{
	public class InjectionParameterValueCollection : ObservableCollection<InjectionParameterValue>
	{
		public Microsoft.Practices.Unity.InjectionParameterValue[] Resolve( Type targetType )
		{
			var query = from parameterValue in this select parameterValue.Create( targetType );
			var result = query.ToArray();
			return result;
		}
	}
}