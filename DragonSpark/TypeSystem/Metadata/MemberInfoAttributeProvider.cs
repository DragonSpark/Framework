using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	public class MemberInfoAttributeProvider : AttributeProviderBase
	{
		readonly MemberInfo info;
		readonly bool inherit;

		public MemberInfoAttributeProvider( MemberInfo info, bool inherit = false )
		{
			this.info = info;
			this.inherit = inherit;
		}

		public override bool Contains( Type attributeType ) => info.IsDefined( attributeType, inherit );

		public override IEnumerable<Attribute> GetAttributes( Type attributeType ) => info.GetCustomAttributes( attributeType, inherit );
	}
}