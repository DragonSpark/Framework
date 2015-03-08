using System;

namespace DragonSpark.Application.Presentation.Configuration
{
    public class TypeFullName : Type
	{
		protected override object ResolveValue( IServiceProvider serviceProvider, System.Type type )
		{
			return type.FullName;
		}
	}
}