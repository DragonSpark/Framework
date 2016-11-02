using DragonSpark.TypeSystem;
using System;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application
{
	public class ApplicationPublicPartsAttribute : PartsAttributeBase
	{
		protected ApplicationPublicPartsAttribute( Action<MethodBase> initialize ) : base( m => PublicPartsLocator.Default.Get( m.DeclaringType.Assembly ), initialize ) {}
	}
}