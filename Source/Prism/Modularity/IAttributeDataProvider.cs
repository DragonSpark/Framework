using System;
using System.Collections.Generic;

namespace Prism.Modularity
{
	public interface IAttributeDataProvider
	{
		T Get<T>( Type attributeType, Type type, string name );

		IEnumerable<T> GetAll<T>( Type attributeType, Type type, string name );
	}
}