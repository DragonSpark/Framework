using System;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Testing.Framework.Setup
{
	public class LocateTypeSpecification : CanLocateSpecification
	{
		readonly Type typeToLocate;

		public LocateTypeSpecification( IServiceLocator locator, Type typeToLocate ) : base( locator )
		{
			this.typeToLocate = typeToLocate;
		}

		protected override bool CanLocate( Type type )
		{
			var result = typeToLocate == type && base.CanLocate( type );
			return result;
		}
	}
}