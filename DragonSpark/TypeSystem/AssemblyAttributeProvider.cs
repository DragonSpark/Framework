using System;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.TypeSystem.Metadata;

namespace DragonSpark.TypeSystem
{
	public class AssemblyAttributeProvider : AttributeProviderBase
	{
		readonly Assembly assembly;
		public AssemblyAttributeProvider( Assembly assembly )
		{
			this.assembly = assembly;
		}

		public override bool Contains( Type attributeType ) => assembly.IsDefined( attributeType );

		public override IEnumerable<Attribute> GetAttributes( Type attributeType ) => assembly.GetCustomAttributes( attributeType );
	}
}