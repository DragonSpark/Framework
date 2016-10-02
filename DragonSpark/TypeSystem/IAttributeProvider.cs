using System;
using System.Collections.Generic;

namespace DragonSpark.TypeSystem
{
	public interface IAttributeProvider
	{
		bool Contains( Type attributeType );

		IEnumerable<Attribute> GetAttributes( Type attributeType );
	}
}