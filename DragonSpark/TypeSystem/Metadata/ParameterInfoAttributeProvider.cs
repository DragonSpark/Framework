using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	public sealed class ParameterInfoAttributeProvider : AttributeProviderBase
	{
		readonly ParameterInfo parameter;
		public ParameterInfoAttributeProvider( ParameterInfo parameter )
		{
			this.parameter = parameter;
		}

		public override bool Contains( Type attributeType ) => parameter.IsDefined( attributeType );

		public override IEnumerable<Attribute> GetAttributes( Type attributeType ) => parameter.GetCustomAttributes( attributeType );
	}
}