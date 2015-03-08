using System;

namespace DragonSpark.Application.Communication.Configuration
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
	public sealed class ProxyCodeGenerationAttribute : Attribute
	{
		readonly Type targetType;

		public ProxyCodeGenerationAttribute( Type targetType )
		{
			this.targetType = targetType;
		}

		public Type TargetType
		{
			get { return targetType; }
		}
	}
}